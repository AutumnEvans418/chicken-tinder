using ChickenTinder.Server.Hubs;
using ChickenTinder.Server.Managers;
using ChickenTinder.Shared.Managers;
using Microsoft.AspNetCore.ResponseCompression;
using System.Net.Http.Headers;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();
builder.Services.AddSignalR();

builder.Services.AddHttpClient("YelpClient", x=>
{
    x.BaseAddress = new Uri("https://api.yelp.com/v3/businesses/");

    x.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", "ffW4qTtxA6sIEDs9oDQuWwBho-X_cLugkW-oUwn1fF-UI1ADd8UfySOq-29IWT1AnZhAJGxzkh489AOPV7wLQr40zgQe2cq-AkriRGqrFXqbox27-_9MjTWpBanaYnYx");
});

builder.Services.AddSingleton<CodeManager>();
builder.Services.AddSingleton<RestaurantManager>();

builder.Services.AddSignalR();
builder.Services.AddResponseCompression(opts =>
{
    opts.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(
        new[] { "application/octet-stream" });
});

var app = builder.Build();
app.UseResponseCompression();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
}
else
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseBlazorFrameworkFiles();
app.UseStaticFiles();

app.UseRouting();

app.MapRazorPages();
app.MapControllers();
app.MapHub<TinderHub>("/tenderhub");
app.MapFallbackToFile("index.html");

app.Run();
