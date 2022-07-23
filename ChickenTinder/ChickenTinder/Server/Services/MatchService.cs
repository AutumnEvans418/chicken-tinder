using ChickenTinder.Shared.Models;

namespace ChickenTinder.Server.Services
{
    public class MatchService
    {
        List<Match> _matches = new();

        public void AddLike(Match match, int userCount)
        {
            if (match.Restaurant?.ID == null) return;

            if (_matches.Count == 0 || !_matches.Any(m => m.Restaurant?.ID == match.Restaurant.ID))
            {
                _matches.Add(match);
            } else
            {
                if (_matches.Where(m => m.Restaurant?.ID == match.Restaurant.ID).Count() == userCount)
                {

                }
            }
        }

        //public Match? CheckForMatch(Match match)
        //{
        //    if (match.Restaurant?.ID == null) return null;

        //    if (_matches.Count == 0 || !_matches.Any(m => m.Restaurant?.ID == match.Restaurant.ID))
        //    {
        //        _matches.Add(match);
        //        return null;
        //    }
        //    else
        //    {
        //        if (_matches.Where(m => m.Restaurant?.ID == match.Restaurant.ID).Count() == userCount)
        //        {

        //        }
        //    }
        //}
    }
}
