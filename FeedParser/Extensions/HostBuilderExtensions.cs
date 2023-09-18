using FeedParser.Core;
using FeedParser.Core.Models;
using FeedParser.Parsers;
using FeedParser.Parsers.Habr;
using FeedParser.Parsers.TProger;
using FeedParser.Parsers.Updates.Handlers;
using FeedParser.Parsers.Updates.Schedulers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace FeedParser.Extensions
{
    public static class HostBuilderExtensions
    {
        public static IHostBuilder ConfigureLogging(this IHostBuilder hostBuilder)
        {
            hostBuilder.ConfigureLogging((_, configuration) =>
            {
                configuration.AddFilter("Microsoft.Hosting.Lifetime", LogLevel.Warning);
                configuration.AddFilter(x => x == LogLevel.Information);
                configuration.AddConsole();
            });

            return hostBuilder;
        }

        public static IHostBuilder ConfigureParsers(this IHostBuilder hostBuilder)
        {
            hostBuilder.ConfigureServices(c =>
            {
                c.AddSingleton<IParser, HabrParser>(c =>
                {
                    var habrParser = new ParserBuilder()
                        .FromHabr()
                        .PerTime(ArticlesFilters.PerTime.Day, (u, p) => new HabrUrlMapper().MapToString(u, p))
                        .Build();

                    return (HabrParser)habrParser;
                });

                c.AddSingleton<IParser, TProgerParser>(c =>
                {
                    var tProgerParser = new ParserBuilder()
                        .FromTProger()
                        .Build();

                    return (TProgerParser)tProgerParser;
                });
            });

            return hostBuilder;
        }

        public static IHostBuilder ConfigureScheduler(this IHostBuilder hostBuilder)
        {
            hostBuilder.ConfigureServices(c =>
            {
                c.AddScoped<IUpdateHandler<IEnumerable<Article>>>((s) => new UpdateHandler());

                c.AddHostedService<UpdateScheduler>(c =>
                {
                    var updateScheduler = new UpdateScheduler(TimeSpan.FromSeconds(5),
                        c.GetServices<IParser>(),
                        c.GetRequiredService<IUpdateHandler<IEnumerable<Article>>>()
                        );

                    return updateScheduler;
                });
            });

            return hostBuilder;
        }
    }
}
