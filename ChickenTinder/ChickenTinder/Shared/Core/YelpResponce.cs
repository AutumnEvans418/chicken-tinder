using System.Collections.Generic; 
namespace ChickenTinder.Shared.Core{ 

    public class YelpResponce
    {
        public List<Business> Businesses { get; set; } = new();
        public int Total { get; set; }
        public Region Region { get; set; } = new();
    }

}