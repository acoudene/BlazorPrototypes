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
