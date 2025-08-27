# Objectif

En tant qu�utilisateur, je souhaite ajouter une photo en live via la cam�ra (sans passer par la galerie)
Ainsi lorsque je fais sauvegarder le formulaire, le serveur ou l�API va recevoir la photo et le formulaire.

# Projet

Une orchestration par .Net Aspire va lancer :
- Une application Blazor WASM permettant de capture une image de la camera
- Une utilisation d'un nuget ImageSharp pour reconstruire l'image
- Une API permettant de stocker les donn�es du formulaire et la photo
- Un stockage en BDD m�moire avec EF Core.

# R�f�rence

- Partie capture : https://wellsb.com/csharp/aspnet/blazor-webcam-capture