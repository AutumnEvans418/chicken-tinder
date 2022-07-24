using ChickenTinder.Client;
using ChickenTinder.Client.Data;
using ChickenTinder.Client.Services;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddSingleton(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

builder.Services.AddSingleton<ServerConnection>();
builder.Services.AddSingleton<InterloopService>();
builder.Services.AddSingleton<UserService>();
builder.Services.AddSingleton<LocationService>();

await builder.Build().RunAsync();
