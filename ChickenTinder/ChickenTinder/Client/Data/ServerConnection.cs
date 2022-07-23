using ChickenTinder.Shared.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;

namespace ChickenTinder.Client.Data
{
    public class ServerConnection
    {
        private DiningRoom? _room = null;
        private User? _user;

        private readonly HubConnection _hubConnection;

        public ServerConnection(NavigationManager NavigationManager)
        {
            _hubConnection = new HubConnectionBuilder()
                                .WithUrl(NavigationManager.ToAbsoluteUri("/tenderhub"))
                                .Build();

            _ = Connect();
           // _ = Connect().ContinueWith(x => CreateRoom());
        }

        public bool HasRoom => _room is not null;

        public event Action? OnStart;
        public event Action<string>? OnMatch; // RestaurantId

        /// <summary>
        /// Connect the SignalR Service and create the User
        /// </summary>
        /// <param name="location"></param>
        /// <returns></returns>
        public async Task Connect(string location = "Kansas City")
        {
            await _hubConnection.StartAsync();

            _user = new()
            {
                Name = "Tommy",
                Location = location,
                SignalRConnection = _hubConnection.ConnectionId ?? "NA"
            };
        }

        public async Task CreateRoom()
        {
            if (_hubConnection is not null)
            {
                _room = await _hubConnection.InvokeAsync<DiningRoom>("CreateRoom", _user);
            }

            Console.WriteLine("Room Created");

            Console.WriteLine(_room.ToJson());
        }

        public async Task JoinRoom(int roomId)
        {

        }

        public async Task Like(string RestaurantId, int votes)
        {

        }
    }
}
