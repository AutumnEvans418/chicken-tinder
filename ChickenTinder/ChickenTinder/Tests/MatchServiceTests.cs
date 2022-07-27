
namespace Tests
{
    public class MatchServiceTests
    {
        public MatchService MatchService { get; set; } = new MatchService();

        [Fact]
        public void MatchSingleUserLike_Should_BeTrue()
        {
            var fixture = new Fixture();
            var restaurants = fixture.CreateMany<Restaurant>(3);
            var host = new User();
            var room = new DiningRoom(host, restaurants.ToList());
            var match = new Match(host, restaurants.First(), UserAction.Like);
            MatchService.CheckForMatch(room, match).Should().BeTrue();
        }

        [Fact]
        public void MatchSinglePickyUser_Should_BeTrue()
        {

            var fixture = new Fixture();
            var restaurants = fixture.CreateMany<Restaurant>(3);
            var host = new User();
            var room = new DiningRoom(host, restaurants.ToList());
            var match = new Match(host, restaurants.First(), UserAction.No);
            room.SetPickyUser(host.Id);

            MatchService.CheckForMatch(room, match).Should().BeTrue();
        }

        [Theory]
        [InlineData(1, UserAction.Like, false, 1, UserAction.Love, false, true, 1)]
        [InlineData(1, UserAction.Like, false, 1, UserAction.Maybe, false, false, null)]
        [InlineData(1, UserAction.Like, false, 1, UserAction.No, false, false, null)]
        [InlineData(1, UserAction.Like, false, 2, UserAction.Like, false, false, null)]
        [InlineData(1, UserAction.Like, true, 2, UserAction.Like, true, true, 1)]
        [InlineData(1, UserAction.Like, true, 2, UserAction.Love, true, true, 2)]
        public void MatchExamples(
            int? restId1, 
            UserAction action1, 
            bool isPicky1, 
            int? restId2, 
            UserAction action2, 
            bool isPicky2, 
            bool result,
            int? winningRestaurant)
        {
            var fixture = new Fixture();
            var restaurants = fixture.Build<Restaurant>().Without(p => p.Distance).CreateMany<Restaurant>(3).ToList();
            var host = new User();
            var room = new DiningRoom(host, restaurants);
            if (restId1 != null)
            {
                var match = new Match(host, restaurants[restId1.Value], action1);
                room.Matches.Add(match);
                if (isPicky1)
                {
                    room.SetPickyUser(host.Id);
                }
            }
            var user1 = new User();
            room.Join(user1);
            if (restId2 != null)
            {
                var match = new Match(user1, restaurants[restId2.Value], action2);
                room.Matches.Add(match);
                if (isPicky2)
                    room.SetPickyUser(user1.Id);
            }

            MatchService.CheckForMatch(room, room.Matches.LastOrDefault()).Should().Be(result);
            if (winningRestaurant != null)
            {
                room.WinningRestaurant.Should().Be(restaurants[winningRestaurant.Value]);
            }
            else
            {
                room.WinningRestaurant.Should().BeNull();
            }
        }
    }
}