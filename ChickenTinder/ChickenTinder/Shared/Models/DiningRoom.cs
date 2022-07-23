using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChickenTinder.Shared.Models
{
    public class DiningRoom
    {
        public DiningRoom(User host, List<Restaurant> restaurants)
        {
            Host = host;
            Users = new List<User>() { host };
            Restaurants = restaurants;
            Matches = new List<Match>();
        }

        public User Host { get; set; }
        public List<User> Users { get; set; }
        public int ID { get; set; }
        public List<Restaurant> Restaurants { get; set; }
        public List<Match> Matches { get; set; }
        
        public void Join(User user)
        {
            Users.Add(user);
        }
        public void Leave(User user)
        {
            Users.Remove(user);
        }

        public string ToJson()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
