// Changelogs Date  | Author                | Description
// 2023-12-23       | Anthony Coudène       | Creation

namespace Core.Data.MongoDb;

public class DatabaseSettings
{
  public string ConnectionString { get; set; } = string.Empty;

  public string DatabaseName { get; set; } = string.Empty;
}