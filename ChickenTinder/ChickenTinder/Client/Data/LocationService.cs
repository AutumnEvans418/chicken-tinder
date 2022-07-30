using Microsoft.JSInterop;

namespace ChickenTinder.Client.Data
{
    public class LocationService : ILocationService
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
            IsRetrievingLocation = true;
            var module = await moduleTask.Value;
            await module.InvokeVoidAsync(identifier: "getCurrentPosition", dotNetObjectReference);
        }
        public bool IsRetrievingLocation { get; set; }
        public Action<GeoCoordinates>? OnFound { get; set; }
        public Action? OnError { get; set; }
        [JSInvokable]
        public void OnFailureAsync()
        {
            IsRetrievingLocation = false;
            OnError?.Invoke();
        }

        [JSInvokable]
        public void OnSuccessAsync(GeoCoordinates geoCoordinates)
        {
            IsRetrievingLocation = false;
            this.GeoCoordinates = geoCoordinates;
            OnFound?.Invoke(geoCoordinates);
        }

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