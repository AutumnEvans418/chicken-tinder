using ChickenTinder.Client.Services;
using ChickenTinder.Shared.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.JSInterop;

namespace ChickenTinder.Client.Data
{
    public class ServerConnection : IAsyncDisposable
    {
        private readonly LocationService _locationService;
        private User? _user;

        private readonly HubConnection _hubConnection;
        private readonly InterloopService _interloopService;

        public ServerConnection(NavigationManager NavigationManager, LocationService locationService, InterloopService interloop)
        {
            _interloopService = interloop;

            _locationService = locationService;

            _ = _locationService.GetLocationAsync();

            _hubConnection = new HubConnectionBuilder()
                                .WithUrl(NavigationManager.ToAbsoluteUri("/tinderhub"))
                                .Build();


            _hubConnection.On("OnStart", () =>
            {
                OnStart?.Invoke();
            });

            _hubConnection.On<User>("OnJoin", (x) =>
            {
                if (HasRoom && x.Id != User.Id)
                    Room!.Users.Add(x);
                OnJoin?.Invoke(x);
            });

            _hubConnection.On<User>("OnLeave", (x) =>
            {
                if (HasRoom)
                    Room!.Users = Room.Users.Where(p => p.Id != x.Id).ToList();
                OnLeave?.Invoke(x);
            });

            _hubConnection.On<string>("OnMatch", (x) =>
            {
                Console.WriteLine(x + "   is a match !!!!!!!");
                OnMatch?.Invoke(x);
            });
        }
        public User? User => _user;
        public bool IsHost => _user?.Id == Room?.Host.Id;
        public bool HasRoom => Room != null;
        public DiningRoom? Room { get; private set; } = null;

        public event Action? OnStart;
        public event Action<string>? OnMatch; // RestaurantId of the Match
        public event Action<User>? OnJoin;
        public event Action<User>? OnLeave;


        private async Task Connect()
        {
            if (_hubConnection.State is not HubConnectionState.Disconnected)
                return;

            await _hubConnection.StartAsync();

            if (_user is null)
            {
                _user = new();

                var userId = await _interloopService.GetLocalStorage("UserId");
                if (string.IsNullOrEmpty(userId) && !string.IsNullOrEmpty(_hubConnection.ConnectionId))
                {
                    userId = _user.Id;
                    await _interloopService.SetLocalStorage("UserId", userId);
                }

                //_user.SignalRConnection = userId ?? "NA";
                _user.Id = userId ?? throw new Exception("userid cannot be null");
                _user.Longitude = _locationService.GeoCoordinates?.Longitude.ToString() ?? string.Empty;
                _user.Latitude = _locationService.GeoCoordinates?.Latitude.ToString() ?? string.Empty;
                _user.SignalRConnection = _hubConnection.ConnectionId ?? throw new Exception("not connected");
            }
        }

        public async Task CreateRoom(string location = "Kansas City")
        {
            await Connect();

            _user!.Location = location;

            Room = await _hubConnection.InvokeAsync<DiningRoom>("CreateRoom", _user);
            UpdateUser(Room);
        }

        public void UpdateUser(DiningRoom? room)
        {
            if (room != null)
                _user = room.Users.First(p => User?.Id == p.Id);
        }

        public async Task JoinRoom(int roomId)
        {
            await Connect();
            Room = await _hubConnection.InvokeAsync<DiningRoom>("JoinRoom", roomId, _user);
            UpdateUser(Room);
        }

        public async Task LeaveRoom()
        {
            await Connect();

            if (HasRoom)
            {
                await _hubConnection.InvokeAsync("LeaveRoom", Room!.ID, _user);
            }
        }

        public async Task Like(string RestaurantId, UserAction votes)
        {
            await Connect();

            if (HasRoom)
            {
                await _hubConnection.InvokeAsync("Like", Room?.ID, RestaurantId, votes);
            }
        }

        public async Task Start()
        {
            await Connect();

            if (HasRoom)
            {
                await _hubConnection.InvokeAsync("StartRoom", Room!.ID);
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

        public async Task SetPickyUser(int roomId, string userId)
        {
            await Connect();

            if (_hubConnection is not null && Room is not null)
            {
                await _hubConnection.InvokeAsync("SetPickyUser", roomId, userId);
            }
        }

        public async ValueTask DisposeAsync()
        {
            await LeaveRoom();
            await _hubConnection.DisposeAsync();
        }
    }
}