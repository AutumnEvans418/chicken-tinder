using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChickenTinder.Shared.Models
{
    public class DinningRoom
    {   
        public int Id { get; set; }
        public List<User> Users { get; set; } = new();
        public List<Restaurant> Restaurants { get; set; } = new();
        public List<Match> Matches { get; set; } = new();
    }
}
