// Changelogs Date  | Author                | Description
// 2023-12-23       | Anthony Coudène       | Creation

using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace Core.Data.MongoDb;

public class MongoContext : IMongoContext
{
  private readonly IMongoDatabase _database;

  public IMongoDatabase GetDatabase() => _database;

  public MongoContext(IOptions<DatabaseSettings> databaseSettings)
          : this(databaseSettings.Value)
  {
  }

  public MongoContext(DatabaseSettings databaseSettings)
  {
    if (databaseSettings is null) throw new ArgumentNullException(nameof(databaseSettings));

    /// Extract connection string and database name
    /// from configuration settings, then create a mongodb instance of <see cref="IMongoDatabase"/>

    string connectionString = databaseSettings.ConnectionString;
    if (string.IsNullOrWhiteSpace(connectionString))
      throw new InvalidOperationException("Missing connection string");

    string databaseName = databaseSettings.DatabaseName;
    if (string.IsNullOrWhiteSpace(databaseName))
      throw new InvalidOperationException("Missing database name");

    var mongoClient = new MongoClient(connectionString);
    _database = mongoClient.GetDatabase(databaseName);

    if (_database is null)
      throw new InvalidOperationException($"Problem while getting mongo database instance for database name {databaseName}...");
  }
}
