var builder = DistributedApplication.CreateBuilder(args);

const string databaseName = "myfeature";

var mongoContainer = builder.AddMongoDB("mongo")
  .WithLifetime(ContainerLifetime.Persistent);

var mongoDatabase = mongoContainer.AddDatabase(databaseName);

builder.AddProject<Projects.MyFeature_Host>("myfeature-host")
  .WithReference(mongoDatabase)
  .WaitFor(mongoDatabase);

builder.AddProject<Projects.MyFeature_WebApp>("myfeature-webapp");

/// dotnet tool install -g aspire.cli --prerelease
/// aspire publish
builder.AddDockerComposePublisher();

builder.Build().Run();
