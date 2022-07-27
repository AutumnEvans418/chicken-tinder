using ChickenTinder.Client.Data;

namespace ChickenTinder.Server.Services;

public class RoomService
{
    private readonly RestaurantService _reastaurantService;
    private readonly MatchService _matchService;
    private readonly UserService userService;
    private readonly Dictionary<int, DiningRoom> _rooms = new();
    public List<UserTimer> Timers { get; set; } = new();
    public RoomService(RestaurantService restaurantService, MatchService match, UserService userService)
    {
        _matchService = match;
        this.userService = userService;
        _reastaurantService = restaurantService;
    }

    public DiningRoom? GetRoom(int Id)
    {
        if (_rooms.TryGetValue(Id, out var room))
        {
            return room;
        }
        return null;
    }

    public async Task<DiningRoom?> CreateRoom(User user)
    {
        if (user is null)
            return null;
        var locations = !string.IsNullOrEmpty(user.Longitude) && !string.IsNullOrEmpty(user.Latitude)
                                            ? await _reastaurantService.GetRestaurants(user.Latitude, user.Longitude)
                                            : await _reastaurantService.GetRestaurants(user.Location);

        user = await UpdateUser(user, null, new List<string>()) ?? throw new Exception("user cannot be null");


        if (locations is not null)
        {
            DiningRoom room = new(user, locations)
            {
                ID = new Random().Next(0, 99999)
            };
            var timer = new UserTimer(user);
            timer.OnExpired = u => RemoveExpiredUsers(u, room, timer);
            Timers.Add(timer);
            if (!_rooms.ContainsKey(room.ID))
            {
                _rooms.Add(room.ID, room);
                return room;
            }
        }

        return null;
    }


    private async Task<User?> UpdateUser(User user, DiningRoom? room, List<string> users)
    {
        var existingUser = room?.GetUser(user.Id);
        if (existingUser != null && room != null)
        {
            var timer = Timers.First(p => p.User.Id == user.Id);
            timer.ResetTimeout();
            timer.OnExpired = u => RemoveExpiredUsers(u, room, timer);
            existingUser.SignalRConnection = user.SignalRConnection;
            return null;
        }
        var data = await userService.GetRandomUser(users);
        
        user.Name = data.Name;
        user.Class = data.Class;
        user.Color = data.Color;
        return user;
    }

    public async Task<DiningRoom?> JoinRoom(User user, int roomId)
    {
        if (_rooms.TryGetValue(roomId, out var room))
        {
            var result = await UpdateUser(user, room, room.Users.Select(p => p.Name).ToList());
            if (result != null)
            {
                var timer = new UserTimer(result);
                timer.OnExpired = u => RemoveExpiredUsers(u, room, timer);
                Timers.Add(timer);
                room.Join(result);
            }
            return room;
        }
        return null;
    }

    public bool SetPickyUser(int roomId, string userId)
    {
        if (_rooms.TryGetValue(roomId, out var room))
        {
            var timer = Timers.First(p => p.User.Id == userId);
            timer.ResetTimeout();
            room.SetPickyUser(userId);
            return _matchService.CheckForMatch(room);
        }
        return false;
    }

    public void LeaveRoom(User user, int roomId)
    {
        if (_rooms.TryGetValue(roomId, out var room))
        {
            room.Leave(user);
        }
    }

    public void SetUserVoid(int roomId, string userId)
    {
        if (_rooms.TryGetValue(roomId, out var room))
        {
            room.VoidUser(userId);
        }
    }

    public bool Vote(int roomId, string userId, string RestaurantId, UserAction votes)
    {
        if (_rooms.TryGetValue(roomId, out var room))
        {
            if (room.UserExist(userId))
            {
                var restaurant = room.GetRestaurant(RestaurantId);
                var user = room.GetUser(userId);
                if (user == null)
                    throw new Exception("user was not found");
                if (restaurant == null)
                    throw new Exception("restaurant was not found");

                var timer = Timers.First(t => t.User.Id == user.Id);
                timer.ResetTimeout();

                if (votes == UserAction.No)
                    return false;

                Match match = new(user, restaurant, votes);
                _matchService.AddMatch(room, match);
                return _matchService.CheckForMatch(room);
                
            }
        }

        return false;
    }

    public List<string> Start(int roomId)
    {
        if (_rooms.TryGetValue(roomId, out var room))
        {
            var timers = Timers.Where(t => room.Users.Select(u => u.Id).Contains(t.User.Id)).ToList();

            timers.ForEach(u => u.ResetTimeout());
            return room.Users.Select(x => x.SignalRConnection).ToList();
        }
        return new();
    }

    public List<string> GetUserIds(int roomId)
    {
        if (_rooms.TryGetValue(roomId, out var room))
            return room.Users.Select(x => x.SignalRConnection).ToList();
        else
            return new();
    }

    public void RemoveExpiredUsers(User user, DiningRoom room, UserTimer timer)
    {
        Timers.Remove(timer);
        timer.Dispose();
        room.Leave(user);
        if (room.Users.Count == 0)
        {
            _rooms.Remove(room.ID);
        }
    }
}