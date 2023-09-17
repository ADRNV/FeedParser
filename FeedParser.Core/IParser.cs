using FeedParser.Core.Models;

namespace FeedParser.Core
{
    public interface IParser
    {
        string Url { get; }

        Task<IEnumerable<Article>> ParseRoot();

        Task<IEnumerable<Article>> GetFeedLinks();
    }
}
