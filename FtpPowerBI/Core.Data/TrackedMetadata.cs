// Changelogs Date  | Author                | Description
// 2023-12-23       | Anthony Coudène       | Creation

namespace Core.Data;

public record TrackedMetadata : ITrackedMetadata
{
  public DateTimeOffset LoggedAt { get; set; }
}
