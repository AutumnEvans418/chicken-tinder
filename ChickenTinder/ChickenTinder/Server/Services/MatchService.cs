namespace ChickenTinder.Server.Services;

public class MatchService
{
    public bool CheckForMatch(DiningRoom room, Match? match = null)
    {
        bool matchFound = false;
        int userMaxSwipeCount = 0;        
        int userMatches = 0;

        if (match?.Restaurant?.ID == null)
        {
            userMaxSwipeCount = room.Users.Where(u => u.MaxSwipesReached).Count();
        } else
        {
            if (!room.Matches.Any(m => m.Restaurant.ID == match.Restaurant.ID))
            {
                room.Matches.Add(match);
            }            
            
            userMatches = room.Matches.Where(m => m.Restaurant?.ID == match.Restaurant.ID && m.Vote <= 3).Count();            
        }

        if (userMaxSwipeCount >= room.Users.Count)
        {            
            //find winning restaurant based on most votes and least distance
            room.WinningRestaurant = room.Matches.OrderByDescending(m => m.Vote).OrderBy(m => m.Restaurant.Distance).FirstOrDefault().Restaurant;
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
