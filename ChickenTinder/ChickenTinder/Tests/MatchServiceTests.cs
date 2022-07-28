
using ChickenTinder.Client.Services;
using ChickenTinder.Client.Pages;
namespace Tests
{
    public class RoomPageTests : IDisposable
    {
        TestContext context = new TestContext();
        IRenderedComponent<Room> Page;
        IServerConnection Server;
        INavigationManager Nav;
        Fixture fixture;
        public RoomPageTests()
        {
            fixture = new Fixture();
            fixture.Customize(new AutoNSubstituteCustomization() { ConfigureMembers = true });
            Server = fixture.Freeze<IServerConnection>();
            Nav = fixture.Freeze<INavigationManager>();
            context.Services.AddSingleton(Server);
            context.Services.AddSingleton(fixture.Freeze<IInterloopService>());
            context.Services.AddSingleton(Nav);
        }

        public void Dispose()
        {
            context.Dispose();
        }

        [Fact]
        public void RoomStateSwiping_Should_NavToSwipe()
        {
            Server.HasRoom.Returns(false);

            fixture.Customize<DinningRoom>(p => p.With(r => r.Status, RoomStatus.Swiping));
            Page = context.RenderComponent<Room>(ComponentParameter.CreateParameter("Id", fixture.Create<int>()));
            Nav.Received().NavigateTo(Arg.Any<string>());
        }

        [Fact]
        public void RoomStateMatch_Should_NavToMatch()
        {
            Server.HasRoom.Returns(false);
            fixture.Customize<DinningRoom>(p => p.With(r => r.Status, RoomStatus.Matched));
            Page = context.RenderComponent<Room>(ComponentParameter.CreateParameter("Id", fixture.Create<int>()));
            Nav.Received().NavigateTo(Arg.Any<string>());
        }
    }

    public class IndexPageTests
    {
        [Fact]
        public void IndexNullRoom_Should_NotNavigate()
        {
            using var context = new TestContext();

            var fixture = new Fixture();
            fixture.Customize(new AutoNSubstituteCustomization() { ConfigureMembers = true });
            var server = fixture.Freeze<IServerConnection>();
            DinningRoom? room = null;
            server.Room.Returns(room);

            var nav = fixture.Freeze<INavigationManager>();
            context.Services.AddSingleton(server);
            context.Services.AddSingleton(nav);
            var cut = context.RenderComponent<ChickenTinder.Client.Pages.Index>();

            cut.Instance.Join();
            nav.DidNotReceive().NavigateTo(Arg.Any<string>());
        }
    }

    public class MatchServiceTests
    {
        public MatchService MatchService { get; set; } = new MatchService();

        [Fact]
        public void MatchSingleUserLike_Should_BeTrue()
        {
            var fixture = new Fixture();
            var restaurants = fixture.CreateMany<Restaurant>(3);
            var host = new User();
            var room = new DinningRoom(host, restaurants.ToList());
            var match = new Match(host, restaurants.First(), UserAction.Like);
            MatchService.AddMatch(room, match);
            MatchService.CheckForMatch(room).Should().BeTrue();
        }

        [Fact]
        public void MatchSinglePickyUser_Should_BeTrue()
        {

            var fixture = new Fixture();
            var restaurants = fixture.CreateMany<Restaurant>(3);
            var host = new User();
            var room = new DinningRoom(host, restaurants.ToList());
            var match = new Match(host, restaurants.First(), UserAction.No);
            room.SetPickyUser(host.Id);
            MatchService.AddMatch(room, match);
            MatchService.CheckForMatch(room).Should().BeTrue();
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
            var room = new DinningRoom(host, restaurants);
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

            MatchService.CheckForMatch(room).Should().Be(result);
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