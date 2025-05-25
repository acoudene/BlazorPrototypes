// Changelogs Date  | Author                | Description
// 2023-12-23       | Anthony Coudène       | Creation

namespace Core.ViewModels.Offline;
public class ChangingEventArgs : ChangedEventArgs
{
  public bool Cancel { get; set; }
}