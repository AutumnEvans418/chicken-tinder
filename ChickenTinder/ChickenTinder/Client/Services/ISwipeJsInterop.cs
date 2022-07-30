using ChickenTinder.Shared.Models;

namespace ChickenTinder.Client
{
    public interface ISwipeJsInterop : IDisposable
    {
        event EventHandler<SwipeDirection>? OnSwiped;
        Task InitCards();
        Task Start();
        void Swipe(int direction);
    }
}