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
            CreatePage();
            Nav.Received(1).NavigateTo(Arg.Any<string>());
        }

        [Fact]
        public void RoomStateMatch_Should_NavToMatch()
        {
            Server.HasRoom.Returns(false);
            fixture.Customize<DinningRoom>(p => p.With(r => r.Status, RoomStatus.Matched));
            CreatePage();
            Nav.Received(1).NavigateTo(Arg.Any<string>());
        }

        private void CreatePage()
        {
            Page = context.RenderComponent<Room>(ComponentParameter.CreateParameter("Id", fixture.Create<int>()));
        }

        [Fact]
        public void RoomNull_Should_GoToIndexWithMessage()
        {
            Server.HasRoom.Returns(false);
            DinningRoom? room = null;
            Server.Room.Returns(room);
            CreatePage();
            var message = $"room {Page!.Instance.Id} does not exist";
            Nav.Received(1).NavigateTo($"/?message={Uri.EscapeDataString(message)}");
        }
    }
}