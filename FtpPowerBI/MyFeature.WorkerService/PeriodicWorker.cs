namespace MyFeature.WorkerService;

public class PeriodicWorker : BackgroundService
{
  private readonly ILogger<PeriodicWorker> _logger;
  private int _executionCount;

  public PeriodicWorker(ILogger<PeriodicWorker> logger)
  {
    _logger = logger;
  }

  protected override async Task ExecuteAsync(CancellationToken stoppingToken)
  {
    _logger.LogInformation("{Worker} running.", nameof(PeriodicWorker));

    // When the timer should have no due-time, then do the work once now.
    await DoWork();

    using PeriodicTimer timer = new(TimeSpan.FromSeconds(30));

    try
    {
      while (await timer.WaitForNextTickAsync(stoppingToken))
      {
        await DoWork();
      }
    }
    catch (OperationCanceledException)
    {
      _logger.LogInformation("{Worker} is stopping.", nameof(PeriodicWorker));
    }
  }

  private async Task DoWork()
  {
    int count = Interlocked.Increment(ref _executionCount);

    // Simulate work
    await Task.Delay(TimeSpan.FromSeconds(2));

    _logger.LogInformation("{Worker} is working. Count: {Count}", nameof(PeriodicWorker), count);
  }
}