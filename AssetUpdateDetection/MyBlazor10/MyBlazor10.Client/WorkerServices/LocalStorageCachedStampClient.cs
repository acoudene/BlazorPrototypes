using Blazored.LocalStorage;

namespace MyBlazor10.Client.WorkerServices;

public class LocalStorageCachedStampClient
{
  private readonly ILocalStorageService _localStorage;
  private const string StorageKey = "app_stamp_info";

  public LocalStorageCachedStampClient(ILocalStorageService localStorage)
  {
    _localStorage = localStorage;
  }

  public async ValueTask<StampInfo?> GetStampAsync(CancellationToken cancellationToken = default)
  {
    return await _localStorage.GetItemAsync<StampInfo>(StorageKey, cancellationToken);
  }

  public async Task StoreStampAsync(StampInfo stampInfo, CancellationToken cancellationToken = default)
  {
    await _localStorage.SetItemAsync(StorageKey, stampInfo, cancellationToken);
  }
}
