_2026-02-06 - Anthony COUDENE - Création_

# Thématique

Détecter les changements d'état d'une application WASM lorsqu'une mise à jour a été effectuée côté Serveur.

## L'idée est simple : 

- On a un service dédié à la détection de changement
- Une tâche de fond indépendante des pages ou des layout, lancée en tâche de fond, qui passe son temps à dire s'il faut mettre à jour ou non.

## Sur quoi est-ce basé ? 

- On stocke un fichier stampInfo.json qui est mis à jour automatiquement par build.
- Ce fichier est récupéré une première fois au chargement du blazor via une requête http (volontairement et pour subir le cache) puis mis à jour dans LocalStorage.
- Le service passe son temps à comparer le contenu avec celui de version.json du serveur en le concaténant à une QueryString bidon mais garantissant le nocache. Du genre, stampInfo.json?Nocache=<Guid.NewGuid()>

## Tests possibles

- Publier (toujours le serveur en Blazor) : `dotnet publish -c Release -o ./publish`
- Aller dans ./publish via cd ./publish, et lancer l'application :
- Ouvrir un navigateur sur le port mentionné (http://localhost:5000 par défaut)
- Ensuite, amusez-vous à changer la valeur de la version directement dans le fichier version.json du répertoire wwwroot, changez également les images, etc...

## Code intéressant

### Versioning des assets
#### Avant .Net 10

Versioning automatique des images (à extrapoler à d'autres composants) :

```razor
<VersionedImg src="img/batman-figurine.jpg" alt="Batman" />
```

Est équivalent à :

```razor
<img src="img/batman-figurine.jpg?v=@_version?.Version" alt="Batman" />
```

#### Après .Net 10

Nouveautés :
- Référence : https://learn.microsoft.com/en-us/aspnet/core/release-notes/aspnetcore-10.0?view=aspnetcore-10.0
- Fingerprinting automatique via la variable tableau @Assets
```
<img src="@Assets["img/monimage.jpg"]" alt="Batman" />
```
- Caching automatique des ressources statiques avec gestion de version

### Côté client - Polling de scrutation de la version serveur

Par défaut, scrutation toutes les 15 secondes.

```csharp
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
            catch(Exception ex)                       
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
```

### Côté client - Classe gérant la version du produit localement

Dépendance au LocalStorage.

```csharp
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
```

### Côté client - Classe récupérant la version du serveur

```csharp
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
```

### Gestion des données de version

```csharp
public record StampInfo
{
    public required DateTimeOffset UpdatedAt { get; set; }
    public required string Identifier { get; set; }
}
```

### Côté client - règle de validation des différences

```csharp
public class AuthoritativeEqualToCachedStampRule
{
    public bool IsSatisfied(StampInfo? authoritativeStamp, StampInfo? cachedStamp)
    {
        if (cachedStamp is null)
            // If we can't get a client stamp, assume update is available
            return false;

        if (authoritativeStamp is null)
            // If we can't get a server stamp, assume no update is available
            return true;

        if (authoritativeStamp.Identifier != cachedStamp.Identifier)
            // Detected update    
            return false;

        return true; // To avoid infinite loop
    }
}
```

### Côté client - interface de notification 

```csharp
public interface IStampChangeNotifier
{
    bool Notify(StampInfo? previous, StampInfo current);
}
```

### Côté client - classe de notification - implémentation SnackBar

```csharp
public class LightSnackBarStampChangeNotifier : IStampChangeNotifier
{
    private readonly ISnackbar _snackbar;
    private readonly IJSRuntime _jsRuntime;

    public LightSnackBarStampChangeNotifier(
      ISnackbar snackbar,
      IJSRuntime jSRuntime)
    {
        _snackbar = snackbar ?? throw new ArgumentNullException(nameof(snackbar));
        _jsRuntime = jSRuntime ?? throw new ArgumentNullException(nameof(jSRuntime));
    }

    public bool Notify(StampInfo? previous, StampInfo current)
    {
        bool reloaded = false;
        _snackbar.Add(
            "Application has been updated. Please reload the page.",
            Severity.Warning,
            config => {
                config.RequireInteraction = false;
                config.HideTransitionDuration = 5000;
                config.Action = "Reload";
                config.ActionColor = Color.Primary;
                config.OnClick = async (snackBar) => {
                    await _jsRuntime.InvokeVoidAsync("eval", "caches.keys().then(keys => keys.forEach(key => caches.delete(key))).then(() => location.reload(true));");
                    reloaded = true;
                };
            });
        return reloaded;
    }
}
```

### Côté client - composant razor

```xml
@implements IDisposable
@inject PollingStampCheckService StampPoller

@code {
  protected override Task OnInitializedAsync()
  {
    _ = StampPoller.StartAsync()
        .ContinueWith(t => Console.Error.WriteLine(t.Exception),
            TaskContinuationOptions.OnlyOnFaulted);

    return Task.CompletedTask;
  }

  public void Dispose()
  {
    _ = StampPoller.DisposeAsync();
  }
}
```

### Côté client - instanciation stamp dans Program.cs

```csharp
///
/// Stamp
/// 
builder.Services.AddHttpClient<HttpAuthoritativeStampClient>(client => client.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress));
builder.Services.AddScoped<LocalStorageCachedStampClient>();
builder.Services.AddScoped<IStampChangeNotifier, LightSnackBarStampChangeNotifier>();
builder.Services.AddScoped<PollingStampCheckService>();
```

### Côté client - intégration au MainLayout.razor

```xml
<StampPollingBootstrap/>
```

### Côté serveur - stockage de la version

Fichier json stampInfo.json

```json
{ 
  "updatedAt": "2026-02-06T21:25:26.1580424+00:00", 
  "identifier": "20260206212526158-c0747943b1b84abcbe95a5bb212ad35a" 
}
```

### Côté serveur - Mise à jour de la version 

Utilisation du csproj en post build

```xml
<Target Name="GenerateStampInfoJson" AfterTargets="Build;Publish">
  <PropertyGroup>
    
    <StampInfoFilePath>wwwroot\stampInfo.json</StampInfoFilePath>
    
    <!-- Date UTC au format JSON/RFC3339 (ISO 8601) -->
    <UpdatedAtUtcIso>$([System.DateTimeOffset]::UtcNow.ToString("O"))</UpdatedAtUtcIso>

    <!-- Même instant, format compact triable pour l'identifiant -->
    <UpdatedAtUtcStamp>$([System.DateTimeOffset]::Parse('$(UpdatedAtUtcIso)').ToString("yyyyMMddHHmmssfff"))</UpdatedAtUtcStamp>

    <!-- Identifiant triable -->
    <Identifier>$(UpdatedAtUtcStamp)-$([System.Guid]::NewGuid().ToString("N"))</Identifier>

    <!-- JSON (mets des virgules + guillemets corrects) -->
    <GeneratedStampInfo>{ "updatedAt": "$(UpdatedAtUtcIso)", "identifier": "$(Identifier)" }</GeneratedStampInfo>
    
  </PropertyGroup>

  <Message Text="Stamp identifier: $(Identifier)" Importance="high" />

  <WriteLinesToFile File="$(StampInfoFilePath)" Lines="$(GeneratedStampInfo)" Overwrite="true" Encoding="UTF-8" />
</Target>
```

