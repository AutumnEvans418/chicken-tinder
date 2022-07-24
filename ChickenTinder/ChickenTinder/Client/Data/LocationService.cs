using Microsoft.JSInterop;

namespace ChickenTinder.Client.Data
{
    public class LocationService
    {
        private readonly Lazy<Task<IJSObjectReference>> moduleTask = default!;
        private readonly DotNetObjectReference<LocationService> dotNetObjectReference;

        private readonly IJSRuntime _jsRuntime;

        public LocationService(IJSRuntime js)
        {
            _jsRuntime = js;

            moduleTask = new(() => _jsRuntime!.InvokeAsync<IJSObjectReference>(
                identifier: "import",
                args: "./geoLocationJsInterop.js")
            .AsTask());

            dotNetObjectReference = DotNetObjectReference.Create(this);
        }

        public GeoCoordinates? GeoCoordinates { get; private set; }

        public async Task GetLocationAsync()
        {
            var module = await moduleTask.Value;
            await module.InvokeVoidAsync(identifier: "getCurrentPosition", dotNetObjectReference);
        }

        [JSInvokable]
        public void OnSuccessAsync(GeoCoordinates geoCoordinates) => this.GeoCoordinates = geoCoordinates;
        

        public async ValueTask DisposeAsync()
        {
            if (moduleTask.IsValueCreated)
            {
                var module = await moduleTask.Value;
                await module.DisposeAsync();
            }
        }
    }
}