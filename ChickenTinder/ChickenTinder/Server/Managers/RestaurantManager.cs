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
            var cache = _cache.Get<List<Restaurant>>(location);

            if (cache is null)
            {
                cache = (await _httpClient.GetFromJsonAsync<YelpResponce>("search?location=" + location))?.Businesses;
                _cache.Set(location, cache, TimeSpan.FromMinutes(15));
            }

            return cache;
        }
    }
}
