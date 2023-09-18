using Microsoft.Extensions.Hosting;
using FeedParser.Extensions;

var host = Host.CreateDefaultBuilder()
     .ConfigureLogging()
     .ConfigureParsers()
     .ConfigureScheduler();

host.Start();
