using FieldValidationBlazorApp.Client.Pages;
using FieldValidationBlazorApp.Components;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveWebAssemblyComponents();

//builder.Services.AddControllersWithViews()
//    .ConfigureApiBehaviorOptions(options =>
//    {
//      options.InvalidModelStateResponseFactory = context =>
//      {
//        if (context.HttpContext.Request.Path == "/StarshipValidation")
//        {
//          return new BadRequestObjectResult(context.ModelState);
//        }
//        else
//        {
//          return new BadRequestObjectResult(
//              new ValidationProblemDetails(context.ModelState));
//        }
//      };
//    });

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
//app.MapDefaultControllerRoute();
app.MapRazorComponents<App>()
    .AddInteractiveWebAssemblyRenderMode()
    .AddAdditionalAssemblies(typeof(FieldValidationBlazorApp.Client._Imports).Assembly);



app.Run();
