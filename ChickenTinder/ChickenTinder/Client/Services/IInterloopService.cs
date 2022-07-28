using ChickenTinder.Shared.Api;

namespace ChickenTinder.Client.Services
{
    public interface IInterloopService
    {
        Task<string?> GetLocalStorage(string key);
        Task LaunchMaps(Location location);
        Task LaunchMaps(string lat, string lon);
        Task SetLocalStorage(string key, string value);
        Task ShareUrl(string url);
    }
}