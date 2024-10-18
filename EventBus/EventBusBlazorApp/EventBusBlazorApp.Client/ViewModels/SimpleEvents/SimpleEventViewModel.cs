using EventBusBlazorApp.Client.Events.SimpleEvents;

namespace EventBusBlazorApp.Client.ViewModels.SimpleEvents;

public class SimpleEventViewModel
{
    public event EventHandler<ProcessingEventArgs>? ProcessingEvent;
    public event EventHandler<ProcessedEventArgs>? ProcessedEvent;

    public async Task DoSomethingAsync()
    {
        Guid guid = Guid.NewGuid();

        ProcessingEvent?.Invoke(this, new ProcessingEventArgs($"Processing Id [{guid}]..."));

        await Task.Delay(2000);

        ProcessedEvent?.Invoke(this, new ProcessedEventArgs($"Processed Id [{guid}] successfully."));

    }
}
