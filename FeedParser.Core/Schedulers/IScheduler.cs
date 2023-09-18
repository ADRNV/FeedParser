using FeedParser.Core.Models;

namespace FeedParser.Core.Schedulers
{
    public interface IScheduler
    {
        void Start();

        void Stop();
    }
}
