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
            Users = new() { { host.SignalRConnection, host } };
            Restaurants = restaurants;
            Matches = new List<Match>();
        }

        public User Host { get; set; }
        public Dictionary<string, User> Users { get; set; }
        public int ID { get; set; }
        public List<Restaurant> Restaurants { get; set; }
        public List<Match> Matches { get; set; }

        public Restaurant? GetRestaurant(string Id)
        {
            return Restaurants.FirstOrDefault(x => x.ID == Id);
        }
        public User? GetUser(string Id)
        {
            if (Users.ContainsKey(Id))
                return Users[Id];
            else return null;
        }
        public bool UserExist(string id)
        {
            return Users.ContainsKey(id);
        }

        public void Join(User user)
        {
            Users.Add(user.SignalRConnection, user);
        }
        public void Leave(User user)
        {
            Users.Remove(user.SignalRConnection);
        }

        public string ToJson()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
