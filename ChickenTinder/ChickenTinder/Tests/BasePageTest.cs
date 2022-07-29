
using ChickenTinder.Client.Services;
using Microsoft.AspNetCore.Components;

namespace Tests
{
    public abstract class BasePageTest<T> : IDisposable where T: IComponent
    {
        public TestContext context = new TestContext();
        public IRenderedComponent<T>? Page;
        public IServerConnection Server;
        public INavigationManager Nav;
        public Fixture fixture;
        public BasePageTest()
        {
            fixture = new Fixture();
            fixture.Customize(new AutoNSubstituteCustomization() { ConfigureMembers = true });
            Server = fixture.Freeze<IServerConnection>();
            Nav = fixture.Freeze<INavigationManager>();
            context.Services.AddSingleton(Server);
            context.Services.AddSingleton(fixture.Freeze<IInterloopService>());
            context.Services.AddSingleton(Nav);
            //Page = context.RenderComponent<T>();
        }

        public void Dispose()
        {
            context.Dispose();
        }

    }
}