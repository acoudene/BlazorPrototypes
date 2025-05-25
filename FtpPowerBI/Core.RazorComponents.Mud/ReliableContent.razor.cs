// Changelogs Date  | Author                | Description
// 2023-12-23       | Anthony Coudène       | Creation

using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using MudBlazor;

namespace Core.RazorComponents.Mud;

public partial class ReliableContent
{
  public bool IsBusy { get; set; } = true;

  public ErrorBoundary? RefErrorBoundary { get; set; }

  [Inject]
  public required ISnackbar Snackbar { private get; init; }

  [Inject]
  public required IStringLocalizer<ReliableContent> Localizer { private get; init; }

  [Inject]
  public required ILogger<ReliableContent> Logger { private get; init; }

  [Parameter]
  public int MaximumErrorCount { get; set; } = 2;

  [Parameter]
  public Func<Task>? LongRunningTask { get; set; }

  [Parameter]
  public RenderFragment? LoadingContentBody { get; set; }

  [Parameter]
  public RenderFragment? Body { get; set; }

  [Parameter]
  public RenderFragment? ChildContent { get => Body; set => Body = value; }

  [Parameter]
  public RenderFragment? ErrorBody { get; set; }

  protected void RecoverError()
  {
    RefErrorBoundary?.Recover();
  }

  protected override void OnParametersSet()
  {
    RecoverError();
  }

  protected override async Task OnInitializedAsync()
  {
    // Must be fatal even if we want to be reliable.
    if (Logger is null) throw new InvalidOperationException($"Missing {nameof(Logger)}!");
    if (Localizer is null) throw new InvalidOperationException($"Missing {nameof(Localizer)}!");
    if (Snackbar is null) throw new InvalidOperationException($"Missing {nameof(Snackbar)}!");

    try
    {      
      IsBusy = true;

      if (LongRunningTask is not null)
      {
        await LongRunningTask();
      }
    }
    catch (Exception ex)
    {
      Logger.LogError(ex, "[{Component}] Problem while initializing component, please see details for more information.", nameof(ReliableContent));
      Snackbar.Add(ex.Message, Severity.Error);
    }
    finally
    {
      IsBusy = false;
    }
  }
}
