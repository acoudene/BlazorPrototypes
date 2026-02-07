using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MudBlazor.Services;
using MyBlazor10.Client.WorkerServices;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.Services.AddMudServices();

///
/// Stamp
/// 
builder.Services.AddHttpClient<HttpAuthoritativeStampClient>(client => client.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress));
builder.Services.AddScoped<LocalStorageCachedStampClient>();
builder.Services.AddScoped<IStampChangeNotifier, LightSnackBarStampChangeNotifier>();
builder.Services.AddScoped<PollingStampCheckService>();

await builder.Build().RunAsync();
