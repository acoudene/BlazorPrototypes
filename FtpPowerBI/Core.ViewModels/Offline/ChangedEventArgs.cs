// Changelogs Date  | Author                | Description
// 2023-12-23       | Anthony Coudène       | Creation

namespace Core.ViewModels.Offline;

public class ChangedEventArgs
{
  public required string Key { get; set; }
  public required object OldValue { get; set; }
  public required object NewValue { get; set; }
}