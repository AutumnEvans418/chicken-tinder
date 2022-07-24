using ChickenTinder.Shared.Models;
using System.Net.Http.Json;

namespace ChickenTinder.Client.Data
{
    public class UserService
    {
        private readonly IWebHostEnvironment environment;
        private Random random = new Random();
        public UserService(IWebHostEnvironment environment)
        {
            this.environment = environment;
        }
        public async Task<User> GetRandomUser(List<string> alreadyUsedNames)
        {
            var path = environment.ContentRootPath;
            var usersPath = Path.Combine(path, "users.json");
            var colorsPath = Path.Combine(path, "colors.json");

            var options = new System.Text.Json.JsonSerializerOptions()
            {
                PropertyNamingPolicy = System.Text.Json.JsonNamingPolicy.CamelCase,
            };

            var users = System.Text.Json.JsonSerializer.Deserialize<List<User>>(await File.ReadAllTextAsync(usersPath), options);
            if (users == null)
                throw new Exception("users file not found");
            var user = users.Where(p => alreadyUsedNames.Contains(p.Name) != true).ToList()[random.Next(0, users.Count - 1)];
            var colors = System.Text.Json.JsonSerializer.Deserialize<List<string>>(await File.ReadAllTextAsync(colorsPath), options);
            if (colors == null)
                throw new Exception("colors file not found");
            user.Color = colors[random.Next(0, colors.Count - 1)];
            return user;
        }
    }
}
