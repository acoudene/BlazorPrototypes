// Changelogs Date  | Author                | Description
// 2023-12-23       | Anthony Coudène       | Creation

using Core.ViewModels.BffProxying;
using Core.ViewObjects;
using MyFeature.ViewObjects;
using System.Net.Http;
using System.Net.Http.Json;

namespace MyFeature.ViewModels.BffProxying;

public class HttpMyEntityRestBffClientBehavior : HttpRestBffClientBehavior<MyEntityVo>
{
  /// <summary>
  /// Constructor
  /// </summary>
  /// <param name="httpClientFactory"></param>
  /// <exception cref="ArgumentNullException"></exception>
  public HttpMyEntityRestBffClientBehavior(IHttpClientFactory httpClientFactory) : base(httpClientFactory)
  {
  }

  public virtual async Task ExportAsync(
    List<MyEntityVo> toExportVos,
    string configurationName,
    bool checkSuccessStatusCode = true,
    CancellationToken cancellationToken = default)
  {
    if (string.IsNullOrWhiteSpace(configurationName))
      throw new InvalidOperationException("Missing configuration name");

    using HttpClient httpClient = HttpClientFactory.CreateClient(configurationName);
    var response = await httpClient.PostAsJsonAsync("Export", toExportVos, cancellationToken);
    if (response is null)
      throw new InvalidOperationException($"Problem while exporting resource from: [{httpClient.BaseAddress}]");

    if (checkSuccessStatusCode)
      response.EnsureSuccessStatusCode();
  }
}


