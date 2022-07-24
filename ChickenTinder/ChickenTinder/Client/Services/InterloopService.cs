using Microsoft.JSInterop;

namespace ChickenTinder.Client.Services
{
    public class InterloopService
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
    }
}