namespace EventBusBlazorApp.Client.Events.BusEvents;

public class ProcessedBusEvent : EventBase<string>
{  
  public ProcessedBusEvent(object sender, string description) : base(sender, description)
  {
  }
}
