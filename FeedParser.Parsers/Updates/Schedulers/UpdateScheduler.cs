﻿using FeedParser.Core;
using FeedParser.Core.Models;
using FeedParser.Core.Schedulers;
using FeedParser.Parsers.Updates.Schedulers.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Timers;
using Timer = System.Timers.Timer;

namespace FeedParser.Parsers.Updates.Schedulers
{
    public class UpdateScheduler : IScheduler, IHostedService
    {
        private IEnumerable<IParser> _parsers;

        private Timer _timer;

        private readonly IUpdateHandler<IEnumerable<Article>> _updateHandler;

        private readonly ILogger<UpdateScheduler> _logger;

        public UpdateScheduler(SchedulerOptions options, IEnumerable<IParser> parsers, IUpdateHandler<IEnumerable<Article>> updateHandler, ILogger<UpdateScheduler> logger)
        {
            _parsers = parsers;

            _timer = new Timer(options.UpdatesInterval);

            _updateHandler = updateHandler;

            _logger = logger;
        }

        public void Start()
        {
            _timer.Elapsed += Timer_Elapsed;

            _timer.Start();
        }

        public void Stop()
        {
            _timer.Stop();
        }

        private async void Timer_Elapsed(object? sender, ElapsedEventArgs e)
        {
            await FetchFeed();
        }

        private async Task<IEnumerable<Article>> FetchFeed()
        {
            var feed = new List<Article>();

            foreach (var parser in _parsers)
            {
                feed.AddRange(await parser.GetFeedLinks());
            }

            await _updateHandler.OnUpdate(feed);

            _logger?.LogInformation($"Take {feed.Count} updates");

            return feed;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            _timer.Elapsed += Timer_Elapsed;

            await Task.Run(() => _timer.Start(), cancellationToken);

            _logger?.LogCritical("Start service");
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _timer.Stop();

            return Task.CompletedTask;
        }
    }
}
