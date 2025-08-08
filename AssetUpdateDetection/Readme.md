# L'idée est simple : 

- On a un service dédié à la détection de changement
- Une tâche de fond indépendante des pages ou des layout, lancée en tâche de fond, qui passe son temps à dire s'il faut mettre à jour ou non.

# Sur quoi est-ce basé ? 

- On stocke un fichier version.json qui est mis à jour automatiquement par build.
- Ce fichier est récupéré une première fois au chargement du blazor via une requête http (volontairement et pour subir le cache) puis mis à jour dans LocalStorage.
- Le service passe son temps à comparer le contenu avec celui de version.json du serveur en le concaténant à une QueryString bidon mais garantissant le nocache. Du genre, version.json?Nocache=<Guid.NewGuid()>

# Tests possibles

- Publier (toujours le serveur en Blazor), folder "MyBlazor\MyBlazor" :

`dotnet publish -c Release -o ./publish`

- Aller dans ./publish via cd ./publish, et lancer l'application :

`dotnet MyBlazor.dll`

- Ouvrir un navigateur sur le port mentionné (http://localhost:5000 par défaut)

Ensuite, amusez-vous à changer la valeur de la version directement dans le fichier version.json du répertoire wwwroot, changez également les images, etc...

# Code intéressant

Versioning automatique des images (à extrapoler à d'autres composants) :

```razor
<VersionedImg src="img/batman-figurine.jpg" alt="Batman" />
```

Est équivalent à :

```razor
<img src="img/batman-figurine.jpg?v=@_version?.Version" alt="Batman" />
```

La classe gérant la version :

```csharp
public class VersionCheckService : IVersionCheckService
{
  private readonly HttpClient _httpClient;
  private readonly ILocalStorageService _localStorage;

  public VersionCheckService(HttpClient httpClient, ILocalStorageService localStorage)
  {
    _httpClient = httpClient;
    _localStorage = localStorage;
  }

  public async ValueTask<VersionInfo?> GetLocalVersionAsync()
  {
    return await _localStorage.GetItemAsync<VersionInfo>("app_version");
  }

  public async Task InitializeLocalVersionAsync()
  {
    var currentVersion = await _httpClient.GetFromJsonAsync<VersionInfo>($"version.json");
    await _localStorage.SetItemAsync("app_version", currentVersion);
  }

  public async Task<bool> IsNewVersionAvailableAsync()
  {
    var storedVersion = await GetLocalVersionAsync();

    var guid = Guid.NewGuid().ToString();
    var freshVersion = await _httpClient.GetFromJsonAsync<VersionInfo>($"version.json?nocache={guid}");
    if (freshVersion is null)
    {
      // If we can't get the fresh version, assume no update is available
      return false;
    }

    if (freshVersion.Version != storedVersion?.Version) // Every changes even below
    {
      // Detected update    
      return true;
    }

    return false;
  }
}
```
