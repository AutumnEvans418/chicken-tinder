using ChickenTinder.Server.Managers;
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

        public DiningRoom? JoinRoom(User user, int roomId)
        {
            return _roomManager.JoinRoom(user, roomId);
        }

        public async Task UpdateRestaurant(Match match)
        {

        }

        public async Task Start(int roomId)
        {

        }

        public async Task Like(int roomId, string RestaurantId, int votes)
        {
            if (_roomManager.Vote(roomId, Context.ConnectionId, RestaurantId, votes))
            {
                await InvokeMatch(RestaurantId);
            }
        }

        private async Task InvokeMatch(string RestaurantId)
        {
          //  Clients.
        }
    }
}
