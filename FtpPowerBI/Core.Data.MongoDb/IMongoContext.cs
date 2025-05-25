// Changelogs Date  | Author                | Description
// 2023-12-23       | Anthony Coudène       | Creation

using MongoDB.Driver;

namespace Core.Data.MongoDb;

public interface IMongoContext
{
    IMongoDatabase GetDatabase();
}