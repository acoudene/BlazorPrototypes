namespace MyFeature.WorkerService;

public class OneTimeWorker : BackgroundService
{
  private readonly ILogger<OneTimeWorker> _logger;

  public OneTimeWorker(ILogger<OneTimeWorker> logger)
  {
    _logger = logger;
  }

  protected override async Task ExecuteAsync(CancellationToken stoppingToken)
  {
    if (_logger.IsEnabled(LogLevel.Information))
    {
      _logger.LogInformation("{Worker} running at: {Time}", nameof(OneTimeWorker), DateTimeOffset.Now);
    }
    
    await Task.CompletedTask; // Simulate some work being done
  }
}
