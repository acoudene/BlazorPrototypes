# Objectif

En tant qu’utilisateur, je souhaite ajouter une photo en live via la caméra (sans passer par la galerie)
Ainsi lorsque je fais sauvegarder le formulaire, le serveur ou l’API va recevoir la photo et le formulaire.

# Projet

Une orchestration par .Net Aspire va lancer :
- Une application Blazor WASM permettant de capture une image de la camera
- Une utilisation d'un nuget ImageSharp pour reconstruire l'image
- Une API permettant de stocker les données du formulaire et la photo
- Un stockage en BDD mémoire avec EF Core.

# Référence

- Partie capture : https://wellsb.com/csharp/aspnet/blazor-webcam-capture