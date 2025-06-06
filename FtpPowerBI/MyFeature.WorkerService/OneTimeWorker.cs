namespace MyFeature.WorkerService;

public class OneTimeWorker : BackgroundService
{
  private readonly ILogger<OneTimeWorker> _logger;
  private readonly ExporterProvider _exporterProvider;

  public OneTimeWorker(
    ILogger<OneTimeWorker> logger,
    ExporterProvider exporterProvider)
  {
    _logger = logger;
    _exporterProvider = exporterProvider ?? throw new ArgumentNullException(nameof(exporterProvider));
  }

  protected override async Task ExecuteAsync(CancellationToken stoppingToken)
  {
    if (_logger.IsEnabled(LogLevel.Information))
    {
      _logger.LogInformation("{Worker} running at: {Time}", nameof(OneTimeWorker), DateTimeOffset.Now);
    }

    // When the timer should have no due-time, then do the work once now.
    await _exporterProvider.ExecuteAsync(stoppingToken);
  }
}
