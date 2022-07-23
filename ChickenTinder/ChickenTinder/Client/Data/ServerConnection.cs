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
        }

        public bool HasRoom => Room is not null;

        public DiningRoom? Room { get => _room; set => _room = value; }

        public event Action? OnStart;
        public event Action<string>? OnMatch; // RestaurantId

        /// <summary>
        /// Connect the SignalR Service and create the User
        /// </summary>
        /// <param name="location"></param>
        /// <returns></returns>
        private async Task Connect(string location = "Kansas City")
        {
            if (_hubConnection.State != HubConnectionState.Disconnected)
                return;
            await _hubConnection.StartAsync();
            if (_user == null)
                _user = new()
                {
                    Name = "Tommy",
                    Location = location,
                    SignalRConnection = _hubConnection.ConnectionId ?? "NA"
                };
        }

        public async Task CreateRoom()
        {
            await Connect();
            if (_hubConnection is not null)
            {
                Room = await _hubConnection.InvokeAsync<DiningRoom>("CreateRoom", _user);
            }

            Console.WriteLine("Room Created");

            Console.WriteLine(Room.ToJson());
        }

        public async Task JoinRoom(int roomId)
        {

        }

        public async Task Like(string RestaurantId, int votes)
        {

        }
    }
}
