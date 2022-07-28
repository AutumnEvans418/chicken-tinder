namespace ChickenTinder.Client.Data
{
    public interface INavigationManager
    {
        void NavigateTo(string url);
        void NavigateTo(string url, bool force);
        string BaseUri { get; }
    }
    public class GeoCoordinates
    {
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public double Accuracy { get; set; }
    }
}
