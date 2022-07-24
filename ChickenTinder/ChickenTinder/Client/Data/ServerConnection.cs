using ChickenTinder.Shared.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.JSInterop;

namespace ChickenTinder.Client.Data
{
    public class ServerConnection : IAsyncDisposable
    {
        private readonly LocationService _locationService;
        private DiningRoom? _room = null;
        private User? _user;

        private readonly HubConnection _hubConnection;

        public ServerConnection(NavigationManager NavigationManager, LocationService locationService)
        {
            _locationService = locationService;

            _locationService.GetLocationAsync();

            _hubConnection = new HubConnectionBuilder()
                                .WithUrl(NavigationManager.ToAbsoluteUri("/tinderhub"))
                                .Build();


            _hubConnection.On("OnStart", () =>
            {
                OnStart?.Invoke();
            });

            _hubConnection.On<User>("OnJoin", (x) =>
            {
                if (HasRoom)
                    Room.Users.Add(x);
                OnJoin?.Invoke(x);
            });

            _hubConnection.On<User>("OnLeave", (x) =>
            {
                if (HasRoom)
                    Room.Users = Room.Users.Where(p => p.SignalRConnection != x.SignalRConnection).ToList();
                OnLeave?.Invoke(x);
            });

            _hubConnection.On<string>("OnMatch", (x) =>
            {
                Console.WriteLine(x + "   is a match !!!!!!!");
                OnMatch?.Invoke(x);
            });
        }
        public bool IsHost => this._hubConnection.ConnectionId == _room.Host.SignalRConnection;
        public bool HasRoom => _room is not null;
        public DiningRoom? Room => _room;

        public event Action? OnStart;
        public event Action<string>? OnMatch; // RestaurantId of the Match
        public event Action<User>? OnJoin;
        public event Action<User>? OnLeave;


        /// <summary>
        /// Connect the SignalR Service and create the User
        /// </summary>
        /// <param name="location">The location of the User. Zip or City name</param>
        /// <returns></returns>
        private async Task Connect()
        {
            if (_hubConnection.State is not HubConnectionState.Disconnected)
                return;

            await _hubConnection.StartAsync();

            if (_user is null)
                _user = new()
                {
                    Longitude = _locationService.GeoCoordinates?.Longitude.ToString() ?? string.Empty,
                    Latitude = _locationService.GeoCoordinates?.Latitude.ToString() ?? string.Empty,
                    SignalRConnection = _hubConnection.ConnectionId ?? "NA"
                };
        }

        public void SetName(string name)
        {
            if (_user is not null)
                _user.Name = name;
        }

        public async Task CreateRoom(string location = "Kansas City")
        {
            await Connect();

            _user!.Location = location;

            if (_hubConnection is not null)
            {
                _room = await _hubConnection.InvokeAsync<DiningRoom>("CreateRoom", _user);
            }

            // Temp write it to dev tools for debugging
            //Console.WriteLine(_room!.ToJson());
        }

        public async Task JoinRoom(int roomId)
        {
            await Connect();
            if (_hubConnection is not null)
            {
                _room = await _hubConnection.InvokeAsync<DiningRoom>("JoinRoom", roomId, _user);
            }
        }

        public async Task LeaveRoom()
        {
            await Connect();

            if (_hubConnection is not null && HasRoom)
            {
                await _hubConnection.InvokeAsync("LeaveRoom", _room!.ID, _user);
            }
        }

        public async Task Like(string RestaurantId, int votes)
        {
            await Connect();

            if (_hubConnection is not null && _room is not null)
            {
                await _hubConnection.InvokeAsync("Like", _room?.ID, RestaurantId, votes);
            }
        }

        public async Task Start()
        {
            await Connect();

            if (_hubConnection is not null && HasRoom)
            {
                await _hubConnection.InvokeAsync("StartRoom", _room!.ID);
            }
        }

        public async Task<Restaurant?> GetRestaurant(string id)
        {
            return await _hubConnection.InvokeAsync<Restaurant>("GetRestaurant", id);
        }

        public async Task<DiningRoom?> GetRoom(int id)
        {
            return await _hubConnection.InvokeAsync<DiningRoom>("GetRoom", id);

        }


        public async ValueTask DisposeAsync()
        {
            await LeaveRoom();
            await _hubConnection.DisposeAsync();
        }
    }
}
