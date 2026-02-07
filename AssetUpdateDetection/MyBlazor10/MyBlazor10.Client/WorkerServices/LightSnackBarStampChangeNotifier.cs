using Microsoft.JSInterop;
using MudBlazor;
using Color = MudBlazor.Color;

namespace MyBlazor10.Client.WorkerServices;

public class LightSnackBarStampChangeNotifier : IStampChangeNotifier
{
  private readonly ISnackbar _snackbar;
  private readonly IJSRuntime _jsRuntime;

  public LightSnackBarStampChangeNotifier(
    ISnackbar snackbar,
    IJSRuntime jSRuntime)
  {
    _snackbar = snackbar ?? throw new ArgumentNullException(nameof(snackbar));
    _jsRuntime = jSRuntime ?? throw new ArgumentNullException(nameof(jSRuntime));
  }

  public bool Notify(StampInfo? previous, StampInfo current)
  {
    bool reloaded = false;
    _snackbar.Add(
        "Application has been updated. Please reload the page.",
        Severity.Warning,
        config =>
        {
          config.RequireInteraction = false;
          config.HideTransitionDuration = 5000;
          config.Action = "Reload";
          config.ActionColor = Color.Primary;
          config.OnClick = async (snackBar) =>
          {
            await _jsRuntime.InvokeVoidAsync("eval", "caches.keys().then(keys => keys.forEach(key => caches.delete(key))).then(() => location.reload(true));");
            reloaded = true;
          };
        });
    return reloaded;
  }
}