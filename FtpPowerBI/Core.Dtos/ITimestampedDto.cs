// Changelogs Date  | Author                | Description
// 2023-12-23       | Anthony Coudène       | Creation

namespace Core.Dtos;

public interface ITimestampedDto
{
  public DateTimeOffset CreatedAt { get; }
  public DateTimeOffset UpdatedAt { get; }
}
