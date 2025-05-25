// Changelogs Date  | Author                | Description
// 2023-12-23       | Anthony Coudène       | Creation

using Microsoft.Extensions.Logging;

namespace Core.Host.Testing;

public class LoggingHttpClientHandler : DelegatingHandler
{
  private readonly ILogger? _logger;  

  public LoggingHttpClientHandler(ILogger? logger = null, HttpClientHandler? httpClientHandler = null)
    : base(httpClientHandler ?? new HttpClientHandler())
  {
    _logger = logger;    
  }

  protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
  {
    Action<string> log = str => Console.WriteLine(str);
    if (_logger is not null)
    {
      log = str => _logger.LogInformation(str);
    }

    log("Request:");
    log(request.ToString());
    if (request.Content != null)
    {
      log(await request.Content.ReadAsStringAsync());
    }
    
    HttpResponseMessage response = await base.SendAsync(request, cancellationToken);

    log("Response:");
    log(response.ToString());
    if (response.Content != null)
    {
      log(await response.Content.ReadAsStringAsync());
    }
    return response;
  }
}
