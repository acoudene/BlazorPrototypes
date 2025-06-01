using CsvHelper;
using FluentFTP;
using Microsoft.Extensions.Logging;
using MyFeature.Dtos;
using System.Globalization;
using System.Net;
using System.Text;

namespace MyFeature.Proxies.Ftp;

public class FtpProxyClient : IFtpProxyClient
{
  public FtpProxyClient(ILogger<FtpProxyClient> logger)
  {
  }

  public virtual FtpClient InitializeFtpClient()
  {
    var client = new FtpClient("ftp://localhost");
    client.Credentials = new NetworkCredential("ftpuser", "ftppassword");
    client.Connect();
    return client;
  }

  public virtual Task ExportAsync(
    List<MyEntityDto> dtos,
    CancellationToken cancellationToken = default)
  {
    if (dtos is null || !dtos.Any())
      return Task.CompletedTask;

    var ftpClient = InitializeFtpClient();
    byte[] buffer = ToCsv(dtos);

    ftpClient.UploadBytes(buffer, $"{nameof(MyEntityDto)}-{DateTime.Now}.csv", FtpRemoteExists.Overwrite, true);
    return Task.CompletedTask;
  }

  public virtual byte[] ToCsv(List<MyEntityDto> dtos)
  {
    using var memoryStream = new MemoryStream();
    using var writer = new StreamWriter(memoryStream, Encoding.UTF8);
    using var csv = new CsvWriter(writer, CultureInfo.InvariantCulture);

    csv.WriteRecords(dtos);
    csv.Flush();

    return memoryStream.ToArray();
  }

}
