using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChickenTinder.Shared.Models
{
    public class Restaurant
    {
        public string ID { get; set; }
        public string Name { get; set; }
        public int ReviewCount { get; set; }
        public float Rating { get; set; }
        public float Distance { get; set; }
        public string Address { get; set; }
        public float Cost { get; set; }
        public bool IsOpen { get; set; }
        public string ImageUrl { get; set; }
        public string MapUrl { get; set; }
    }
}
