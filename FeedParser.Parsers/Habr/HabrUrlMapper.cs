using FeedParser.Core.Models;
using FeedParser.Parsers.Mappers;

namespace FeedParser.Parsers.Habr
{
    public class HabrUrlMapper : UrlMapper
    {
        public override string MapToString(string url, ArticlesFilters.PerTime perTime)
        {
            switch (perTime)
            {
                case ArticlesFilters.PerTime.Day:
                    return url + "daily/";
                case ArticlesFilters.PerTime.Weak:
                    return url + "weekly/";
                case ArticlesFilters.PerTime.Mounth:
                    return url + "monthly/";
                case ArticlesFilters.PerTime.Year:
                    return url + "yearly";
                default:
                    throw new InvalidOperationException();
            }
        }
    }
}
