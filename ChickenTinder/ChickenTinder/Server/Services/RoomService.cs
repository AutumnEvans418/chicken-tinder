namespace ChickenTinder.Server.Services;

public class RoomService
{
    private readonly RestaurantService _reastaurantService;
    private readonly MatchService _matchService;

    private readonly Dictionary<int, DiningRoom> _rooms = new();


    public RoomService(RestaurantService restaurantService, MatchService match)
    {
        _matchService = match;
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
        if (user is null) return null;

        var locations = !string.IsNullOrEmpty(user.Longitude) && !string.IsNullOrEmpty(user.Latitude) 
                                            ? await _reastaurantService.GetRestaurants(user.Longitude, user.Latitude) 
                                            : await _reastaurantService.GetRestaurants(user.Location);
        if (locations is not null)
        {
            DiningRoom room = new(user, locations)
            {
                ID = new Random().Next(0, 99999)
            };

            if (!_rooms.ContainsKey(room.ID))
            {
                _rooms.Add(room.ID, room);
                return room;
            }
        }

        return null;
    }

    public DiningRoom? JoinRoom(User user, int roomId)
    {
        if (_rooms.TryGetValue(roomId, out var room))
        {
            room.Join(user);
            return room;
        }
        return null;
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

    public bool Vote(int roomId, string userId, string RestaurantId, int votes)
    {
        if (_rooms.TryGetValue(roomId, out var room))
        {
            if (room.UserExist(userId))
            {
                var restaurant = room.GetRestaurant(RestaurantId);
                var user = room.GetUser(userId);

                Match match = new()
                {
                    User = user,
                    Vote = votes,
                    Restaurant = restaurant,
                };

                if (_matchService.CheckForMatch(room, match))
                {
                    room.WinningRestaurant = restaurant;
                    return true;
                }
            }
        }

        return false;
    }

    public List<string> GetUserIds(int roomId)
    {
        if (_rooms.TryGetValue(roomId, out var room))
            return room.Users.Select(x => x.SignalRConnection).ToList();
        else
            return new();
    }
}