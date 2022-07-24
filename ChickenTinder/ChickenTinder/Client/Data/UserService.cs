using ChickenTinder.Shared.Models;
using System.Net.Http.Json;

namespace ChickenTinder.Client.Data
{
    public class UserService
    {
        private readonly HttpClient client;
        private Random random = new Random();
        public UserService(HttpClient client)
        {
            this.client = client;
        }
        public async Task<User> GetRandomUser()
        {
            var users = await client.GetFromJsonAsync<List<User>>("users.json");
            if (users == null)
                throw new Exception("users file not found");
            var user = users[random.Next(0, users.Count - 1)];
            var colors = await client.GetFromJsonAsync<List<string>>("colors.json");
            if (colors == null)
                throw new Exception("colors file not found");
            user.Color = colors[random.Next(0, colors.Count - 1)];
            return user;
        }
    }
}
