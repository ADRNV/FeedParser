using AngleSharp;
using FeedParser.Core.Models;
using FeedParser.Parsers.Habr;
using FeedParser.Parsers.TProger;

namespace FeedParser.Parsers
{
    public static class ParserBuilderExtensions
    {
        public static ParserBuilder FromHabr(this ParserBuilder parserBuilder, IConfiguration configuration = null)
        {
            parserBuilder.Parser = new HabrParser(configuration);

            return parserBuilder;
        }

        public static ParserBuilder FromTProger(this ParserBuilder parserBuilder, IConfiguration configuration = null)
        {
            parserBuilder.Parser = new TProgerParser(configuration);

            return parserBuilder;
        }
        public static ParserBuilder WithUrl(this ParserBuilder parserBuilder, string url)
        {
            parserBuilder.Parser.Url = url;

            return parserBuilder;
        }

        public static ParserBuilder PerTime(this ParserBuilder parserBuilder, ArticlesFilters.PerTime perTime, Func<string, ArticlesFilters.PerTime, string> urlMapper)
        {
            var url = parserBuilder.Parser.Url;

            parserBuilder.Parser.Url = urlMapper.Invoke(url, perTime);

            return parserBuilder;
        }
    }
}
