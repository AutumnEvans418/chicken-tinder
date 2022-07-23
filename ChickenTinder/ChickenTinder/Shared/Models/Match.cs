using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChickenTinder.Shared.Models
{
    public class Match
    {
        public User User { get; set; }
        public UserAction Action { get; set; }
        public Restaurant Restaurant { get; set; }
        public int Vote { get; set; }
    }
}
