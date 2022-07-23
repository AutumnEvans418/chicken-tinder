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

        public Room? JoinRoom()
        {

        }

        public Room? CreateRoom()
        {

        }

        public void LeaveRoom()
        {

        }
    }
}
