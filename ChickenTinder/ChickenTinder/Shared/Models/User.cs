using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChickenTinder.Shared.Models
{
    public class User
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string SignalRConnection { get; set; } = string.Empty;
        public string? Longitude { get; set; }
        public string? Latitude { get; set; }
        public string? Location { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Class { get; set; }
        public string? Color { get; set; }
        public bool MaxSwipesReached { get; set; }

        public string Style => $"color: #{Color}";
    }
}
