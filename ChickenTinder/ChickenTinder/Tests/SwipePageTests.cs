using ChickenTinder.Client.Shared;

namespace Tests
{
    public class SwipePageTests : BasePageTest<SwipeList>
    {
        ISwipeJsInterop swipe;
        public SwipePageTests()
        {
            swipe = fixture.Freeze<ISwipeJsInterop>();
            context.Services.AddSingleton(swipe);
        }
        private void CreatePage()
        {
            Page = context.RenderComponent<SwipeList>(ComponentParameter.CreateParameter("Id", fixture.Create<int>()));
        }

        [Fact]
        public void RoomStateMatched_Should_NavigateToMatchPage()
        {
            Server.HasRoom.Returns(true);
            fixture.Customize<DinningRoom>(p => p.With(r => r.Status, RoomStatus.Matched));
            CreatePage();
            Nav.Received().NavigateTo(Arg.Any<string>());
        }

        [Fact]
        public void Interop_Should_BeStarted()
        {
            CreatePage();
            swipe.Received(2).Start();
        }

        [Fact]
        public void Restaurants_Should_HaveCount3()
        {
            CreatePage();
            Page!.Instance.Restaurants.Should().HaveCount(3);
        }

        [Theory]
        [InlineData(SwipeDirection.Up, UserAction.Love, 2)]
        [InlineData(SwipeDirection.Down, UserAction.No, 2)]
        [InlineData(SwipeDirection.Left, UserAction.Maybe, 3)]
        [InlineData(SwipeDirection.Right, UserAction.Like, 2)]
        public void OnSwipeAction_Should_Like(SwipeDirection action, UserAction result, int count)
        {
            CreatePage();
            swipe.OnSwiped += Raise.Event<EventHandler<SwipeDirection>>(this, action);
            Server.Received(1).Like(Arg.Any<string>(), result);

            Page!.Instance.Restaurants.Should().HaveCount(count);
        }

        [Fact]
        public void InvalidSwipe_Should_NotLike()
        {
            SwipeDirection action = 0;
            CreatePage();
            swipe.OnSwiped += Raise.Event<EventHandler<SwipeDirection>>(this, action);
            Server.DidNotReceive().Like(Arg.Any<string>(), Arg.Any<UserAction>());
        }

        [Theory]
        [InlineData(3, SwipeDirection.Down, true)]
        [InlineData(2, SwipeDirection.Down, false)]
        [InlineData(5, SwipeDirection.Left, false)]
        [InlineData(3, SwipeDirection.Left, false)]
        [InlineData(6, SwipeDirection.Left, true)]
        public void GoingThroughList_Should_SetPickyUser(int count, SwipeDirection direction, bool isPicky)
        {
            CreatePage();
            for (int i = 0; i < count; i++)
            {
                swipe.OnSwiped += Raise.Event<EventHandler<SwipeDirection>>(this, direction);
            }

            Server.Received(isPicky ? 1 : 0).SetPickyUser(Arg.Any<int>(), Arg.Any<string>());
        }
    }
}