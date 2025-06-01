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
  private readonly FtpClient _ftpClient;

  public FtpProxyClient(
    ILogger<FtpProxyClient> logger,
    FtpClient ftpClient)
  {
    _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    _ftpClient = ftpClient ?? throw new ArgumentNullException(nameof(ftpClient));
  }

  public virtual Task ExportAsync(
    List<MyEntityDto> dtos,
    CancellationToken cancellationToken = default)
  {
    if (dtos is null || !dtos.Any())
      return Task.CompletedTask;

    _ftpClient.Connect();
    byte[] buffer = ToCsv(dtos);

    _ftpClient.UploadBytes(buffer, $"{nameof(MyEntityDto)}-{DateTime.Now.ToString("yyyyMMdd-HHmmssf")}.csv", FtpRemoteExists.Overwrite, true);
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
