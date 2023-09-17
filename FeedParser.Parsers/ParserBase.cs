using AngleSharp;
using FeedParser.Core;
using FeedParser.Core.Models;

namespace FeedParser.Parsers
{
    public abstract class ParserBase : IParser
    {
        protected IConfiguration _config { get; set; }

        protected readonly IBrowsingContext context;

        public ParserBase(IConfiguration configuration = null)
        {
            _config = Configuration.Default.WithDefaultLoader();

            context = BrowsingContext.New(_config);
        }

        public abstract string Url { get; set; }

        public abstract Task<IEnumerable<Article>> GetFeedLinks();

        public abstract Task<IEnumerable<Article>> ParseRoot();
    }
}
