using ChickenTinder.Server.Managers;
using ChickenTinder.Shared.Core;
using ChickenTinder.Shared.Models;

namespace ChickenTinder.Server.Hubs
{
    public class TinderHub : Hub
    {
        private readonly RoomManager _roomManager;


        public TinderHub(RoomManager roomManager)
        {
            _roomManager = roomManager;
        }

        public async Task<DiningRoom?> CreateRoom(User user)
        {
            return await _roomManager.CreateRoom(user);
        }

        public async Task JoinRoom(int roomId)
        {

        }

        public async Task UpdateRestaurant(Match match)
        {

        }

        public async Task Start(int roomId)
        {

        }
    }
}
