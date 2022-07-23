using ChickenTinder.Server.Services;
using ChickenTinder.Shared.Models;

namespace ChickenTinder.Server.Managers
{
    public class RoomManager
    {
        private readonly RestaurantManager _reastaurantManager;
        private readonly MatchService _matchService;

        private readonly Dictionary<int, DiningRoom> _rooms = new();


        public RoomManager(RestaurantManager restaurantManager)
        {
            _reastaurantManager = restaurantManager;
        }

        public async Task<DiningRoom?> CreateRoom(User user)
        {
            if (user is null) return null;

            var locations = await _reastaurantManager.GetRestaurants(user.Location);
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
                        return true;
                    }
                }
            }

            return false;
        }
    }
}
