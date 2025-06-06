namespace MyFeature.WorkerService;

public record TimerOptions
{
  public TimeSpan Period { get; set; }

  public TimerOptions()
  {
    Period = TimeSpan.FromMinutes(2); // Default period of 2 minutes
  }
}