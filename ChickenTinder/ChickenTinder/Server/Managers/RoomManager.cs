using ChickenTinder.Shared.Models;

namespace ChickenTinder.Server.Managers
{
    public class RoomManager
    {
        private readonly RestaurantManager _reastaurantManager;

        private readonly Dictionary<int, DiningRoom> _rooms = new();


        public RoomManager(RestaurantManager restaurantManager)
        {
            _reastaurantManager = restaurantManager;
        }

        public async Task<DiningRoom?> CreateRoom(User user)
        {
            var locations = await _reastaurantManager.GetRestaurants(user.Location);
            if (locations is not null)
            {
                DiningRoom room = new(user, locations)
                {
                    ID = new Random().Next(0, 99999)
                };

                if (!_rooms.ContainsKey(room.ID))
                {
                    return room;
                }
            }

            return null;
        }

        public DiningRoom? JoinRoom()
        {
            return null;
        }

        public void LeaveRoom()
        {

        }
    }
}
