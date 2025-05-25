// Changelogs Date  | Author                | Description
// 2023-12-23       | Anthony Coudène       | Creation

using Core.ViewModels.BffProxying;
using MyFeature.ViewObjects;

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
}


