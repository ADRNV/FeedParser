using FeedParser.Core;
using FeedParser.Core.Models;
using FeedParser.Parsers;
using FeedParser.Parsers.Habr;
using FeedParser.Parsers.TProger;
using FeedParser.Parsers.Updates.Handlers;
using FeedParser.Parsers.Updates.Schedulers;
using FeedParser.Parsers.Updates.Schedulers.Configuration;
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
                configuration.ClearProviders();
                configuration.AddFilter("Microsoft.Hosting.Lifetime", LogLevel.Information);
                configuration.AddFilter(x => x == LogLevel.Information);
                configuration.AddConsole();
            });


            return hostBuilder;
        }

        public static IHostBuilder ConfigureParsers(this IHostBuilder hostBuilder)
        {
            hostBuilder.ConfigureServices(c =>
            {
                c.AddTransient<IParser, HabrParser>(c =>
                {
                    var habrParser = new ParserBuilder()
                        .FromHabr()
                        .PerTime(ArticlesFilters.PerTime.Day, (u, p) => new HabrUrlMapper().MapToString(u, p))
                        .Build();

                    return (HabrParser)habrParser;
                });

                c.AddTransient<IParser, TProgerParser>(c =>
                {
                    var tProgerParser = new ParserBuilder()
                        .FromTProger()
                        .Build();

                    return (TProgerParser)tProgerParser;
                });
            });

            return hostBuilder;
        }

        public static IHostBuilder ConfigureParser<T>(this IHostBuilder hostBuilder, ParserBuilder parserBuilder) where T: ParserBase
        {
            hostBuilder.ConfigureServices(c =>
            {
                c.AddTransient<IParser, T>(c =>
                {
                    return parserBuilder.Build() as T;
                });
            });

            return hostBuilder;
        }

        public static IHostBuilder ConfigureScheduler(this IHostBuilder hostBuilder)
        {
            hostBuilder.ConfigureServices(c =>
            {
                c.AddTransient<IUpdateHandler<IEnumerable<Article>>>(c =>
                {
                    var updateHandler = new UpdateHandler(c.GetServices<IParser>(),
                         c.GetService<ILogger<UpdateHandler>>());

                    return updateHandler;
                });

                c.AddSingleton<SchedulerOptions>(i => new SchedulerOptions
                {
                    UpdatesInterval = TimeSpan.FromSeconds(5)
                });

                c.AddHostedService<UpdateScheduler>();
            });

            return hostBuilder;
        }

        public static IHostBuilder ConfigureScheduler(this IHostBuilder hostBuilder, IUpdateHandler<IEnumerable<Article>> updateHandler, SchedulerOptions schedulerOptions)
        {
            hostBuilder.ConfigureServices(c =>
            {
                c.AddTransient<IUpdateHandler<IEnumerable<Article>>>(c => updateHandler);

                c.AddSingleton<SchedulerOptions>(i => schedulerOptions);

                c.AddHostedService<UpdateScheduler>();
            });

            return hostBuilder;
        }
    }
}
