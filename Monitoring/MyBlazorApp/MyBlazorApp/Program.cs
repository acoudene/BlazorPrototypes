using Microsoft.EntityFrameworkCore;
using MyBlazorApp.Components;
using MyBlazorApp.Data;
using System;

var builder = WebApplication.CreateBuilder(args);

//builder.Services.AddDbContext<MyBlazorAppContext>(options =>
//    options.UseNpgsql(builder.Configuration.GetConnectionString("MyBlazorAppContext") ?? throw new InvalidOperationException("Connection string 'MyBlazorAppContext' not found.")));

//builder.AddNpgsqlDbContext<MyBlazorAppContext>("postgresdb");

var connectionString = builder.Configuration.GetConnectionString("postgresdb");
builder.Services.AddDbContextPool<MyBlazorAppContext>(dbContextOptionsBuilder => dbContextOptionsBuilder.UseNpgsql(connectionString));
builder.EnrichNpgsqlDbContext<MyBlazorAppContext>();

builder.AddServiceDefaults();

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveWebAssemblyComponents();

builder.Services.AddControllers();

var app = builder.Build();

app.MapDefaultEndpoints();

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
    .AddAdditionalAssemblies(typeof(MyBlazorApp.Client._Imports).Assembly);

app.MapControllers();

using (var scope = app.Services.CreateScope())
{
  var dbContext = scope.ServiceProvider.GetRequiredService<MyBlazorAppContext>();
  dbContext.Database.Migrate(); // Applique toutes les migrations
}

app.Run();
