namespace EventBusBlazorApp.Client.Events.BusEvents;

public class ProcessingBusEvent : EventBase<string>
{  
  public ProcessingBusEvent(object sender, string description) : base(sender, description)
  {
  }
}
