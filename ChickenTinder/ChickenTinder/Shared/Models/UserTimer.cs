namespace ChickenTinder.Shared.Models
{
    public class DisposalTimer<T> : IDisposable
    {
        public DisposalTimer(T value, TimeSpan time)
        {
            Timer = new System.Timers.Timer(time.TotalMilliseconds);
            Timer.Elapsed += Timer_Elapsed;
            Timer.Stop();
            Value = value;

        }
        public T Value { get; set; }
        private readonly System.Timers.Timer Timer;

        private void Timer_Elapsed(object? sender, System.Timers.ElapsedEventArgs e)
        {
            OnExpired?.Invoke(Value);
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
