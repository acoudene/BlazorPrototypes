using EventBusBlazorApp.Client.Events.BusEvents;
using EventBusBlazorApp.Client.ViewModels.BusEvents;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace EventBusBlazorApp.Client.Pages.BusEvents;

public partial class EventBusHome
{
  [Inject]
  public required ISnackbar Snackbar { get; set; }

  [Inject]
  public required IEventBus EventBus { get; set; }

  [Inject]
  public required EventBusViewModel ViewModel { get; set; }

  protected override void OnInitialized()
  {
    EventBus.Subscribe<ProcessingBusEvent>(@event => Snackbar.Add(@event.Data, Severity.Info));
    EventBus.Subscribe<ProcessedBusEvent>(@event => Snackbar.Add(@event.Data, Severity.Success));
  }

  private async Task OnClickAsync()
  {
    await ViewModel.DoSomethingAsync();
  }

}
