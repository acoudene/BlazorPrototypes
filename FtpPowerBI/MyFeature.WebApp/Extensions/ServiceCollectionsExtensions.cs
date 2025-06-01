using MyFeature.Proxies;
using MyFeature.Proxies.Ftp;
using MyFeature.WebApp.Client.Extensions;
using System.Net;

namespace MyFeature.WebApp.Extensions;

public static class ServiceCollectionsExtensions
{
  public static void AddMyEntityApiClient(this IServiceCollection serviceCollection, Uri apiUri)
    => serviceCollection
    .AddClientsWithUri<IMyEntityClient, HttpMyEntityClient>(
      HttpMyEntityClient.ConfigurationName,
      apiUri);

  public static void AddMyEntityFtpExportClient(this IServiceCollection serviceCollection)
    => serviceCollection
    .AddTransient<NetworkCredential>(provider => new NetworkCredential("ftpuser", "ftppassword"))
    .AddSingleton<IFtpProxyClient, FtpProxyClient>();
}
