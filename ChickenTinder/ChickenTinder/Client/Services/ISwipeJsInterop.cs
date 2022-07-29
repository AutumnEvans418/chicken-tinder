using ChickenTinder.Shared.Models;

namespace ChickenTinder.Client
{
    public interface ISwipeJsInterop
    {
        event EventHandler<SwipeDirection>? OnSwiped;

        void Dispose();
        Task InitCards();
        Task Start();
        void Swipe(int direction);
    }
}