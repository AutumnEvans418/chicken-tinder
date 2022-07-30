
namespace Tests
{
    public class RoomServiceTests
    {
        Fixture fixture;
        RoomService roomService;
        public RoomServiceTests()
        {
            fixture = new Fixture();
            fixture.Customize(new AutoNSubstituteCustomization() { ConfigureMembers = true });

            roomService = fixture.Create<RoomService>();
            roomService.MinId = 1;
            roomService.MaxId = 5;
        }

        [Fact]
        public async Task Room_Should_NotCreateDuplicateIds()
        {
            for (int i = 0; i < 20; i++)
            {
                var room = await roomService.CreateRoom(fixture.Create<User>());
                room.Should().NotBeNull();
            }
        }
    }

    public class UnitTest1
    {
        [Fact]
        public void Test1()
        {

        }
    }
}