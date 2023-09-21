using FeedParser.Extensions;
using Microsoft.Extensions.Hosting;

var host = Host.CreateDefaultBuilder()
     .ConfigureLogging()
     .ConfigureParsers()
     .ConfigureScheduler();

await host.RunConsoleAsync();

host.Start();
