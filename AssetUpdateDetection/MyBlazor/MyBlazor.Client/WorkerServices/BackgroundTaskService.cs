using MudBlazor;

namespace MyBlazor.Client.WorkerServices;

public class BackgroundTaskService
{
  private readonly PeriodicTimer _timer = new(TimeSpan.FromSeconds(2));
  
  private readonly VersionCheckService _versionCheckService;
  private readonly ISnackbar _snackbar;

  public BackgroundTaskService(VersionCheckService versionCheckService, ISnackbar snackbar)
  {
    _versionCheckService = versionCheckService ?? throw new ArgumentNullException(nameof(versionCheckService));
    _snackbar = snackbar ?? throw new ArgumentNullException(nameof(snackbar));
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
          _snackbar.Add("Une nouvelle version de l'application est disponible. Veuillez recharger la page.", Severity.Info);
          continue;
        }

        _snackbar.Add("Aucune mise à jour disponible.", Severity.Success);
      }
      catch
      {
        // Ignore les erreurs réseau
      }
    }
  }
}
