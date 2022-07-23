using ChickenTinder.Shared.Models;

namespace ChickenTinder.Server.Services;

public class MatchService
{
    public bool CheckForMatch(DiningRoom room, Match match)
    {
        if (match.Restaurant?.ID == null) throw new ArgumentException("Restaurant not valid");

        room.Matches.Add(match);

        bool matchFound = false;

        if (room.Matches.Where(m => m.Restaurant?.ID == match.Restaurant.ID).Count() == room.Users.Count)
        {
            matchFound = true;
            room.WinningRestaurant = match.Restaurant;
        }
        return matchFound;
    }
}
