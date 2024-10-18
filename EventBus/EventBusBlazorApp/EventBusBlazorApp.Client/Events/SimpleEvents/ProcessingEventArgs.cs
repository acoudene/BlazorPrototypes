namespace EventBusBlazorApp.Client.Events.SimpleEvents;

public class ProcessingEventArgs : EventArgs
{
    public string Description { get; set; }

    public ProcessingEventArgs(string description)
    {
        Description = description;
    }
}
