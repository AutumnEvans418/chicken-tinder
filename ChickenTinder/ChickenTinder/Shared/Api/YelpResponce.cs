using ChickenTinder.Shared.Models;

namespace ChickenTinder.Shared.Api;

public class YelpResponce
{
    public List<Restaurant> Businesses { get; set; } = new();
    public int Total { get; set; }
    public Region Region { get; set; } = new();
}