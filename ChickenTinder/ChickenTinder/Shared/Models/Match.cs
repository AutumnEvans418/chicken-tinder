using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChickenTinder.Shared.Models
{
    public record Match(User User, Restaurant Restaurant, UserAction Action);
}
