using ChickenTinder.Client.Pages;

namespace Tests
{

    public class RoomPageTests : BasePageTest<Room>
    {
        
        public RoomPageTests()
        {
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
}