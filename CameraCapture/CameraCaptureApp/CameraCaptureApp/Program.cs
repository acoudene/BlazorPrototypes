using CameraCaptureApp.Client.ViewModels;
using CameraCaptureApp.Components;
using CameraCaptureApp.Models;
using CameraCaptureApp.Persistance.Context;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using MudBlazor.Services;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

// Add MudBlazor services
builder.Services.AddMudServices();

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveWebAssemblyComponents();

builder.Services.AddDbContext<PhotoCaptureDbContext>(opt => opt.UseInMemoryDatabase("PhotoCapturesDatabase"));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddHttpClient();

builder.Services.AddScoped<CameraCaptureViewModel>();

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


app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveWebAssemblyRenderMode()
    .AddAdditionalAssemblies(typeof(CameraCaptureApp.Client._Imports).Assembly);

app.MapGet("/api/photocaptures", async (PhotoCaptureDbContext dbContext) =>
    await dbContext.PhotoCaptures.ToListAsync());

app.MapGet("/api/photocaptures/{id}", async (Guid id, PhotoCaptureDbContext dbContext) =>
    await dbContext.PhotoCaptures.FindAsync(id)
        is PhotoCapture photoCapture
            ? Results.Ok(photoCapture)
            : Results.NotFound());


app.MapPost("/api/photocaptures/image", async (HttpRequest request, PhotoCaptureDbContext dbContext) =>
{

  using var reader = new StreamReader(request.Body);
  string? dataUri = await reader.ReadToEndAsync();
  if (string.IsNullOrWhiteSpace(dataUri))
    return TypedResults.NoContent();

  var photoCapture = new PhotoCapture
  {
    Id = Guid.NewGuid(),
    Timestamp = DateTimeOffset.UtcNow,
    DataUri = dataUri
  };
  dbContext.PhotoCaptures.Add(photoCapture);
  await dbContext.SaveChangesAsync();

  return Results.Created($"/api/photocaptures/{photoCapture.Id}", photoCapture);
});

app.Run();
