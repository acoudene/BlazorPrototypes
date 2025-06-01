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
  private readonly ILogger<FtpProxyClient> _logger;
  private readonly NetworkCredential _networkCredential;

  public FtpProxyClient(
    ILogger<FtpProxyClient> logger,
    NetworkCredential networkCredential)
  {
    _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    _networkCredential = networkCredential ?? throw new ArgumentNullException(nameof(networkCredential));
  }

  public virtual FtpClient InitializeFtpClient()
  {
    var client = new FtpClient("ftp://localhost");
    client.Credentials = _networkCredential;
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

    ftpClient.UploadBytes(buffer, $"{nameof(MyEntityDto)}-{DateTime.Now.ToString("yyyyMMdd-HHmmssf")}.csv", FtpRemoteExists.Overwrite, true);
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
