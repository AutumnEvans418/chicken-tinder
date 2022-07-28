using ChickenTinder.Shared.Models;

namespace ChickenTinder.Client.Data
{
    public interface IServerConnection
    {
        bool HasRoom { get; }
        bool IsHost { get; }
        bool IsLocationSet { get; }
        Action? OnLocationChanged { get; set; }
        Action? OnUserChanged { get; set; }
        DinningRoom? Room { get; }
        User User { get; }

        event Action<User>? OnJoin;
        event Action<User>? OnLeave;
        event Action? OnMatch;
        event Action? OnStart;

        Task CreateRoom();
        ValueTask DisposeAsync();
        Task<Restaurant?> GetRestaurant(string id);
        Task<DinningRoom?> GetRoom(int id);
        Task JoinRoom(int roomId);
        Task LeaveRoom();
        Task Like(string RestaurantId, UserAction votes);
        Task SetLocation();
        Task SetPickyUser(int roomId, string userId);
        Task Start();
        void UpdateUser(DinningRoom? room);
    }
}