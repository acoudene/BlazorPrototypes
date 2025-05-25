namespace Core.Data;

public interface ITimestampedEntity
{
  public DateTimeOffset CreatedAt { get; }
  public DateTimeOffset UpdatedAt { get; }
}
