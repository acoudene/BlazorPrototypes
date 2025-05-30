using FluentFTP;
using Microsoft.Extensions.Logging;
using MyFeature.Dtos;
using System.Net;

namespace MyFeature.Proxies.Ftp;

public class FtpProxyClient : HttpMyEntityClient
{
  public FtpProxyClient(
    ILogger<HttpMyEntityClient> logger,
    IHttpClientFactory httpClientFactory)
    : base(logger, httpClientFactory)
  {
  }

  public virtual FtpClient InitializeFtpClient()
  {
    var client = new FtpClient("ftp://localhost");
    client.Credentials = new NetworkCredential("ftpuser", "ftppassword");
    client.Connect();
    return client;
  }

  public override async Task CreateOrUpdateAsync(
    MyEntityDto dto,
    CancellationToken cancellationToken = default)
  {
    await base.CreateOrUpdateAsync(dto, cancellationToken);
    
    var ftpClient = InitializeFtpClient();
    ftpClient.UploadFile(@"C:\Temp\fichier.txt", "/fichier.txt");
  }
}
