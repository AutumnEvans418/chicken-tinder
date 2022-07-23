using ChickenTinder.Shared.Models;

namespace ChickenTinder.Server.Managers
{
    public class RoomManager
    {
        private readonly RestaurantManager _reastaurantManager;

        private readonly Dictionary<int, DinningRoom> _rooms = new();


        public RoomManager(RestaurantManager restaurantManager)
        {
            _reastaurantManager = restaurantManager;
        }

        public DinningRoom? CreateRoom(User user)
        {
            var locations = _reastaurantManager.GetRestaurants(user.Location);
            DinningRoom room = new(new Random().Next(0,99999), locations);


        }

        public DinningRoom? JoinRoom()
        {
            return null;
        }

        public void LeaveRoom()
        {

        }
    }
}
