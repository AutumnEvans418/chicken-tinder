using ChickenTinder.Shared.Core;

namespace ChickenTinder.Server.Managers
{
    public class RestaurantManager
    {
        private readonly HttpClient _httpClient;

        public RestaurantManager(IHttpClientFactory clientFactory)
        {
            _httpClient = clientFactory.CreateClient("YelpClient");
        }

        public async Task<List<Business>?> GetRestaurants(string location)
        {
            return (await _httpClient.GetFromJsonAsync<YelpResponce>("search?location=" + location))?.Businesses;
        }
    }
}
