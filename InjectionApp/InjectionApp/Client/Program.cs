using InjectionApp.Client;
using InjectionApp.Client.Services;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using static System.Formats.Asn1.AsnWriter;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

{
  // Provider: Singleton - Dependency: Singleton => OK

  var builderTest = WebAssemblyHostBuilder.CreateDefault(args);

  builderTest.Services.AddSingleton<IMyProvider, MyProvider>();
  builderTest.Services.AddSingleton<IMyDependency, MyDependency>();

  var hostTest = builderTest.Build();
  var myProviderTest = hostTest.Services.GetRequiredService<IMyProvider>();

  Console.WriteLine($"(Good practice) Provider: Singleton - Dependency: Singleton => Dependency Property: {myProviderTest.MyDependency.MyDependencyProperty}");
}
{
  // Provider: Singleton - Dependency: Scoped => KO

  var builderTest = WebAssemblyHostBuilder.CreateDefault(args);

  builderTest.Services.AddSingleton<IMyProvider, MyProvider>();
  builderTest.Services.AddScoped<IMyDependency, MyDependency>();

  var hostTest = builderTest.Build();
  try
  {
    var myProviderTest = hostTest.Services.GetRequiredService<IMyProvider>();
    Console.WriteLine($"(Bad practice) Provider: Singleton - Dependency: Scoped => Dependency Property: {myProviderTest.MyDependency.MyDependencyProperty}");
  }
  catch (InvalidOperationException ex)
  {
    // Expected exception:
    // Cannot consume scoped service 'InjectionApp.Client.Services.IMyDependency' from singleton 'InjectionApp.Client.Services.IMyProvider'.
    Console.WriteLine(ex.Message);
  }
}
{
  // Provider: Singleton - Dependency: Transient => OK

  var builderTest = WebAssemblyHostBuilder.CreateDefault(args);

  builderTest.Services.AddSingleton<IMyProvider, MyProvider>();
  builderTest.Services.AddTransient<IMyDependency, MyDependency>();

  var hostTest = builderTest.Build();
  
  var myProviderTest = hostTest.Services.GetRequiredService<IMyProvider>();
  Console.WriteLine($"(Bad practice) Provider: Singleton - Dependency: Transient => Dependency Property: {myProviderTest.MyDependency.MyDependencyProperty}");
}

{
  // Provider: Scoped - Dependency: Singleton => OK

  var builderTest = WebAssemblyHostBuilder.CreateDefault(args);

  builderTest.Services.AddScoped<IMyProvider, MyProvider>();
  builderTest.Services.AddSingleton<IMyDependency, MyDependency>();

  var hostTest = builderTest.Build();
  var myProviderTest = hostTest.Services.GetRequiredService<IMyProvider>();
  Console.WriteLine($"(Good practice) Provider: Scoped - Dependency: Singleton => Dependency Property: {myProviderTest.MyDependency.MyDependencyProperty}");
}

{
  // Provider: Scoped - Dependency: Scoped => OK

  var builderTest = WebAssemblyHostBuilder.CreateDefault(args);

  builderTest.Services.AddScoped<IMyProvider, MyProvider>();
  builderTest.Services.AddScoped<IMyDependency, MyDependency>();

  var hostTest = builderTest.Build();
  var myProviderTest = hostTest.Services.GetRequiredService<IMyProvider>();
  Console.WriteLine($"(Good practice) Provider: Scoped - Dependency: Scoped => Dependency Property: {myProviderTest.MyDependency.MyDependencyProperty}");
}

{
  // Provider: Scoped - Dependency: Transient => OK

  var builderTest = WebAssemblyHostBuilder.CreateDefault(args);

  builderTest.Services.AddScoped<IMyProvider, MyProvider>();
  builderTest.Services.AddTransient<IMyDependency, MyDependency>();

  var hostTest = builderTest.Build();

  var myProviderTest = hostTest.Services.GetRequiredService<IMyProvider>();
  Console.WriteLine($"(Bad practice) Provider: Scoped - Dependency: Transient => Dependency Property: {myProviderTest.MyDependency.MyDependencyProperty}");
}

{
  // Provider: Transient - Dependency: Singleton => OK

  var builderTest = WebAssemblyHostBuilder.CreateDefault(args);

  builderTest.Services.AddTransient<IMyProvider, MyProvider>();
  builderTest.Services.AddSingleton<IMyDependency, MyDependency>();

  var hostTest = builderTest.Build();
  var myProviderTest = hostTest.Services.GetRequiredService<IMyProvider>();
  Console.WriteLine($"(Good practice) Provider: Transient - Dependency: Singleton => Dependency Property: {myProviderTest.MyDependency.MyDependencyProperty}");
}

{
  // Provider: Transient - Dependency: Scoped => OK

  var builderTest = WebAssemblyHostBuilder.CreateDefault(args);

  builderTest.Services.AddTransient<IMyProvider, MyProvider>();
  builderTest.Services.AddScoped<IMyDependency, MyDependency>();

  var hostTest = builderTest.Build();
  var myProviderTest = hostTest.Services.GetRequiredService<IMyProvider>();
  Console.WriteLine($"(Good practice) Provider: Transient - Dependency: Scoped => Dependency Property: {myProviderTest.MyDependency.MyDependencyProperty}");
}

// Provider: Transient - Dependency: Transient => OK

builder.Services.AddTransient<IMyProvider, MyProvider>();
builder.Services.AddTransient<IMyDependency, MyDependency>();

var host = builder.Build();
var myProvider = host.Services.GetRequiredService<IMyProvider>();
Console.WriteLine($"(Good practice) Provider: Transient - Dependency: Transient => Dependency Property: {myProvider.MyDependency.MyDependencyProperty}");

// Result sample:
// (Good practice) Provider: Singleton - Dependency: Singleton => Dependency Property: 0342c5cf - 00a0 - 4406 - 9970 - ff659a02987b
// Cannot consume scoped service 'InjectionApp.Client.Services.IMyDependency' from singleton 'InjectionApp.Client.Services.IMyProvider'.
// (Bad practice) Provider: Singleton - Dependency: Transient => Dependency Property: 0c059b3e - 0733 - 4501 - b278 - fcca8216d3e1
// (Good practice) Provider: Scoped - Dependency: Singleton => Dependency Property: 0b723513 - 5122 - 47bb - 9c90 - 1e51fa7c13d5
// (Good practice) Provider: Scoped - Dependency: Scoped => Dependency Property: 27896ed7 - e01a - 4834 - 8ac2 - 39b5c61c74e4
// (Bad practice) Provider: Scoped - Dependency: Transient => Dependency Property: be558031 - 8111 - 48b7 - 9439 - 72ae9cd4b214
// (Good practice) Provider: Transient - Dependency: Singleton => Dependency Property: d25751bf - e746 - 41a2 - 84af - f3e5dc6e76a4
// (Good practice) Provider: Transient - Dependency: Scoped => Dependency Property: 80975183 - 5649 - 4253 - bc4b - 5f0cc79e17ac
// (Good practice) Provider: Transient - Dependency: Transient => Dependency Property: b0d6c08f - f2d6 - 4552 - b181 - 38b034fc8769

// Autres injections
{
  // Use [Inject] attribute, only activated for razor page, does not work in IServiceCollection
  var builderTest = WebAssemblyHostBuilder.CreateDefault(args);

  builderTest.Services.AddTransient<IMyDependency, MyRazorDependency>();
  builderTest.Services.AddSingleton<IMyProvider, MyRazorProvider>();

  var hostTest = builderTest.Build();
  var myProviderTest = hostTest.Services.GetRequiredService<IMyProvider>();

  // Expected trace: myProviderTest is not null but myProviderTest.MyDependency is null
  Console.WriteLine($"myProviderTest is not null but myProviderTest.MyDependency is {(myProviderTest.MyDependency?.MyDependencyProperty ?? "null")}");
}


// Conseils :
// -	L’IoC/DI via IServiceCollection ne sait faire que de l’injection par constructeur.
// -	L’attribut @inject (razor) ou[Inject](C# code-behind razor page) ne fonctionne que pour le DI effectué dans une page Razor mais est inopérant via IServiceCollection (donc la propriété aura null comme dans ton cas.
// -	Il ne faut pas utiliser des portées de type static ou[ThreadStatic] dans les objets que l’on veut injecter, même pour des objets de type helper/outils. Il est préférable d’utiliser le moteur d’IoC/DI pour les marquer à Singleton.
// -	Il ne faut pas injecter une portée Scoped dans une instance Singleton.
// - 	Enfin, lorsqu’on utilise un contexte nullable, il est important de marquer une propriété injectée par [Inject] avec l’opérateur null-forgiving (ou null indulgent) !
// - Exemple:
//   [Inject]
//   public IMyDependency MyDependency { get; set; } = default!;

await host.RunAsync();