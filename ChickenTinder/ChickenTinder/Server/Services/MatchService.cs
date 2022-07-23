using ChickenTinder.Shared.Models;

namespace ChickenTinder.Server.Services
{
    public class MatchService
    {
        public bool CheckForMatch(DiningRoom room, Match match)
        {
            if (match.Restaurant?.ID == null) throw new ArgumentException("Restaurant not valid");

            bool matchFound = false;
            if (room.Matches.Count == 0 || !room.Matches.Any(m => m.Restaurant?.ID == match.Restaurant.ID))
            {
                room.Matches.Add(match);
                matchFound = false;
            }
            else
            {
                if (room.Matches.Where(m => m.Restaurant?.ID == match.Restaurant.ID).Count() == room.Users.Count)
                {
                    matchFound = true;
                }
            }
            return matchFound;
        }
    }
}
