// Changelogs Date  | Author                | Description
// 2023-12-23       | Anthony Coudène       | Creation

using MyFeature.Localization;
using MyFeature.ViewObjects;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;

namespace MyFeature.RazorComponents;

public partial class MyEntitysTable
{
  private string _searchString = string.Empty;

  /// Use it if needed for row edition template: private MyEntityVoFluentValidator _myEntityValidator = new MyEntityVoFluentValidator();

  [Inject]
  public required IStringLocalizer<MyFeatureResource> Localizer { private get; init; }

  [Parameter, EditorRequired]
  public required IEnumerable<MyEntityVo> ViewObjects { get; set; }

  [Parameter]
  public EventCallback<IEnumerable<MyEntityVo>?> ViewObjectsChanged { get; set; }

  [Parameter]
  public HashSet<MyEntityVo>? SelectedViewObjects { get; set; }

  [Parameter]
  public MyEntityVo? SelectedViewObject { get; set; }

  [Parameter]
  public EventCallback<HashSet<MyEntityVo>> OnSelectedItemsChanged { get; set; }

  protected override void OnInitialized()
  {
    if (Localizer is null)
      throw new InvalidOperationException($"Misssing {nameof(Localizer)}");

    if (ViewObjects is null)
      throw new InvalidOperationException($"Missing {nameof(ViewObjects)}");
  }

  private bool FilterFunc(MyEntityVo vo)
  {
    return vo switch
    {
      MyEntityVo x when x.Id.ToString().Contains(_searchString, StringComparison.OrdinalIgnoreCase) => true,
      MyEntityVo x when x.CreatedAt.ToString().Contains(_searchString, StringComparison.OrdinalIgnoreCase) => true,
      MyEntityVo x when x.UpdatedAt.ToString().Contains(_searchString, StringComparison.OrdinalIgnoreCase) => true,

      // TODO - Complete with search filter in grid

      MyEntityVo x when x.Metadata is not null && x.Metadata.ToString().Contains(_searchString, StringComparison.OrdinalIgnoreCase) => true,

      null => false,
      _ => false
    };
  }

  private ElementComparer MyEntityVoComparer = new();

  class ElementComparer : IEqualityComparer<MyEntityVo>
  {
    public bool Equals(MyEntityVo? a, MyEntityVo? b) => a?.Id == b?.Id;
    public int GetHashCode(MyEntityVo x) => HashCode.Combine(x?.Id);
  }
}
