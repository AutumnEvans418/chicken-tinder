using Microsoft.Extensions.Caching.Memory;

namespace ChickenTinder.Server.Services;

public class RestaurantService : IRestaurantService
{
    private readonly HttpClient _httpClient;
    private readonly IMemoryCache _cache;

    public RestaurantService(IHttpClientFactory clientFactory, IMemoryCache cache)
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


    public async Task<List<Restaurant>?> GetRestaurants(string latitude, string longitude)
    {
        var output = _cache.Get<List<Restaurant>>(latitude + longitude);

        if (output is null)
        {
            output = (await _httpClient.GetFromJsonAsync<YelpResponce>("search?latitude=" + latitude + "&longitude=" + longitude))?.Businesses;
            _cache.Set(latitude + longitude, output, TimeSpan.FromMinutes(15));
        }

        return output;
    }

    public async Task<Restaurant?> GetRestaurant(string id)
    {
        var output = _cache.Get<Restaurant>(id);

        if (output is null)
        {
            output = await _httpClient.GetFromJsonAsync<Restaurant>(id);
            _cache.Set(id, output, TimeSpan.FromMinutes(15));
        }

        return output;
    }
}