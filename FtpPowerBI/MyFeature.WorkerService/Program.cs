using MyFeature.WorkerService;
using System.CommandLine;

var builder = Host.CreateApplicationBuilder(args);

// Command Line Options
var periodicOption = new Option<bool>(
    name: "--periodic", 
    description: "Define the periodic mode to use for this service",
    getDefaultValue: () => true);
var intervalOption = new Option<int>(
    name: "--interval",
    description: "Define the interval in seconds for the periodic mode",
    getDefaultValue: () => 60); // Default to 60 seconds if not specified

var rootCommand = new RootCommand("MyFeature Worker Service")
{
  periodicOption,
  intervalOption
};

// Set the handler for the root command
rootCommand.SetHandler(new Func<bool, int, Task>(async (periodic, interval) =>
{
  IHost? host = null;

  builder.AddServiceDefaults();

  if (periodic)
  {
    Console.WriteLine($"Periodic mode enabled with an interval of {interval} seconds.");
    builder.Services.AddHostedService<PeriodicWorker>();
    host = builder.Build();
    await host.RunAsync();
    return;
  }

  Console.WriteLine("Periodic mode disabled. The service will run once.");
  builder.Services.AddHostedService<OneTimeWorker>();
  host = builder.Build();
  await host.StartAsync();
  await host.StopAsync();
}), 
periodicOption, 
intervalOption);

// Parse the command line arguments and invoke the handler
await rootCommand.InvokeAsync(args);