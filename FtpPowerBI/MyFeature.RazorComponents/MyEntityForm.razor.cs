// Changelogs Date  | Author                | Description
// 2023-12-23       | Anthony Coudène       | Creation

using MyFeature.Localization;
using MyFeature.ViewObjects;
using MyFeature.ViewObjects.Validation;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;

namespace MyFeature.RazorComponents;

public partial class MyEntityForm
{
  [Inject]
  public required IStringLocalizer<MyFeatureResource> Localizer { private get; init; }

  [Parameter, EditorRequired]
  public required MyEntityVo ViewObject { get; set; }

  private MyEntityVoFluentValidator _myEntityValidator = new MyEntityVoFluentValidator();

  protected override void OnInitialized()
  {
    if (Localizer is null)
      throw new InvalidOperationException($"Misssing {nameof(Localizer)}");

    if (ViewObject is null)
      throw new InvalidOperationException($"Missing {nameof(ViewObject)}");
  }
}
