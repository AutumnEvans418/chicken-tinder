using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChickenTinder.Shared.Models
{
    public class User
    {
        public string SignalRConnection { get; set; } = string.Empty;
        public string Longitude { get; set; } = string.Empty;
        public string Latitude { get; set; } = string.Empty;
        public string Location { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string? Class { get; set; }
        public string? Color { get; set; }
        public bool MaxSwipesReached { get; set; }

        public string Style => $"color: #{Color}";
    }

    public enum UserAction
    {
        Like,
        Dislike,
        No,
        Yes
    }

    public enum SwipeDirection
    {
        Left = 2,
        Up = 8,
        Right = 4,
        Down = 16,
    }
}
