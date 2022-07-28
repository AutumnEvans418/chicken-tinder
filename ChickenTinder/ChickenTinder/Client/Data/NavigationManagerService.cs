using Microsoft.AspNetCore.Components;

namespace ChickenTinder.Client.Data
{
    public class NavigationManagerService : INavigationManager
    {
        private readonly NavigationManager nav;

        public NavigationManagerService(NavigationManager nav)
        {
            this.nav = nav;
        }
        public string BaseUri => nav.BaseUri;
        public void NavigateTo(string url) => nav.NavigateTo(url);

        public void NavigateTo(string url, bool force) => nav.NavigateTo(url, force);
    }
}
