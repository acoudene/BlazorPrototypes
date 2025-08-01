using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MyBlazorApp.Client.Handlers;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.Services.AddTransient<TimingHandler>();

builder.Services.AddScoped(sp =>
{
  var handler = sp.GetRequiredService<TimingHandler>();
  handler.InnerHandler = new HttpClientHandler();

  return new HttpClient(handler)
  {
    BaseAddress = new Uri(builder.HostEnvironment.BaseAddress)
  };
});

await builder.Build().RunAsync();
