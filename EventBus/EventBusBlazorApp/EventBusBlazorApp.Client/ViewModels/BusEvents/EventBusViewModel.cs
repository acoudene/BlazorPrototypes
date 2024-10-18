using EventBusBlazorApp.Client.Events.BusEvents;

namespace EventBusBlazorApp.Client.ViewModels.BusEvents;

public class EventBusViewModel
{
  public readonly IEventBus _eventBus;

  public EventBusViewModel(IEventBus eventBus) => _eventBus = eventBus;

  public async Task DoSomethingAsync()
  {
    Guid guid = Guid.NewGuid();

    _eventBus.Publish(new ProcessingBusEvent(this, $"Processing Id [{guid}]..."));

    await Task.Delay(2000);

    _eventBus.Publish(new ProcessedBusEvent(this, $"Processed Id [{guid}] successfully."));
  }
}
