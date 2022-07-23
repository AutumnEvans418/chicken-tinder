using ChickenTinder.Shared.Core;
using ChickenTinder.Shared.Models;
using Microsoft.Extensions.Caching.Memory;

namespace ChickenTinder.Server.Managers
{
    public class RestaurantManager
    {
        private readonly HttpClient _httpClient;
        private readonly IMemoryCache _cache;

        public RestaurantManager(IHttpClientFactory clientFactory, IMemoryCache cache)
        {
            _httpClient = clientFactory.CreateClient("YelpClient");
            _cache = cache;
        }

        public async Task<List<Restaurant>?> GetRestaurants(string location)
        {
            var output = _cache.Get<List<Restaurant>>(location);

            if (output is null)
            {
                output = (await _httpClient.GetFromJsonAsync<YelpResponce>("search?location=" + location))?.Businesses;
                _cache.Set(location, output, TimeSpan.FromMinutes(15));
            }

            return output;
        }
    }
}
