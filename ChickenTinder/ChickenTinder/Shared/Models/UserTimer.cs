namespace ChickenTinder.Shared.Models
{
    public class UserTimer : IDisposable
    {
        public UserTimer(User user)
        {
            Timer.Elapsed += Timer_Elapsed;
            Timer.Stop();
            User = user;
        }
        public User User { get; set; }
        private System.Timers.Timer Timer = new System.Timers.Timer(30000);

        private void Timer_Elapsed(object? sender, System.Timers.ElapsedEventArgs e)
        {
            OnExpired?.Invoke(User);
        }
        public void ResetTimeout()
        {
            Timer.Stop();
            Timer.Start();
        }
        public Action<User>? OnExpired { get; set; }

        public void Dispose()
        {
            Timer.Dispose();
        }
    }
}
