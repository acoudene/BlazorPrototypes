using Microsoft.Extensions.Options;

namespace MyFeature.WorkerService;

public class PeriodicWorker : BackgroundService
{
  private readonly ILogger<PeriodicWorker> _logger;
  private readonly ExporterProvider _exporterProvider;
  private readonly TimerOptions _timerOptions;

  public PeriodicWorker(
    ILogger<PeriodicWorker> logger,
    IOptions<TimerOptions> timerOptions,
    ExporterProvider exporterProvider)
  {
    _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    _timerOptions = timerOptions?.Value ?? throw new ArgumentNullException(nameof(timerOptions));
    _exporterProvider = exporterProvider ?? throw new ArgumentNullException(nameof(exporterProvider));    
    
    if (_timerOptions.Period <= TimeSpan.Zero)
    {
      throw new ArgumentOutOfRangeException(nameof(timerOptions), "Period period must be greater than zero.");
    }
  }

  protected override async Task ExecuteAsync(CancellationToken stoppingToken)
  {    
    _logger.LogInformation("{Worker} running.", nameof(PeriodicWorker));

    // When the timer should have no due-time, then do the work once now.
    await _exporterProvider.ExecuteAsync(stoppingToken);

    // Use a PeriodicTimer to execute the work at regular intervals.   
    using PeriodicTimer timer = new(_timerOptions.Period);

    try
    {
      while (await timer.WaitForNextTickAsync(stoppingToken))
      {
        await _exporterProvider.ExecuteAsync(stoppingToken);
      }
    }
    catch (OperationCanceledException)
    {
      _logger.LogInformation("{Worker} is stopping.", nameof(PeriodicWorker));
    }
    catch (Exception ex)
    {
      _logger.LogError(ex, "{Worker} encountered an error.", nameof(PeriodicWorker));      
    }
  }
}