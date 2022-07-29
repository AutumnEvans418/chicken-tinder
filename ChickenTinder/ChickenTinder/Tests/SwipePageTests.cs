using ChickenTinder.Client.Shared;

namespace Tests
{
    public class SwipePageTests : BasePageTest<SwipeList>
    {
        public SwipePageTests()
        {
            context.Services.AddSingleton(fixture.Freeze<ISwipeJsInterop>());
        }

        [Fact]
        public void RoomStateMatched_Should_NavigateToMatchPage()
        {
            Server.HasRoom.Returns(true);
            fixture.Customize<DinningRoom>(p => p.With(r => r.Status, RoomStatus.Matched));

            Page = context.RenderComponent<SwipeList>(ComponentParameter.CreateParameter("Id", fixture.Create<int>()));
            Nav.Received().NavigateTo(Arg.Any<string>());
        }
    }
}