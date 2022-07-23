using ChickenTinder.Shared.Models;
using Microsoft.AspNetCore.SignalR.Client;

namespace ChickenTinder.Client.Data
{
    public class ServerConnection
    {
        private DiningRoom? _room = null;
        private readonly HubConnection _hubConnection;

        public ServerConnection(HubConnection hub)
        {
            _hubConnection = hub;
        }

        public event Action? OnStart;
        public event Action<string> OnMatch; // RestaurantId


        public async Task CreateRoom()
        {

        }

        public async Task JoinRoom(int roomId)
        {

        }

        public async Task Like(string RestaurantId)
        {

        }

        public async Task (string RestaurantId)
        {

        }
    }
}
