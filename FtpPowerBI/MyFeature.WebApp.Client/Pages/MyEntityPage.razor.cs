using MyFeature.Localization;
using MyFeature.RazorComponents;
using MyFeature.ViewModels;
using MyFeature.ViewObjects;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using MudBlazor;

namespace MyFeature.WebApp.Client.Pages;

public partial class MyEntityPage : ComponentBase
{
  private MyEntityForm? _form;

  [Inject]
  public required ISnackbar Snackbar { private get; init; }

  [Inject]
  public required ILogger<MyEntityPage> Logger { private get; init; }

  [Inject]
  public required IMyEntityViewModel ViewModel { private get; init; }

  [Inject]
  public required NavigationManager Navigation { private get; init; }

  [Inject]
  public required IStringLocalizer<MyFeatureResource> Localizer { private get; init; }

  [Parameter]
  public string? Id { get; set; } = null;

  protected override void OnInitialized()
  {
    if (Localizer is null)
      throw new InvalidOperationException($"Missing {nameof(Localizer)}");

    if (ViewModel is null)
      throw new InvalidOperationException($"Missing {nameof(ViewModel)}");

  }

  protected async Task InitByIdAsync()
  {
    ViewModel.SelectedItem = new MyEntityVo() { Id = Guid.NewGuid(), CreatedAt = DateTimeOffset.UtcNow, UpdatedAt = DateTimeOffset.UtcNow };

    if (!string.IsNullOrWhiteSpace(Id) && Guid.TryParse(Id, out Guid guid))
    {
      ViewModel.SelectedItem = await ViewModel.GetByIdAsync(guid);
    }

    if (ViewModel.SelectedItem is null)
    {
      throw new InvalidOperationException($"Missing {nameof(ViewModel.SelectedItem)}");
    }
  }

  private async Task ValidateSubmitAsync()
  {
    if (_form is null)
      return;

    if (ViewModel.SelectedItem is null)
      return;

    await _form.Validate();

    if (!_form.IsValid)
      return;

    await ViewModel.CreateOrUpdateAsync(ViewModel.SelectedItem);
    Snackbar.Add(Localizer["Saved!"]);
    Navigation.NavigateTo("/myEntitys");
  }
}
