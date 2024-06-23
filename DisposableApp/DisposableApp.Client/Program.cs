using Alteva.Blazor.JsEvent.Services;
using DisposableApp.Client.Services;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.Services.AddScoped<InjectedServiceExample>();
builder.Services.AddScoped<JsEventService>();
builder.Services.AddScoped<JsUtilsService>();
await builder.Build().RunAsync();
