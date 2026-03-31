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
builder.Services.AddScoped<BrowserProcessedAssetCacheService>();
builder.Services.AddScoped<IBrowserAssetStreamSource>(sp => sp.GetRequiredService<BrowserAssetSourceService>());
builder.Services.AddScoped<BrowserAssetLoaderHarnessService>();
builder.Services.AddScoped<BrowserTileDataReaderService>();
builder.Services.AddScoped<BrowserClilocReaderService>();
builder.Services.AddScoped<BrowserHuesReaderService>();
builder.Services.AddScoped<BrowserBootstrapAssetService>();
builder.Services.AddScoped<BrowserRuntimeBootstrapAssetService>();
builder.Services.AddScoped<IBrowserRuntimeBootstrapAssets>(sp => sp.GetRequiredService<BrowserRuntimeBootstrapAssetService>());
builder.Services.AddScoped<IBrowserClientAssetService, BrowserClientAssetService>();
builder.Services.AddScoped<BrowserVirtualFileBridgeService>();

await builder.Build().RunAsync();

