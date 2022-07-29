using ChickenTinder.Client;
using ChickenTinder.Client.Data;
using ChickenTinder.Client.Services;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddSingleton(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

builder.Services.AddSingleton<IServerConnection, ServerConnection>();
builder.Services.AddSingleton<ISwipeJsInterop, SwipeJsInterop>();
builder.Services.AddSingleton<IInterloopService, InterloopService>();
builder.Services.AddSingleton<LocationService>();
builder.Services.AddScoped<INavigationManager, NavigationManagerService>();
await builder.Build().RunAsync();
