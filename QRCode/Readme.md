# Objectif

G�n�rer un QR Code avec une url sous la forme d'une image

# Comment

- Utilisation des fonctionnalit�s de SyncFusion : https://www.syncfusion.com/blazor-components/blazor-barcode
- Adaptation � MudBlazor.

# Avertissement

- Utilisation volontaire ici des versions : 28.1.39
- Attention, si on ajoute le nuget Syncfusion.Blazor avec le nuget Syncfusion.Blazor.BarcodeGenerator, il y a des conflits � r�soudre. A ce jour, j'ai utilis� ces d�pendances :
	- Syncfusion.Blazor.BarcodeGenerator" Version="28.1.39"
  - Syncfusion.Blazor.Buttons" Version="28.1.39"
  - Syncfusion.Blazor.DropDowns" Version="28.1.39"
  - Syncfusion.Blazor.Inputs" Version="28.1.39"
  - Syncfusion.Blazor.Themes" Version="28.1.39"

# Autres solutions � creuser

- PinguApps.Blazor.QRCode (https://github.com/PinguApps/Blazor.QRCode)
- QRCode (https://www.c-sharpcorner.com/article/create-qr-code-in-blazor-using-asp-net-core/)
- Blazorise QRCode (https://blazorise.com/docs/extensions/qrcode)