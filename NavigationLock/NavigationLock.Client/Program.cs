using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

///
/// <see cref="https://mattjameschampion.com/2023/07/04/controlling-and-preventing-navigation-events-in-blazor-using-navigationlock/"/>
/// 


var builder = WebAssemblyHostBuilder.CreateDefault(args);

await builder.Build().RunAsync();

