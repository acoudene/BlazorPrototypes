using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MudBlazor.Services;
using MyBlazor.Client.WorkerServices;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.Services.AddScoped(sp => new HttpClient()
{
  BaseAddress = new Uri(builder.HostEnvironment.BaseAddress)
});
builder.Services.AddScoped<VersionCheckService>();
builder.Services.AddScoped<BackgroundTaskService>();
builder.Services.AddMudServices();
builder.Services.AddBlazoredLocalStorage();

var app = builder.Build();

var backgroundService = app.Services.GetRequiredService<BackgroundTaskService>();
_ = backgroundService.StartAsync(); // Ne pas attendre, sinon ça bloque

await app.RunAsync();
