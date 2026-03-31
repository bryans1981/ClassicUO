using BrowserHost;
using BrowserHost.Services;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
builder.Services.AddScoped<EnvironmentProbeService>();
builder.Services.AddScoped<BrowserStorageService>();
builder.Services.AddScoped<BrowserAssetSourceService>();
builder.Services.AddScoped<TileDataProbeService>();
builder.Services.AddScoped<BrowserVirtualFileBridgeService>();

await builder.Build().RunAsync();

