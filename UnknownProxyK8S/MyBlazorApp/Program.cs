using Microsoft.AspNetCore.HttpOverrides;
using MyBlazorApp;
using MyBlazorApp.Components;
using System.Net;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
  app.UseExceptionHandler("/Error", createScopeForErrors: true);
  // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
  app.UseHsts();
}

var options = new ForwardedHeadersOptions()
{
  ForwardedHeaders =
      ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
};

var knownProxies = app.Configuration.GetSection("KnownProxies").Get<KnownProxies>();
if (knownProxies?.IpAddresses is not null)
{
  foreach (var ipAddress in knownProxies.IpAddresses)
  {
    if (IPAddress.TryParse(ipAddress, out var address))
    {
      options.KnownProxies.Add(address);
    }
  }
}

var knownNetworks = app.Configuration.GetSection("KnownNetworks").Get<KnownNetworks>();
if (knownNetworks?.PrefixCdrNetworks is not null)
{
  foreach (var prefixCdrNetwork in knownNetworks.PrefixCdrNetworks)
  {
    if (Microsoft.AspNetCore.HttpOverrides.IPNetwork.TryParse(prefixCdrNetwork, out var address))
    {
      options.KnownNetworks.Add(address);
    }
  }
}

app.UseForwardedHeaders(options);

app.Use(async (context, next) =>
{
  // ⚠️ Si volontairement pas de KnownProxies, pas de KnownNetworks alors on avec le code ci-après on aura le warning
  // Sinon rajouter ces variables d'environnement par exemple dans launchSettings.json
  // "KnownProxies:IpAddresses:0": "10.244.87.43"
  // "KnownProxies:IpAddresses:1": "10.244.183.210"
  // "KnownNetworks:PrefixCdrNetworks:0": "10.244.0.0/16"

  // On simule une requête provenant d'un proxy inverse
  context.Request.Headers["X-Forwarded-For"] = "10.244.138.210";
  await next.Invoke();
});

app.UseHttpsRedirection();


app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
