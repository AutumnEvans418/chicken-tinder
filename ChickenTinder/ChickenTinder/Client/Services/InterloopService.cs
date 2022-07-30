using ChickenTinder.Shared.Api;
using ChickenTinder.Shared.Models;
using Microsoft.JSInterop;

namespace ChickenTinder.Client.Services
{
    public class InterloopService : IInterloopService
    {
        private readonly IJSRuntime _jsRuntime;

        public InterloopService(IJSRuntime js)
        {
            _jsRuntime = js;
        }

        public async Task ShareUrl(string url)
        {
            await _jsRuntime.InvokeVoidAsync("interloop.shareUrl", url);
        }

        public async Task<string?> GetLocalStorage(string key)
        {
            return await _jsRuntime.InvokeAsync<string?>("interloop.getLocalStorage", key);
        }

        public async Task SetLocalStorage(string key, string value)
        {
            await _jsRuntime.InvokeAsync<string?>("interloop.setLocalStorage", key, value);
        }

        public async Task LaunchMaps(string lat, string lon)
        {
            await _jsRuntime.InvokeAsync<string?>("interloop.LaunchMaps", lat, lon);
        }

        public async Task LaunchMaps(Location location)
        {
            await _jsRuntime.InvokeAsync<string?>("interloop.LaunchMaps", location.Address1);
        }

        public async Task SetUserId(User user)
        {
            var userId = await GetLocalStorage("UserId");
            if (string.IsNullOrEmpty(userId))
            {
                userId = user.Id;
                await SetLocalStorage("UserId", userId);
            }
            user.Id = userId;
        }
    }
}