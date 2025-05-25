// Changelogs Date  | Author                | Description
// 2023-12-23       | Anthony Coudène       | Creation

namespace MyFeature.Data.Entities;

// This commented part could be used to have benefits of entity typing
//public abstract record MyEntityBase : IIdentifierEntity

public record MyEntity : IIdentifierEntity, ITimestampedEntity
{
  public required Guid Id { get; init; }

  public DateTimeOffset CreatedAt { get; init; }

  public DateTimeOffset UpdatedAt { get; init; }

  // TODO - EntityProperties - Fields to complete

  public string? Metadata { get; set; } // Example, to remove if needed
}

// This commented part could be used to have benefits of entity typing
// Example of inherited class
//public record MyEntityInherited : MyEntityBase
//{
//}