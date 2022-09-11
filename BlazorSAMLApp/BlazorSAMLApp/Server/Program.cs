using ITfoxtec.Identity.Saml2;
using ITfoxtec.Identity.Saml2.MvcCore.Configuration;
using ITfoxtec.Identity.Saml2.Schemas.Metadata;
using Sustainsys.Saml2;
//using Sustainsys.Saml2.Metadata;

var builder = WebApplication.CreateBuilder(args);
//var connectionString = builder.Configuration.GetConnectionString("BlazorSAMLAppServerContextConnection") ?? throw new InvalidOperationException("Connection string 'BlazorSAMLAppServerContextConnection' not found.");

//builder.Services.AddDbContext<BlazorSAMLAppServerContext>(options =>
//    options.UseSqlServer(connectionString));

//builder.Services.AddDefaultIdentity<BlazorSAMLAppServerUser>(options => options.SignIn.RequireConfirmedAccount = true)
//    .AddEntityFrameworkStores<BlazorSAMLAppServerContext>();

// Add services to the container.

builder.Services.AddDatabaseDeveloperPageExceptionFilter();


//builder.Services
//  .AddIdentityServer()
//    .AddApiAuthorization<BlazorSAMLAppServerUser, BlazorSAMLAppServerContext>();

//builder.Services.AddAuthentication()
//    .AddIdentityServerJwt();


builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

//SAML
builder.Services.Configure<Saml2Configuration>(builder.Configuration.GetSection("Saml2"));

builder.Services.Configure<Saml2Configuration>(saml2Configuration =>
{
  saml2Configuration.AllowedAudienceUris.Add(saml2Configuration.Issuer);

  var entityDescriptor = new EntityDescriptor();
  entityDescriptor.ReadIdPSsoDescriptorFromUrl(new Uri(builder.Configuration["Saml2:IdPMetadata"]));
  if (entityDescriptor.IdPSsoDescriptor != null)
  {
    saml2Configuration.SingleSignOnDestination = entityDescriptor.IdPSsoDescriptor.SingleSignOnServices.First().Location;
    //saml2Configuration.SingleLogoutDestination = entityDescriptor.IdPSsoDescriptor.SingleLogoutServices.First().Location;
    saml2Configuration.SignatureValidationCertificates.AddRange(entityDescriptor.IdPSsoDescriptor.SigningCertificates);
  }
  else
  {
    throw new Exception("IdPSsoDescriptor not loaded from metadata.");
  }
});

builder.Services.AddSaml2();
//END SAML

//builder.Services.AddAuthentication()
//  .AddSaml2(options =>
//  {
//    options.SPOptions.EntityId = new EntityId("https://dev-51587557.okta.com");
//    options.IdentityProviders.Add(
//      new IdentityProvider(
//        new EntityId("https://dev-51587557.okta.com/app/exk5r6njsrnrv8FJS5d7/sso/saml/metadata"), options.SPOptions)
//        {
//          LoadMetadata = true
//        });

//                  //options.SPOptions.ServiceCertificates.Add(new X509Certificate2("Sustainsys.Saml2.Tests.pfx"));
//  });


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
  app.UseWebAssemblyDebugging();
}
else
{
  app.UseExceptionHandler("/Error");
  // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
  app.UseHsts();
}

app.UseHttpsRedirection();

app.UseBlazorFrameworkFiles();
app.UseStaticFiles();

app.UseRouting();
app.UseSaml2(); //SAML
app.UseAuthorization();

//app.UseEndpoints(endpoints =>
//{
//  endpoints.MapRazorPages();

//  endpoints.MapControllerRoute(
//      name: "default",
//      pattern: "{controller=Home}/{action=Index}/{id?}");
//});




app.MapRazorPages();
app.MapControllers();
app.MapFallbackToFile("index.html");

app.Run();
