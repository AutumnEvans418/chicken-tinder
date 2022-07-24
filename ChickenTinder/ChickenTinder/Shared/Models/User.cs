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
