// Changelogs Date  | Author                | Description
// 2023-12-23       | Anthony Coudène       | Creation

using Core.ViewModels;
using Core.ViewObjects;
using MyFeature.ViewObjects;

namespace MyFeature.ViewModels;

/// <summary>
/// Dedicated interface to manage ViewModel for a dedicated entity
/// </summary>
public interface IMyEntityViewModel : IViewModel<MyEntityVo>
{
  IEnumerable<MyEntityVo> Items { get; set; }
  HashSet<MyEntityVo> SelectedItems { get; set; }
  MyEntityVo? SelectedItem { get; set; }

  Task ExportAsync(List<MyEntityVo> toExportVos, CancellationToken cancellationToken = default);
}