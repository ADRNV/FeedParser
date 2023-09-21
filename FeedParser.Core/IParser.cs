using FeedParser.Core.Models;

namespace FeedParser.Core
{
    public interface IParser
    {
        string Url { get; }

        IAsyncEnumerable<Article> ParseRoot(IEnumerable<Article> articles);

        Task<IEnumerable<Article>> GetFeedLinks();
    }
}
