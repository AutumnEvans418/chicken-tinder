namespace ChickenTinder.Shared.Core
{

    public class Business
    {
        public string Id { get; set; } = string.Empty;
        public string Alias { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string ImageUrl { get; set; } = string.Empty;
        public bool IsClosed { get; set; }
        public string Url { get; set; } = string.Empty;
        public int ReviewCount { get; set; }
        public List<Category> Categories { get; set; } = new();
        public double Rating { get; set; }
        public Coordinates Coordinates { get; set; } = new();
        public List<object> Transactions { get; set; } = new();
        public string Price { get; set; } = string.Empty;
        public Location Location { get; set; } = new();
        public string Phone { get; set; } = string.Empty;
        public string DisplayPhone { get; set; } = string.Empty;
        public double Distance { get; set; }
    }

}