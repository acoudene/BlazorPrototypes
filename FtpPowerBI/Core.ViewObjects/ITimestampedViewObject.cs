// Changelogs Date  | Author                | Description
// 2023-12-23       | Anthony Coudène       | Creation

namespace Core.ViewObjects;

public interface ITimestampedViewObject
{
  public DisplayableDateTime CreatedAt { get; }
  public DisplayableDateTime UpdatedAt { get; }
}
