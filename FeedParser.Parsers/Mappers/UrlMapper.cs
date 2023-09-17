using FeedParser.Core.Models;

namespace FeedParser.Parsers.Mappers
{
    public abstract class UrlMapper
    {
        public abstract string MapToString(string url, ArticlesFilters.PerTime perTime);
    }
}
