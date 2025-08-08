using Blazored.LocalStorage;
using MudBlazor.Services;
using MyBlazor.Client.WorkerServices;
using MyBlazor.Components;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveWebAssemblyComponents();

builder.Services.AddScoped<IVersionCheckService, VersionCheckService>();
builder.Services.AddMudServices();
builder.Services.AddBlazoredLocalStorage();

builder.Services.AddControllers();
builder.Services.AddHttpClient();

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

app.MapRazorComponents<App>()
    .AddInteractiveWebAssemblyRenderMode()
    .AddAdditionalAssemblies(typeof(MyBlazor.Client._Imports).Assembly);


// Par exemple, pour gérer des headers
//app.UseStaticFiles(new StaticFileOptions
//{
//  OnPrepareResponse = ctx =>
//  {
//    if (ctx.File.Name == "version.json")
//    {
//      ctx.Context.Response.Headers.Append("Cache-Control", "public,max-age=3600");
//    }
//  }
//});

app.MapControllers();

app.Run();
