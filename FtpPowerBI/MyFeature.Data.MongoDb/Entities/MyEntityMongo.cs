// Changelogs Date  | Author                | Description
// 2023-12-23       | Anthony Coudène       | Creation

using Core.Data;

namespace MyFeature.Data.MongoDb.Entities;

// This commented part could be used to have benefits of mongo entity typing
//[BsonIgnoreExtraElements]
//[BsonDiscriminator("myEntity", Required = true, RootClass = true)]
//[BsonKnownTypes(typeof(MyEntityInheritedMongo))]
//public record MyEntityMongoBase : IIdentifierMongoEntity

[BsonIgnoreExtraElements]
[BsonDiscriminator("myEntity", Required = true)]
public record MyEntityMongo : IIdentifierMongoEntity, ITimestampedMongoEntity
{
  [BsonId]
  [BsonElement("_id")]
  [BsonRepresentation(representation: BsonType.ObjectId)]
  [BsonIgnoreIfDefault]
  public ObjectId ObjectId { get; set; }

  [BsonElement("uuid")]
  [BsonGuidRepresentation(GuidRepresentation.Standard)]
  public required Guid Id { get; set; }

  [BsonElement("createdAt")]
  [BsonRepresentation(representation: BsonType.DateTime)]
  [BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
  public DateTime CreatedAt { get; set; }

  [BsonElement("updatedAt")]
  [BsonRepresentation(representation: BsonType.DateTime)]
  [BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
  public DateTime UpdatedAt { get; set; }

  // TODO - EntityProperties - Fields to complete

  [BsonElement("metadata")]
  public string? Metadata { get; set; } // Example, to remove if needed
}

// This commented part could be used to have benefits of mongo entity typing
// Example of inherited class
//[BsonIgnoreExtraElements]
//[BsonDiscriminator("myEntityInherited", Required = true)]
//public record MyEntityInheritedMongo : MyEntityMongoBase
//{
//}