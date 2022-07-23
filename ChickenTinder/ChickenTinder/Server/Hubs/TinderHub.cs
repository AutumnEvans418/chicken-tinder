namespace ChickenTinder.Server.Hubs;

public class TinderHub : Hub
{
    private readonly RoomService _roomManager;

    public TinderHub(RoomService roomManager)
    {
        _roomManager = roomManager;
    }


    public async Task<DiningRoom?> CreateRoom(User user)
    {
        return await _roomManager.CreateRoom(user);
    }

    public async Task<DiningRoom?> JoinRoom(int roomId, User user)
    {
        await InvokeJoin(roomId, user);
        return _roomManager.JoinRoom(user, roomId);
    }

    public async Task StartRoom(int roomId)
    {
        await Clients.Clients(_roomManager.GetUserIds(roomId)).SendAsync("OnStart");
    }

    public async Task Like(int roomId, string RestaurantId, int votes)
    {
        if (_roomManager.Vote(roomId, Context.ConnectionId, RestaurantId, votes))
        {
            await InvokeMatch(roomId, RestaurantId);
        }
    }

    public async Task LeaveRoom(User user, int roomId)
    {
        _roomManager.LeaveRoom(user, roomId);
        await InvokeLeave(roomId, user);
    }

    private async Task InvokeMatch(int roomId, string RestaurantId)
    {
        await Clients.Clients(_roomManager.GetUserIds(roomId)).SendAsync("OnMatch", RestaurantId);
    }

    private async Task InvokeJoin(int roomId, User userId)
    {
        await Clients.Clients(_roomManager.GetUserIds(roomId)).SendAsync("OnJoin", userId);
    }

    private async Task InvokeLeave(int roomId, User userId)
    {
        await Clients.Clients(_roomManager.GetUserIds(roomId)).SendAsync("OnLeave", userId);
    }
}
