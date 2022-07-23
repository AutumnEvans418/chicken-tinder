using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChickenTinder.Shared.Models
{
    public class User
    {
        public string SignalRConnection { get; set; }
        public string Location { get; set; }
        public string Name { get; set; }
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
