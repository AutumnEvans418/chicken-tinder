namespace ChickenTinder.Server.Services;

public class MatchService
{
    public void AddMatch(DinningRoom room, Match match)
    {
        if (match?.Restaurant?.ID == null) throw new ArgumentNullException(nameof(match));

        if (!room.Matches.Where(m => m.Restaurant.ID == match.Restaurant.ID && m.User.Id == match.User.Id).Any())
        {
            room.Matches.Add(match);
        }
    }

    public bool CheckForMatch(DinningRoom room)
    {
        int userMaxSwipeCount = 0;
        userMaxSwipeCount = room.Users.Where(u => u.MaxSwipesReached).Count();


        if (userMaxSwipeCount >= room.Users.Count)
        {
            room.Status = RoomStatus.Matched;
            //find winning restaurant based on most votes and least distance
            room.WinningRestaurant = room.Matches.OrderByDescending(m => (int)m.Action).ThenBy(m => m.Restaurant.Distance).FirstOrDefault()?.Restaurant;
            return true;
        }

        foreach (var restaurant in room.Restaurants)
        {
            int userMatches = room.Matches
                .Where(m => m.Restaurant.ID == restaurant.ID && (int)m.Action > 1)
                .Select(m => new { m.Restaurant.ID, m.User.Id })
                .Distinct().Count();

            if (userMaxSwipeCount + userMatches >= room.Users.Count)
            {
                room.Status = RoomStatus.Matched;
                room.WinningRestaurant = restaurant;
                return true;
            }
        }
        

        return false;
    }
}
