namespace ChickenTinder.Server.Services;

public class MatchService
{
    public bool CheckForMatch(DiningRoom room, Match? match)
    {
        if (match?.Restaurant?.ID == null) throw new ArgumentNullException(nameof(match));

        bool matchFound = false;
        int userMaxSwipeCount = 0;
        int userMatches = 0;

        userMaxSwipeCount = room.Users.Where(u => u.MaxSwipesReached).Count();
        
        if (!room.Matches.Any(m => m.Restaurant.ID == match.Restaurant.ID && m.User.Name == match.User.Name))
        {
            room.Matches.Add(match);
        }            
            
        userMatches = room.Matches
            .Where(m => m.Restaurant?.ID == match.Restaurant.ID && m.Vote > 1)
            .Select(m => m.Restaurant.ID)
            .Distinct().Count();

        if (userMaxSwipeCount >= room.Users.Count)
        {            
            //find winning restaurant based on most votes and least distance
            room.WinningRestaurant = room.Matches.OrderByDescending(m => m.Vote).ThenBy(m => m.Restaurant.Distance).FirstOrDefault().Restaurant;
            matchFound = true;
        }
        else if (userMaxSwipeCount + userMatches >= room.Users.Count)
        {
            room.WinningRestaurant = match.Restaurant;
            matchFound = true;
        }

        return matchFound;
    }
}
