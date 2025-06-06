using MyFeature.Proxies;
using MyFeature.Proxies.Ftp;

namespace MyFeature.WorkerService;

public class ExporterProvider
{
  private readonly ILogger<ExporterProvider> _logger;
  private readonly IMyEntityClient _client;
  private readonly IFtpProxyClient _ftpProxyClient;

  public ExporterProvider(
    ILogger<ExporterProvider> logger,
    IMyEntityClient client,
    IFtpProxyClient ftpProxyClient)
  {
    _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    _client = client ?? throw new ArgumentNullException(nameof(client));
    _ftpProxyClient = ftpProxyClient ?? throw new ArgumentNullException(nameof(ftpProxyClient));
  }

  public async Task ExecuteAsync(CancellationToken cancellationToken)
  {
    _logger.LogInformation("{Provider} is working.", nameof(ExporterProvider));

    var dtos = await _client.GetAllAsync(cancellationToken);
    await _ftpProxyClient.ExportAsync(dtos, cancellationToken);
  }
}
