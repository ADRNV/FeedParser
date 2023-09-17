using AngleSharp;
using FeedParser.Core.Models;

namespace FeedParser.Parsers.TProger
{
    public class TProgerParser : ParserBase
    {
        private string _url = "https://tproger.ru/";

        public TProgerParser(IConfiguration configuration) : base(configuration)
        {
            
        }

        public override string Url 
        {
            get => _url;

            set
            {
                _url = value;
            }
        }

        public override async Task<IEnumerable<Article>> GetFeedLinks()
        {
            var document = await context.OpenAsync(Url);

            var articles = document.GetElementsByClassName("tp-post-card__link")
                .Select(c => new Article { 
                    Header = c.TextContent, 
                    Link = c.GetAttribute("href")!
                }).AsEnumerable();

            return articles;
        }

        public override Task<IEnumerable<Article>> ParseRoot()
        {
            throw new NotImplementedException();
        }
    }
}
