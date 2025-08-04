using Microsoft.JSInterop;
using MudBlazor;

namespace MyBlazor.Client.WorkerServices;

public class BackgroundTaskService
{
  private readonly PeriodicTimer _timer = new(TimeSpan.FromSeconds(2));
  
  private readonly VersionCheckService _versionCheckService;
  private readonly ISnackbar _snackbar;
  private readonly IJSRuntime _jsRuntime;

  public BackgroundTaskService(
    VersionCheckService versionCheckService, 
    ISnackbar snackbar,
    IJSRuntime jSRuntime)
  {
    _versionCheckService = versionCheckService ?? throw new ArgumentNullException(nameof(versionCheckService));
    _snackbar = snackbar ?? throw new ArgumentNullException(nameof(snackbar));
    _jsRuntime = jSRuntime ?? throw new ArgumentNullException(nameof(jSRuntime));
  }

  public async Task StartAsync()
  {
    await _versionCheckService.InitializeLocalVersionAsync();

    while (await _timer.WaitForNextTickAsync())
    {
      try
      {
        bool isUpdateAvailable = await _versionCheckService.IsNewVersionAvailableAsync();
        if (isUpdateAvailable)
        {
          _snackbar.Add("A new version of the application is available. Please reload the page.", Severity.Warning, 
            config =>
            {
              config.Action = "Reload";
              config.ActionColor = Color.Primary;
              config.OnClick = async (snackBar) =>
              {
                await _jsRuntime.InvokeVoidAsync("eval", "caches.keys().then(keys => keys.forEach(key => caches.delete(key))).then(() => location.reload());");
              };
            });
          continue;
        }

        _snackbar.Add("No updates available.", Severity.Success);
      }
      catch
      {
        // Ignore les erreurs réseau
      }
    }
  }
}
