using Blazor.IndexedDB;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.Services.AddScoped<IIndexedDbFactory, IndexedDbFactory>();

await builder.Build().RunAsync();
