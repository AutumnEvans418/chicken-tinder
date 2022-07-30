using ChickenTinder.Client.Data;

namespace ChickenTinder.Server.Services;

public class RoomService
{
    private readonly IRestaurantService _reastaurantService;
    private readonly MatchService _matchService;
    private readonly IUserService userService;
    private readonly Dictionary<int, DisposalTimer<DinningRoom>> _rooms = new();
    private TimeSpan UserTimeout = TimeSpan.FromMinutes(5);
    private TimeSpan RoomTimeout = TimeSpan.FromMinutes(30);
    public List<DisposalTimer<User>> Timers { get; set; } = new();
    public RoomService(
        IRestaurantService restaurantService, 
        MatchService match, 
        IUserService userService)
    {
        _matchService = match;
        this.userService = userService;
        _reastaurantService = restaurantService;
    }

    public DinningRoom? GetRoom(int Id)
    {
        if (_rooms.TryGetValue(Id, out var room))
        {
            return room.Value;
        }
        return null;
    }

    public async Task<DinningRoom?> CreateRoom(User user)
    {
        if (user is null)
            return null;

        var locations = new List<Restaurant>();
        
        if(!string.IsNullOrWhiteSpace(user.Longitude) && !string.IsNullOrWhiteSpace(user.Latitude))
        {
            locations = await _reastaurantService.GetRestaurants(user.Latitude, user.Longitude);
        }
        else if (!string.IsNullOrWhiteSpace(user.Location))
        {
            locations = await _reastaurantService.GetRestaurants(user.Location);
        }
        else
        {
            throw new Exception("location must be set");
        }
        user = await UpdateUser(user, null, new List<string>()) ?? throw new Exception("user cannot be null");


        if (locations is not null)
        {
            DinningRoom room = new(user, locations)
            {
                ID = GetNextId(),
            };
            var timer = new DisposalTimer<User>(user, UserTimeout);
            timer.OnExpired = u => RemoveExpiredUsers(u, room, timer);
            Timers.Add(timer);
            if (!_rooms.ContainsKey(room.ID))
            {
                var dTimer = new DisposalTimer<DinningRoom>(room, RoomTimeout);
                dTimer.ResetTimeout();
                dTimer.OnExpired = d => RemoveRoom(dTimer);
                _rooms.Add(room.ID, dTimer);
                return room;
            }
        }

        return null;
    }
    public int MaxId { get; set; } = 99999;
    public int MinId { get; set; } = 1;
    public int GetNextId()
    {
        var id = new Random().Next(MinId, MaxId);
        if (!_rooms.ContainsKey(id))
            return id;

        var data = Enumerable.Range(MinId, MaxId);
        
        foreach (var item in data)
        {
            if (!_rooms.ContainsKey(item))
            {
                return item;
            }
        }
        var minDate = _rooms.Min(p => p.Value.Value.DateCreated);
        var oldestRoom = _rooms.First(p => p.Value.Value.DateCreated == minDate);
        _rooms.Remove(oldestRoom.Key);
        return oldestRoom.Key;
    }


    public void RemoveRoom(DisposalTimer<DinningRoom> room)
    {
        if (_rooms.ContainsKey(room.Value.ID))
        {
            _rooms.Remove(room.Value.ID);
        }
        room.Dispose();
    }

    private async Task<User?> UpdateUser(User user, DinningRoom? room, List<string> users)
    {
        var existingUser = room?.GetUser(user.Id);
        if (existingUser != null && room != null)
        {
            var timer = Timers.First(p => p.Value.Id == user.Id);
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

    public async Task<DinningRoom?> JoinRoom(User user, int roomId)
    {
        if (_rooms.TryGetValue(roomId, out var room))
        {
            var result = await UpdateUser(user, room.Value, room.Value.Users.Select(p => p.Name).ToList());
            if (result != null)
            {
                var timer = new DisposalTimer<User>(result, UserTimeout);
                timer.OnExpired = u => RemoveExpiredUsers(u, room.Value, timer);
                Timers.Add(timer);
                room.Value.Join(result);
            }
            return room.Value;
        }
        return null;
    }

    public bool SetPickyUser(int roomId, string userId)
    {
        if (_rooms.TryGetValue(roomId, out var room))
        {
            var timer = Timers.First(p => p.Value.Id == userId);
            timer.ResetTimeout();
            room.Value.SetPickyUser(userId);
            return _matchService.CheckForMatch(room.Value);
        }
        return false;
    }

    public void LeaveRoom(User user, int roomId)
    {
        if (_rooms.TryGetValue(roomId, out var room))
        {
            room.Value.Leave(user);
        }
    }

    public void SetUserVoid(int roomId, string userId)
    {
        if (_rooms.TryGetValue(roomId, out var room))
        {
            room.Value.VoidUser(userId);
        }
    }

    public bool Vote(int roomId, string userId, string RestaurantId, UserAction votes)
    {
        if (_rooms.TryGetValue(roomId, out var room))
        {
            if (room.Value.UserExist(userId))
            {
                var restaurant = room.Value.GetRestaurant(RestaurantId);
                var user = room.Value.GetUser(userId);
                if (user == null)
                    throw new Exception("user was not found");
                if (restaurant == null)
                    throw new Exception("restaurant was not found");

                var timer = Timers.First(t => t.Value.Id == user.Id);
                timer.ResetTimeout();

                if (votes == UserAction.No)
                    return false;

                Match match = new(user, restaurant, votes);
                _matchService.AddMatch(room.Value, match);
                return _matchService.CheckForMatch(room.Value);
                
            }
        }

        return false;
    }

    public List<string> Start(int roomId)
    {
        if (_rooms.TryGetValue(roomId, out var room))
        {
            room.Value.Status = RoomStatus.Swiping;
            var timers = Timers.Where(t => room.Value.Users.Select(u => u.Id).Contains(t.Value.Id)).ToList();

            timers.ForEach(u => u.ResetTimeout());
            return room.Value.Users.Select(x => x.SignalRConnection).ToList();
        }
        return new();
    }

    public List<string> GetUserIds(int roomId)
    {
        if (_rooms.TryGetValue(roomId, out var room))
            return room.Value.Users.Select(x => x.SignalRConnection).ToList();
        else
            return new();
    }

    public void RemoveExpiredUsers(User user, DinningRoom room, DisposalTimer<User> timer)
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