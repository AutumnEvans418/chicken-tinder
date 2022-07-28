using Newtonsoft.Json;

namespace ChickenTinder.Shared.Models;

public class DinningRoom
{
    public DinningRoom(User host, List<Restaurant> restaurants)
    {
        Host = host;
        Users = new() { host };
        Restaurants = restaurants;
        Matches = new List<Match>();
        PickyUsers = new List<User>() { host };
    }
    public RoomStatus Status { get; set; }
    public User Host { get; set; }
    public List<User> Users { get; set; }
    public int ID { get; set; }
    public List<Restaurant> Restaurants { get; set; }
    public List<Match> Matches { get; set; }
    public List<User> PickyUsers { get; set; }
    public Restaurant? WinningRestaurant { get; set; }

    public Restaurant? GetRestaurant(string Id)
    {
        return Restaurants.FirstOrDefault(x => x.ID == Id);
    }

    public User? GetUser(string Id)
    {
        return Users.FirstOrDefault(x=> x.Id == Id);
    }

    public bool UserExist(string id)
    {
        return Users.Any(x=> x.Id == id);
    }

    public void VoidUser(string id)
    {
        var user = GetUser(id);
        if (user is not null)
        {
            Users.Remove(user);
        }
    }

    public void SetPickyUser(string id)
    {
        var user = GetUser(id);
        if (user is not null)
        {
            user.MaxSwipesReached = true;
        }
    }

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