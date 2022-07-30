namespace ChickenTinder.Server.Services
{
    public interface IRestaurantService
    {
        Task<Restaurant?> GetRestaurant(string id);
        Task<List<Restaurant>?> GetRestaurants(string location);
        Task<List<Restaurant>?> GetRestaurants(string latitude, string longitude);
    }
}