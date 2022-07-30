namespace ChickenTinder.Client.Data
{
    public interface INavigationManager
    {
        void NavigateTo(string url);
        void NavigateTo(string url, bool force);
        string BaseUri { get; }
        Uri ToAbsoluteUri(string url);
    }
}
