using ChickenTinder.Shared.Core;
using ChickenTinder.Shared.Models;

namespace ChickenTinder.Server.Managers
{
    public class RestaurantManager
    {
        private readonly HttpClient _httpClient;

        public RestaurantManager(IHttpClientFactory clientFactory)
        {
            _httpClient = clientFactory.CreateClient("YelpClient");
        }

        public async Task<List<Restaurant>?> GetRestaurants(string location)
        {
            return (await _httpClient.GetFromJsonAsync<YelpResponce>("search?location=" + location))?.Businesses;
        }
    }
}
