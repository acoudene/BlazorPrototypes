namespace EventBusBlazorApp.Client.Events.SimpleEvents;

public class ProcessedEventArgs : EventArgs
{
    public string Description { get; set; }

    public ProcessedEventArgs(string description)
    {
        Description = description;
    }
}
