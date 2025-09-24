using CameraCaptureApp.Client.ViewModels;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MudBlazor.Services;
var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.Services.AddMudServices();

builder.Services.AddScoped<CameraCaptureViewModel>();
builder.Services.AddHttpClient<CameraCaptureViewModel>(httpClient =>
{
  httpClient.BaseAddress = new Uri(new Uri(builder.HostEnvironment.BaseAddress), "/api/photocaptures/");
});

await builder.Build().RunAsync();
