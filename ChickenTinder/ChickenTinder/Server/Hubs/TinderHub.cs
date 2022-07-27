namespace ChickenTinder.Server.Hubs;

public class TinderHub : Hub
{
    private readonly RoomService _roomManager;
    private readonly RestaurantService _restaurantService;

    public TinderHub(RoomService roomManager, RestaurantService restaurantService)
    {
        _restaurantService = restaurantService;
        _roomManager = roomManager;
    }

    public async Task<DiningRoom?> CreateRoom(User user)
    {
        return await _roomManager.CreateRoom(user);
    }

    public async Task<DiningRoom?> JoinRoom(int roomId, User user)
    {
        var room = await _roomManager.JoinRoom(user, roomId);
        await InvokeJoin(roomId, user);
        return room;
    }

    public async Task StartRoom(int roomId)
    {
        var ids = _roomManager.Start(roomId);
        await Clients.Clients(ids).SendAsync("OnStart");
    }

    public async Task Like(int roomId, string userId, string RestaurantId, UserAction votes)
    {
        if (_roomManager.Vote(roomId, userId, RestaurantId, votes))
        {
            await InvokeMatch(roomId, RestaurantId);
        }
    }

    public void SetPickyUser(int roomId, string userId)
    {
        _roomManager.SetPickyUser(roomId, userId);
    }

    public async Task LeaveRoom(User user, int roomId)
    {
        _roomManager.LeaveRoom(user, roomId);
        await InvokeLeave(roomId, user);
    }

    public async Task<Restaurant?> GetRestaurant(string id)
    {
        return await _restaurantService.GetRestaurant(id);
    }

    public DiningRoom? GetRoom(int id)
    {
        return  _roomManager.GetRoom(id);
    }

    public void SetUserVoid(int roomId, string userId)
    {
         _roomManager.SetUserVoid(roomId, userId);
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
