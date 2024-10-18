using EventBusBlazorApp.Client.ViewModels.BusEvents;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MudBlazor.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.Services.AddMudServices();

builder.Services.AddSingleton<IEventBus, EventBus>();
builder.Services.AddTransient<EventBusViewModel>();

await builder.Build().RunAsync();
