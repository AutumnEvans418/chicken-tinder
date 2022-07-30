namespace ChickenTinder.Client.Data
{
    public interface ILocationService : IAsyncDisposable
    {
        GeoCoordinates? GeoCoordinates { get; }
        Action<GeoCoordinates>? OnFound { get; set; }
        Action? OnError { get; set; }
        Task GetLocationAsync();
        void OnSuccessAsync(GeoCoordinates geoCoordinates);
        bool IsRetrievingLocation { get; }
    }
}