namespace Core.Data;

public interface ITimestampedMongoEntity
{
  public DateTime CreatedAt { get; set; }
  public DateTime UpdatedAt { get; set; }
}
