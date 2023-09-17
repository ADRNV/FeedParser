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

        private event Action<IEnumerable<Article>> OnFetched;

        public UpdateScheduler(TimeSpan updateInterval, IEnumerable<IParser> parsers, Action<IEnumerable<Article>> onFetched)
        {
            _parsers = parsers;

            _timer = new Timer(updateInterval);

            OnFetched = onFetched;
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

            OnFetched.Invoke(feed);

            return feed;
        }

        private async Task<Stream> WriteAsStream(IEnumerable<Article> articles)
        {
            var stream = new MemoryStream();

            var sw = new StreamWriter(stream, Encoding.UTF8);

            var articlesJson = JsonConvert.SerializeObject(articles);

            await sw.WriteLineAsync(articlesJson);

            return stream;
        }
    }
}
