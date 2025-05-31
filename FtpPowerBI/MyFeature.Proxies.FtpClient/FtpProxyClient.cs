using CsvHelper;
using FluentFTP;
using Microsoft.Extensions.Logging;
using MyFeature.Dtos;
using Newtonsoft.Json;
using System.Globalization;
using System.Net;
using System.Text;

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
    byte[] buffer = ToCsv(dto);

    ftpClient.UploadBytes(buffer, $"{nameof(MyEntityDto)}-{dto.Id}.csv", FtpRemoteExists.Overwrite, true);
  }

  public virtual byte[] ToCsv(MyEntityDto dto)
  {
    using var memoryStream = new MemoryStream();
    using var writer = new StreamWriter(memoryStream, Encoding.UTF8);
    using var csv = new CsvWriter(writer, CultureInfo.InvariantCulture);

    csv.WriteRecord(dto);
    csv.Flush();

    return memoryStream.ToArray();
  }
}
