namespace ChickenTinder.Shared.Models
{
    public class Restaurant
    {
        public string ID { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public int Review_Count { get; set; }
        public float Rating { get; set; }
        public string Address { get; set; } = string.Empty;
        public float Cost { get; set; }
        public bool Is_Closed { get; set; }
        public string ImageUrl { get; set; } = string.Empty;
        public string Url { get; set; } = string.Empty;
        public List<object> Transactions { get; set; } = new();
        public string Price { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string Display_Phone { get; set; } = string.Empty;
        public double Distance { get; set; }
    }
}
