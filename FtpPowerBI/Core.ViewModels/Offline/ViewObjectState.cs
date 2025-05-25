namespace Core.ViewModels.Offline;

[Flags]
public enum ViewObjectState
{
  Unchanged = 2,
  Added = 4,
  Deleted = 8,
  Modified = 16
}
