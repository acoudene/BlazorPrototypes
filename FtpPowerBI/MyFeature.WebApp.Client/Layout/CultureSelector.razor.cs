using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System.Globalization;

namespace MyFeature.WebApp.Client.Layout;

public partial class CultureSelector : ComponentBase
{
	[Inject]
  public required NavigationManager Navigation { private get; init; }

	[Inject]
  public required IJSRuntime JSRuntime { private get; init; }

	protected override void OnInitialized()
	{
		if (Navigation is null)
			throw new InvalidOperationException($"Missing {nameof(Navigation)}");

		if (JSRuntime is null)
			throw new InvalidOperationException($"Missing {nameof(JSRuntime)}");
	}

	private CultureInfo[] cultures = new[]
	{
		new CultureInfo("en-US"),
		new CultureInfo("fr-FR")
	};

	private CultureInfo Culture
	{
		get => CultureInfo.CurrentCulture;
		set
		{
			if (CultureInfo.CurrentCulture != value)
			{
				var js = (IJSInProcessRuntime)JSRuntime;
				js.InvokeVoid("blazorCulture.set", value.Name);

				Navigation.NavigateTo(Navigation.Uri, forceLoad: true);
			}
		}
	}
}
