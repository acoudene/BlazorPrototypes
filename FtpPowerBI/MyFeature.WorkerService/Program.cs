using FluentFTP;
using MyFeature.Proxies;
using MyFeature.Proxies.Ftp;
using MyFeature.WorkerService;
using System.CommandLine;

var builder = Host.CreateApplicationBuilder(args);

builder.Configuration
  .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
  .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true, reloadOnChange: true)
  .AddCommandLine(args);

///
/// Command Line Options
/// Example: dotnet run --periodic true --TimerOptions:Period 00:02:00
/// 
var periodicOption = new Option<bool>(
    name: "--periodic",
    description: "Define the periodic mode to use for this service (ex: \"--periodic true\" to activate a periodic worker else one shot)",
    getDefaultValue: () => false);
var periodOption = new Option<TimeSpan>(
    name: "--TimerOptions:Period",
    description: "Define the period in TimeSpan for the periodic mode (ex: \"--TimerOptions:Period 00:02:00\" for 2 minutes)"); // Default to 60 seconds if not specified

var rootCommand = new RootCommand("MyFeature Worker Service")
{
  periodicOption,
  periodOption
};

///
/// Add services to the container.
///
builder.AddServiceDefaults();
builder.Services.AddSingleton<ExporterProvider>();
builder.Services.Configure<TimerOptions>(
    builder.Configuration.GetSection(key: nameof(TimerOptions)));

const string myEntityApiBaseAddressKey = "MyEntity_API_BASEADDRESS";
string myEntityApiBaseAddress = builder.Configuration[myEntityApiBaseAddressKey] ?? string.Empty;
if (string.IsNullOrWhiteSpace(myEntityApiBaseAddress))
  throw new InvalidOperationException($"Missing value for configuration key: {myEntityApiBaseAddressKey}");
builder.Services.AddHttpClient(HttpMyEntityClient.ConfigurationName, client => client.BaseAddress = new Uri(myEntityApiBaseAddress));
builder.Services.AddSingleton<IMyEntityClient, HttpMyEntityClient>();
builder.Services.AddSingleton<IFtpProxyClient, FtpProxyClient>();
builder.Services.AddSingleton(provider => new FtpClient("localhost", "ftpuser", "ftppassword"))
    .AddSingleton<IFtpProxyClient, FtpProxyClient>();

///
/// Set the handler for the root command
/// 
rootCommand.SetHandler(new Func<bool, TimeSpan, Task>(async (periodic, period) =>
{
  using var cancellationTokenSource = new CancellationTokenSource();
  Func<CancellationTokenSource, Task> runActionAsync = async cts =>
  {
    IHost host = builder.Build();
    await host.RunAsync(cts.Token);
  };

  if (periodic)
  {
    Console.WriteLine($"Periodic mode enabled");
    builder.Services.AddHostedService<PeriodicWorker>();
    await runActionAsync(cancellationTokenSource);
    return;
  }

  Console.WriteLine("Periodic mode disabled. The service will run once.");
  builder.Services.AddHostedService<OneTimeWorker>(provider =>
  new OneTimeWorker(
    provider.GetRequiredService<ILogger<OneTimeWorker>>(),
    provider.GetRequiredService<ExporterProvider>(),
    cancellationTokenSource
  ));
  await runActionAsync(cancellationTokenSource);
}),
periodicOption,
periodOption);

///
/// Parse the command line arguments and invoke the handler
/// 
await rootCommand.InvokeAsync(args);
