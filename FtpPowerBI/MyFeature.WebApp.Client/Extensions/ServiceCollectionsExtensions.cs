using MyFeature.ViewModels.BffProxying;
using MyFeature.ViewModels;

namespace MyFeature.WebApp.Client.Extensions;

public static class ServiceCollectionsExtensions
{
  public const string MyEntityBffApiRelativePath = "api/MyEntityBff/";

  public static void AddClientsWithUri<TService, TImplementation>(
    this IServiceCollection serviceCollection,
    string name,
    Uri apiUri)
    where TService : class
    where TImplementation : class, TService
  {

    // Think to adapt to manage ending slash on basse uri
    Func<IServiceCollection, string, Uri, IHttpClientBuilder> defaultHttpClientbuilder = (service, name, uri) 
      => service.AddHttpClient(name, client => client.BaseAddress = uri);
    
    serviceCollection
      .AddClientsWithUri<TService, TImplementation>(name, apiUri, defaultHttpClientbuilder);   
  }

public static void AddClientsWithUri<TService, TImplementation>(
    this IServiceCollection serviceCollection, 
    string name, 
    Uri apiUri, 
    Func<IServiceCollection, string, Uri, IHttpClientBuilder> httpClientbuilder)
    where TService : class
    where TImplementation : class, TService
  {
    if (string.IsNullOrWhiteSpace(name))
      throw new ArgumentNullException(nameof(name));

    if (apiUri is null)
      throw new ArgumentNullException(nameof(apiUri));

    if (httpClientbuilder is null)
      throw new ArgumentNullException(nameof(httpClientbuilder));

    httpClientbuilder(serviceCollection, name, apiUri);
    serviceCollection.AddScoped<TService, TImplementation>();
  }

  public static void AddViewModels(this IServiceCollection serviceCollection)
  {
    serviceCollection.AddScoped<IMyEntityViewModel, MyEntityViewModel>();
  }  

  public static void AddBffClients(this IServiceCollection serviceCollection, Uri baseUri)
    => serviceCollection
    .AddClientsWithUri<IMyEntityRestBffClient, HttpMyEntityRestBffClient>(
      HttpMyEntityRestBffClient.ConfigurationName,
      new Uri(baseUri, MyEntityBffApiRelativePath));
}
