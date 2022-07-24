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
    }
}