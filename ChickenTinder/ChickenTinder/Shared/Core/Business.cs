namespace ChickenTinder.Shared.Core
{

    public class Business
    {
        public string Id { get; set; } = string.Empty;
        public string Alias { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string ImageUrl { get; set; } = string.Empty;
        public bool Is_Closed { get; set; }
        public string Url { get; set; } = string.Empty;
        public int Review_Count { get; set; }
        public List<Category> Categories { get; set; } = new();
        public double Rating { get; set; }
        public Coordinates Coordinates { get; set; } = new();
        public List<object> Transactions { get; set; } = new();
        public string Price { get; set; } = string.Empty;
        public Location Location { get; set; } = new();
        public string Phone { get; set; } = string.Empty;
        public string Display_Phone { get; set; } = string.Empty;
        public double Distance { get; set; }
    }

}