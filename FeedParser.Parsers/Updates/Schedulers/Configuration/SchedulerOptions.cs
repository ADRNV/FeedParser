using Microsoft.Extensions.Options;

namespace FeedParser.Parsers.Updates.Schedulers.Configuration
{
    public class SchedulerOptions
    {
        public TimeSpan UpdatesInterval { get; set; }
    }
}
