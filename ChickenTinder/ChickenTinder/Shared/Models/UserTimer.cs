namespace ChickenTinder.Shared.Models
{
    public class DisposalTimer<T> : IDisposable
    {
        public DisposalTimer(T user)
        {
            Timer.Elapsed += Timer_Elapsed;
            Timer.Stop();
            User = user;
        }
        public T User { get; set; }
        private readonly System.Timers.Timer Timer = new System.Timers.Timer(TimeSpan.FromMinutes(5).TotalMilliseconds);

        private void Timer_Elapsed(object? sender, System.Timers.ElapsedEventArgs e)
        {
            OnExpired?.Invoke(User);
        }
        public void ResetTimeout()
        {
            Timer.Stop();
            Timer.Start();
        }
        public Action<T>? OnExpired { get; set; }

        public void Dispose()
        {
            Timer.Dispose();
        }
    }
}
