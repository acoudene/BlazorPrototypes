// Changelogs Date  | Author                | Description
// 2023-12-23       | Anthony Coudène       | Creation

using Microsoft.AspNetCore.Mvc.Testing;

namespace Core.Host.Testing;

public class TestHttpClientFactory<TEntryPoint> : IHttpClientFactory where TEntryPoint : class
{
  private readonly WebApplicationFactory<TEntryPoint> _appFactory;
  private readonly TestWebApplicationFactoryClientOptions _options;
  public TestHttpClientFactory(
    WebApplicationFactory<TEntryPoint> appFactory,
    TestWebApplicationFactoryClientOptions options)
  {
    if (appFactory is null)
      throw new ArgumentNullException(nameof(appFactory));
    if (options is null)
      throw new ArgumentNullException(nameof(options));

    _appFactory = appFactory;
    _options = options;
  }

  public virtual HttpClient CreateClient(string name)
    => _appFactory.CreateDefaultClient(_options.BaseAddress, _options.CreateHandlers());


  // Warning be careful on Uri code when merging: 
  // - Given baseAddress = https://localhost:8080/BaseAddress and relativePath = /api/myApi
  // - When using new Uri(baseAddress, relativePath); 
  // - Then Uri will generate: https://localhost:8080/api/myApi (BaseAddress has been removed)
  // - Whereas expected Uri: https://localhost:8080/BaseAddress/api/myApi
  // Idem for this kind of merge: baseUri = https://localhost:8080/BaseAddress/api/myApi and relativePath=/Create
  private static Uri GenerateUrl(Uri baseAddress, string relativePath)
  {
    Uri baseUri = new Uri($"{baseAddress.AbsoluteUri.TrimEnd('/')}/", UriKind.Absolute); // Always add a slash at the end
    //Uri relativeUri = new Uri($"{relativePath.Trim('/')}/", UriKind.Relative); // Always add a slash at the end for a future concatenation
    return new Uri(baseUri, relativePath);
  }

  private static TestWebApplicationFactoryClientOptions SetBaseAddress(TestWebApplicationFactoryClientOptions options, string relativePath)
  {
    options.BaseAddress = GenerateUrl(options.BaseAddress, relativePath);
    return options;
  }

  public static IHttpClientFactory CreateHttpClientFactory(
    WebApplicationFactory<TEntryPoint> webApplicationFactory,
    string relativePath,
    TestWebApplicationFactoryClientOptions? givenOptions = null)
  {
    if (givenOptions is null)
      givenOptions = new TestWebApplicationFactoryClientOptions();

    var options = SetBaseAddress(givenOptions, relativePath);
    return new TestHttpClientFactory<TEntryPoint>(webApplicationFactory, options);
  }
}