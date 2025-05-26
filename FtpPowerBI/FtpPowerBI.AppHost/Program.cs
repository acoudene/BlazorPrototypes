using Aspire.Hosting;

var builder = DistributedApplication.CreateBuilder(args);

const string databaseName = "myfeature";

var mongoContainer = builder.AddMongoDB("mongo")
  .WithLifetime(ContainerLifetime.Persistent);

var mongoDatabase = mongoContainer.AddDatabase(databaseName);

builder.AddProject<Projects.MyFeature_Host>("myfeature-host")
  .WithReference(mongoDatabase)
  .WaitFor(mongoDatabase);

builder.AddProject<Projects.MyFeature_WebApp>("myfeature-webapp");


// FTP
// docker run --rm -d --name ftpserver -p 21:21 -p 30000-30009:30000-30009 stilliard/pure-ftpd bash /run.sh -c 30 -C 10 -l puredb:/etc/pure-ftpd/pureftpd.pdb -E -j -R -P localhost -p 30000:30059
// docker run --rm -d --name ftpserver -p 21:21 -p 30000-30009:30000-30009 -e FTP_USER_NAME=ftpuser -e FTP_USER_PASS=ftppassword -e FTP_USER_HOME=/home/ftpuser stilliard/pure-ftpd
builder.AddContainer("ftpserver", "stilliard/pure-ftpd:latest")
  .WithEndpoint(port: 21, targetPort: 21, name: "ftp")
  .WithEndpoint(port: 30000, targetPort: 30000, name: "ftp-passive-30000")
  .WithEndpoint(port: 30001, targetPort: 30001, name: "ftp-passive-30001")
  .WithEndpoint(port: 30002, targetPort: 30002, name: "ftp-passive-30002")
  .WithEndpoint(port: 30003, targetPort: 30003, name: "ftp-passive-30003")
  .WithEndpoint(port: 30004, targetPort: 30004, name: "ftp-passive-30004")
  .WithEndpoint(port: 30005, targetPort: 30005, name: "ftp-passive-30005")
  .WithEndpoint(port: 30006, targetPort: 30006, name: "ftp-passive-30006")
  .WithEndpoint(port: 30007, targetPort: 30007, name: "ftp-passive-30007")
  .WithEndpoint(port: 30008, targetPort: 30008, name: "ftp-passive-30008")
  .WithEndpoint(port: 30009, targetPort: 30009, name: "ftp-passive-30009")
  .WithEnvironment("FTP_USER_NAME", "ftpuser")
  .WithEnvironment("FTP_USER_PASS", "ftppassword")
  .WithEnvironment("FTP_USER_HOME", "/home/ftpuser");

/// dotnet tool install -g aspire.cli --prerelease
/// aspire publish
builder.AddDockerComposePublisher();

builder.Build().Run();
