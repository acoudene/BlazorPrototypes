// Changelogs Date  | Author                | Description
// 2023-12-23       | Anthony Coudène       | Creation

using MongoDB.Bson;

namespace Core.Data.MongoDb;

public interface IMongoEntity : IEntity
{
  ObjectId ObjectId { get; set; }
}
