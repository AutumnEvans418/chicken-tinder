using ChickenTinder.Client.Services;

namespace Tests
{
    public class IndexPageTests : BasePageTest<ChickenTinder.Client.Pages.Index>
    {

        [Fact]
        public void IndexNullRoom_Should_NotNavigate()
        {
            DinningRoom? room = null;
            Server.Room.Returns(room);
            CreatePage();
            Page!.Instance.Join();
            Nav.DidNotReceive().NavigateTo(Arg.Any<string>());
        }

        private void CreatePage()
        {
            Page = context.RenderComponent<ChickenTinder.Client.Pages.Index>();
        }

        [Theory]
        [InlineData(true, "", true)]
        [InlineData(true, "test", true)]
        [InlineData(false, "", false)]
        [InlineData(false, null, false)]
        [InlineData(false, "test", true)]
        public void Start_Should_BeDisabled(bool locationSet, string? code, bool isEnabled)
        {
            Server.IsLocationSet.Returns(locationSet);

            CreatePage();
            Page!.Instance.Code = code;
            Page.Render();
            var start = Page!.Find("#start");
            var dis = start.Attributes["disabled"];
            if (isEnabled)
            {
                dis.Should().BeNull();
            }else
            {
                dis.Should().NotBeNull();
            }
        }
    }
}