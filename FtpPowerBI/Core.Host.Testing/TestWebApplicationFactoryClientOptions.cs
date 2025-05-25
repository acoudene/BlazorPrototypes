// Changelogs Date  | Author                | Description
// 2023-12-23       | Anthony Coudène       | Creation

using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.Mvc.Testing.Handlers;
using Microsoft.Extensions.Logging;

namespace Core.Host.Testing;

public class TestWebApplicationFactoryClientOptions : WebApplicationFactoryClientOptions
{
  public bool AllowLogs { get; set; } = true;

  private readonly ILogger? _logger;

  public TestWebApplicationFactoryClientOptions(ILogger? logger = null)
  {
    _logger = logger;
  }

  internal DelegatingHandler[] CreateHandlers()
  {
    return CreateHandlersCore().ToArray();

    IEnumerable<DelegatingHandler> CreateHandlersCore()
    {
      if (AllowAutoRedirect)
      {
        yield return new RedirectHandler(MaxAutomaticRedirections);
      }
      if (HandleCookies)
      {
        yield return new CookieContainerHandler();
      }
      if (AllowLogs)
      {
        yield return new LoggingHttpClientHandler(_logger);
      }
    }
  }
}
