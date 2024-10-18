using EventBusBlazorApp.Client.Events.SimpleEvents;
using EventBusBlazorApp.Client.ViewModels.SimpleEvents;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace EventBusBlazorApp.Client.Pages.SimpleEvents;

public partial class SimpleEventHome
{
    [Inject]
    public required ISnackbar Snackbar { get; set; }

    private readonly SimpleEventViewModel ViewModel = new();

    protected override void OnInitialized()
    {
        ViewModel.ProcessingEvent += ViewModel_ProcessingEvent;
        ViewModel.ProcessedEvent += ViewModel_ProcessedEvent;
    }

    private void ViewModel_ProcessingEvent(object? sender, ProcessingEventArgs e)
    {
        Snackbar.Add(e.Description, Severity.Info);
    }

    private void ViewModel_ProcessedEvent(object? sender, ProcessedEventArgs e)
    {
        Snackbar.Add(e.Description, Severity.Success);
    }

    private async Task OnClickAsync()
    {
        await ViewModel.DoSomethingAsync();
    }

}
