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
        private readonly HubConnection _hubConnection;
        private readonly InterloopService _interloopService;
        private readonly ILogger<ServerConnection> logger;
        private User user = new User();

        public ServerConnection(
            NavigationManager NavigationManager,
            LocationService locationService,
            InterloopService interloop,
            ILogger<ServerConnection> logger)
        {
            locationService.OnFound = g =>
            {
                User.Longitude = locationService.GeoCoordinates?.Longitude.ToString();
                User.Latitude = locationService.GeoCoordinates?.Latitude.ToString();
                User.Location = $"{User.Latitude},{User.Longitude}";
                OnLocationChanged?.Invoke();
            };
            _interloopService = interloop;
            this.logger = logger;
            _locationService = locationService;


            _hubConnection = new HubConnectionBuilder()
                                .WithUrl(NavigationManager.ToAbsoluteUri("/tinderhub"))
                                .Build();


            _hubConnection.On("OnStart", () =>
            {
                try
                {
                    OnStart?.Invoke();
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "on start failed");
                    throw;
                }
            });

            _hubConnection.On<User>("OnJoin", (x) =>
            {
                try
                {
                    if (HasRoom && x.Id != User.Id)
                        Room!.Users.Add(x);
                    OnJoin?.Invoke(x);
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "on join failed");
                    throw;
                }
            });

            _hubConnection.On<User>("OnLeave", (x) =>
            {
                try
                {
                    if (HasRoom)
                        Room!.Users = Room.Users.Where(p => p.Id != x.Id).ToList();
                    OnLeave?.Invoke(x);
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "on leave failed");
                    throw;
                }
            });

            _hubConnection.On("OnMatch", () =>
            {
                try
                {
                    Console.WriteLine("   is a match !!!!!!!");
                    OnMatch?.Invoke();
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "on match failed");
                    throw;
                }
            });
        }
        public Action? OnUserChanged { get; set; }
        public Action? OnLocationChanged { get; set; }
        public User User
        {
            get => user; 
            private set
            {
                user = value;
                OnUserChanged?.Invoke();
            }
        }
        public bool IsHost => User.Id == Room?.Host.Id;
        public bool HasRoom => Room != null;
        public DinningRoom? Room { get; private set; } = null;

        public event Action? OnStart;
        public event Action? OnMatch; // RestaurantId of the Match
        public event Action<User>? OnJoin;
        public event Action<User>? OnLeave;
        public bool IsLocationSet => !string.IsNullOrWhiteSpace(User.Location) || !string.IsNullOrWhiteSpace(User.Latitude);
        private async Task Connect()
        {
            try
            {
                if (_hubConnection.State is not HubConnectionState.Disconnected)
                    return;

                await _hubConnection.StartAsync();


                var userId = await _interloopService.GetLocalStorage("UserId");
                if (string.IsNullOrEmpty(userId) && !string.IsNullOrEmpty(_hubConnection.ConnectionId))
                {
                    userId = User.Id;
                    await _interloopService.SetLocalStorage("UserId", userId);
                }

                //_user.SignalRConnection = userId ?? "NA";
                User.Id = userId ?? throw new Exception("userid cannot be null");

                User.SignalRConnection = _hubConnection.ConnectionId ?? throw new Exception("not connected");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "connect failed");
                throw;
            }
        }

        public async Task SetLocation()
        {
            if (User == null)
                return;
            await _locationService.GetLocationAsync();

        }

        public async Task CreateRoom()
        {
            await Connect();
            Room = await _hubConnection.InvokeAsync<DinningRoom>("CreateRoom", User);
            UpdateUser(Room);
        }

        public void UpdateUser(DinningRoom? room)
        {
            if (room != null)
                User = room.Users.First(p => User?.Id == p.Id);
        }

        public async Task JoinRoom(int roomId)
        {
            await Connect();
            Room = await _hubConnection.InvokeAsync<DinningRoom>("JoinRoom", roomId, User);
            UpdateUser(Room);
        }

        public async Task LeaveRoom()
        {
            await Connect();

            if (HasRoom)
            {
                await _hubConnection.InvokeAsync("LeaveRoom", Room!.ID, User);
            }
        }

        public async Task Like(string RestaurantId, UserAction votes)
        {
            await Connect();

            if (HasRoom)
            {
                await _hubConnection.InvokeAsync("Like", Room?.ID, User?.Id, RestaurantId, votes);
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
            await Connect();
            return await _hubConnection.InvokeAsync<Restaurant>("GetRestaurant", id);
        }

        public async Task<DinningRoom?> GetRoom(int id)
        {
            await Connect();
            return await _hubConnection.InvokeAsync<DinningRoom>("GetRoom", id);
        }

        public async Task SetPickyUser(int roomId, string userId)
        {
            await Connect();

            if (Room is not null)
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