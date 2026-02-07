using System.Net.Http.Json;

namespace MyBlazor10.Client.WorkerServices;

public class HttpAuthoritativeStampClient
{
  private readonly HttpClient _httpClient;
  private const string StampInfoPath = "stampInfo.json";
  private const string NoCacheQueryStringKey = "nocache";

  public HttpAuthoritativeStampClient(HttpClient httpClient)
  {
    _httpClient = httpClient;
  }

  public async ValueTask<StampInfo?> GetStampAsync(CancellationToken cancellationToken = default)
  {
    var guid = Guid.NewGuid().ToString();
    return await _httpClient.GetFromJsonAsync<StampInfo>($"{StampInfoPath}?{NoCacheQueryStringKey}={guid}", cancellationToken);
  }
}