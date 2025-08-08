namespace MyBlazor.Client.WorkerServices;

using Blazored.LocalStorage;
using System.Net.Http.Json;

public class VersionCheckService : IVersionCheckService
{
  private readonly HttpClient _httpClient;
  private readonly ILocalStorageService _localStorage;

  public VersionCheckService(HttpClient httpClient, ILocalStorageService localStorage)
  {
    _httpClient = httpClient;
    _localStorage = localStorage;
  }

  public async ValueTask<VersionInfo?> GetLocalVersionAsync()
  {
    return await _localStorage.GetItemAsync<VersionInfo>("app_version");
  }

  public async Task InitializeLocalVersionAsync()
  {
    var currentVersion = await _httpClient.GetFromJsonAsync<VersionInfo>($"version.json");
    await _localStorage.SetItemAsync("app_version", currentVersion);
  }

  public async Task<bool> IsNewVersionAvailableAsync()
  {
    var storedVersion = await GetLocalVersionAsync();

    var guid = Guid.NewGuid().ToString();
    var freshVersion = await _httpClient.GetFromJsonAsync<VersionInfo>($"version.json?nocache={guid}");
    if (freshVersion is null)
    {
      // If we can't get the fresh version, assume no update is available
      return false;
    }

    if (freshVersion.Version != storedVersion?.Version) // Every changes even below
    {
      // Detected update    
      return true;
    }

    return false;
  }
}
