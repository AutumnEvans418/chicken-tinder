namespace Tests
{
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
}