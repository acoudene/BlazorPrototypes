namespace MyBlazor10.Client.WorkerServices;

public class PollingStampCheckService : IAsyncDisposable
{
  private readonly HttpAuthoritativeStampClient _authoritativeStampClient;
  private readonly LocalStorageCachedStampClient _cachedStampClient;
  private readonly IStampChangeNotifier _notifier;
  private readonly TimeSpan _period;

  private CancellationTokenSource _cts;
  private Task? _loop;

  public PollingStampCheckService(
    HttpAuthoritativeStampClient authoritativeStampClient,
    LocalStorageCachedStampClient cachedStampClient,
    IStampChangeNotifier notifier,
    long pollingPeriodInSeconds = 15)
  {
    _authoritativeStampClient = authoritativeStampClient ?? throw new ArgumentNullException(nameof(authoritativeStampClient));
    _cachedStampClient = cachedStampClient ?? throw new ArgumentNullException(nameof(cachedStampClient));
    _notifier = notifier ?? throw new ArgumentNullException(nameof(notifier));
    _period = TimeSpan.FromSeconds(pollingPeriodInSeconds);
    _cts = new CancellationTokenSource();
  }

  public Task StartAsync()
  {
    if (_loop is not null) return Task.CompletedTask;

    if (_cts.IsCancellationRequested)
    {
      _cts.Dispose();
      _cts = new CancellationTokenSource();
    }

    _loop = RunAsync(_cts.Token);
    return Task.CompletedTask;
  }

  private async Task RunAsync(CancellationToken cancellationToken)
  {
    // Prime cache once
    var first = await _authoritativeStampClient.GetStampAsync(cancellationToken);
    if (first is not null)
      await _cachedStampClient.StoreStampAsync(first, cancellationToken);

    using var timer = new PeriodicTimer(_period);
    var rule = new AuthoritativeEqualToCachedStampRule();

    while (await timer.WaitForNextTickAsync(cancellationToken))
    {
      try
      {
        var authoritative = await _authoritativeStampClient.GetStampAsync(cancellationToken);
        if (authoritative is null)
          continue;

        var cached = await _cachedStampClient.GetStampAsync(cancellationToken);
        var isSatisfied = rule.IsSatisfied(authoritative, cached);
        if (!isSatisfied)
        {
          bool notified = _notifier.Notify(cached, authoritative);
          if (notified)
            await _cachedStampClient.StoreStampAsync(authoritative, cancellationToken);
        }
      }
      catch (OperationCanceledException)
      {
        break;
      }
      catch (Exception ex)
      {
        // Ignore network errors
#if DEBUG
        Console.Error.WriteLine(ex);
#endif
      }
    }
  }

  public async ValueTask DisposeAsync()
  {
    _cts.Cancel();

    try
    {
      if (_loop is not null)
        await _loop;
    }
    catch
    {
    }

    _cts.Dispose();
  }
}