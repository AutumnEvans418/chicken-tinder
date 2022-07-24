using ChickenTinder.Shared.Api;

namespace ChickenTinder.Shared.Models;

public class Restaurant
{
    public string ID { get; set; } = string.Empty;
    public string Alias { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public int Review_Count { get; set; }
    public float Rating { get; set; }
    public float Cost { get; set; }
    public bool Is_Closed { get; set; }
    public string Image_Url { get; set; } = string.Empty;
    public string Url { get; set; } = string.Empty;
    public string Price { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string Display_Phone { get; set; } = string.Empty;
    public double Distance { get; set; }
    public Location Location { get; set; } = new();
    public Coordinates Coordinates { get; set; } = new();
        
    public List<string>? Photos { get; set; } 
    public List<Hour>? Hours { get; set; }
}