using ChickenTinder.Shared.Models;
using Microsoft.JSInterop;

namespace ChickenTinder.Client
{
    // This class provides an example of how JavaScript functionality can be wrapped
    // in a .NET class for easy consumption. The associated JavaScript module is
    // loaded on demand when first needed.
    //
    // This class can be registered as scoped DI service and then injected into Blazor
    // components for use.

    public class SwipeJsInterop : IAsyncDisposable
    {

        DotNetObjectReference<SwipeJsInterop> dotNetRef;
        private readonly IJSRuntime jsRuntime;

        public SwipeJsInterop(IJSRuntime jsRuntime)
        {
            dotNetRef = DotNetObjectReference.Create(this);
            this.jsRuntime = jsRuntime;
        }

        public async Task Start()
        {
            await jsRuntime.InvokeVoidAsync("Swipe.start", dotNetRef);
        }
        public event EventHandler<SwipeDirection> OnSwiped;
        [JSInvokable]
        public void Swipe(int direction)
        {
            OnSwiped?.Invoke(this, (SwipeDirection)direction);
        }

        public async ValueTask DisposeAsync()
        {
            dotNetRef.Dispose();
        }
    }
}