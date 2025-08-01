using System.Diagnostics;

namespace MyBlazorApp.Client.Handlers;

public class TimingHandler : DelegatingHandler
{
  protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
  {
    var stopwatch = Stopwatch.StartNew();

    var response = await base.SendAsync(request, cancellationToken);

    stopwatch.Stop();
    var elapsedMs = stopwatch.ElapsedMilliseconds;

    Console.WriteLine($"[HTTP] {request.Method} {request.RequestUri} - {elapsedMs} ms");

    return response;
  }
}

