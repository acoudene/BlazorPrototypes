// Changelogs Date  | Author                | Description
// 2023-12-23       | Anthony Coudène       | Creation

using Microsoft.AspNetCore.Components;

namespace Core.RazorComponents;

public partial class ForEach<T> : ComponentBase
{
  [Parameter]
  public IEnumerable<T>? Items { get; set; }

  [Parameter]
  public RenderFragment<T>? ChildContent { get; set; }
}