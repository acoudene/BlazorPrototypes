using System.Threading;

namespace MyFeature.WorkerService;

public class OneTimeWorker : BackgroundService
{
  private readonly ILogger<OneTimeWorker> _logger;
  private readonly ExporterProvider _exporterProvider;
  private readonly CancellationTokenSource _cancellationTokenSource;

  public OneTimeWorker(
    ILogger<OneTimeWorker> logger,
    ExporterProvider exporterProvider,
    CancellationTokenSource cancellationTokenSource)
  {
    _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    _exporterProvider = exporterProvider ?? throw new ArgumentNullException(nameof(exporterProvider));
    _cancellationTokenSource = cancellationTokenSource ?? throw new ArgumentNullException(nameof(cancellationTokenSource));
  }

  protected override async Task ExecuteAsync(CancellationToken stoppingToken)
  {
    if (_logger.IsEnabled(LogLevel.Information))
    {
      _logger.LogInformation("{Worker} running at: {Time}", nameof(OneTimeWorker), DateTimeOffset.Now);
    }

    // When the timer should have no due-time, then do the work once now.
    await _exporterProvider.ExecuteAsync(stoppingToken);

    // Signal cancellation to the executing method
    await _cancellationTokenSource.CancelAsync();
  }
}
