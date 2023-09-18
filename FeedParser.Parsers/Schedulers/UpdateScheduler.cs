using FeedParser.Core;
using FeedParser.Core.Models;
using Newtonsoft.Json;
using System.Text;
using System.Timers;
using Timer = System.Timers.Timer;

namespace FeedParser.Parsers.Schedulers
{
    public class UpdateScheduler
    {
        private IEnumerable<IParser> _parsers;

        private Timer _timer;

        private readonly IUpdateHandler<IEnumerable<Article>> _updateHandler;

        public UpdateScheduler(TimeSpan updateInterval, IEnumerable<IParser> parsers, IUpdateHandler<IEnumerable<Article>> updateHandler)
        {
            _parsers = parsers;

            _timer = new Timer(updateInterval);

            _updateHandler = updateHandler;

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

            _updateHandler.OnUpdate(feed);

            return feed;
        }
    }
}
