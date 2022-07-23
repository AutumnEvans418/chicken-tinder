using ChickenTinder.Shared.Models;
using System.Collections.Generic; 
namespace ChickenTinder.Shared.Core{ 

    public class YelpResponce
    {
        public List<Restaurant> Businesses { get; set; } = new();
        public int Total { get; set; }
        public Region Region { get; set; } = new();
    }

}