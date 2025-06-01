using MyFeature.Localization;
using MyFeature.ViewModels;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using MudBlazor;

namespace MyFeature.WebApp.Client.Pages;

public partial class MyEntitysPage : ComponentBase
{
  [Inject]
  public required ISnackbar Snackbar { private get; init; }

  [Inject]
  public required ILogger<MyEntitysPage> Logger { private get; init; } 

  [Inject]
  public required IMyEntityViewModel ViewModel { private get; init; } 

  [Inject]
  public required NavigationManager Navigation { private get; init; } 

  [Inject]
  public required IStringLocalizer<MyFeatureResource> Localizer { private get; init; } 

  protected override void OnInitialized()
  {
    if (Localizer is null)
      throw new InvalidOperationException($"Missing {nameof(Localizer)}");

    if (ViewModel is null)
      throw new InvalidOperationException($"Missing {nameof(ViewModel)}");
  }

  protected Task AddViewObjectAsync()
  {
    Navigation.NavigateTo("/myEntity");
    return Task.CompletedTask;
  }

  protected Task UpdateViewObjectAsync()
  {
    if (ViewModel.SelectedItems.Count != 1)
      return Task.CompletedTask;

    Guid id = ViewModel.SelectedItems.Single().Id;
    if (id == Guid.Empty)
      return Task.CompletedTask;

    Navigation.NavigateTo($"/myEntity/{id}");
    return Task.CompletedTask;
  }

  protected async Task RemoveViewObjectAsync()
  {
    foreach (var item in ViewModel.SelectedItems)
    {
      await ViewModel.RemoveAsync(item.Id);
    }

    Snackbar.Add(Localizer["Deleted!"]);
    ViewModel.Items = await ViewModel.GetAllAsync();
  }

  protected async Task ExportViewObjectAsync()
  {    
    await ViewModel.ExportAsync(ViewModel.SelectedItems.ToList());
    Snackbar.Add(Localizer["Exported!"]);
  }
  
}
