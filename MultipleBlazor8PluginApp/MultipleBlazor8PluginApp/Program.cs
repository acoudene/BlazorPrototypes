using MultipleBlazor8PluginApp.Client;
using MultipleBlazor8PluginApp.Components;
using OtherBlazorApp;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveWebAssemblyComponents();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
  app.UseWebAssemblyDebugging();
}
else
{
  app.UseExceptionHandler("/Error", createScopeForErrors: true);
  // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
  app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapWhen(ctx => ctx.Request.Path.StartsWithSegments("/OtherBlazorApp"), second =>
{
  second.UseStaticFiles();
  second.UseStaticFiles("/OtherBlazorApp");

  second.UseRouting();
  second.UseAntiforgery();
  second.UseEndpoints(endpoints =>
  {
    endpoints.MapRazorComponents<OtherApp>()
    .AddInteractiveWebAssemblyRenderMode();
  });
});

app.MapWhen(ctx => ctx.Request.Path.StartsWithSegments("/MainApp"), second =>
{
  second.UseStaticFiles();
  second.UseStaticFiles("/MainApp");

  second.UseRouting();
  second.UseAntiforgery();
  second.UseEndpoints(endpoints =>
  {
    endpoints.MapRazorComponents<App>()
        .AddInteractiveWebAssemblyRenderMode()
        .AddAdditionalAssemblies(typeof(MultipleBlazor8PluginApp.Client._Imports).Assembly);
  });
});


app.Run();
