using AngleSharp;
using AngleSharp.Dom;
using FeedParser.Core.Models;
using System.Diagnostics;

namespace FeedParser.Parsers.Habr
{
    public class HabrParser : ParserBase
    {
        private string _url = "https://habr.com/ru/articles/top/";

        public HabrParser(IConfiguration configuration) : base(configuration)
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
            IDocument document = await context.OpenAsync(Url);

            var links = document.All.
                Where(c => c.ClassName == "tm-title__link")
                .Select(e => new Article
                {
                    Header = e.TextContent,
                    Link = e.GetAttribute("href")

                }).AsEnumerable();

            Debug.WriteLine(links.Count());

            return links;
        }

        public override Task<IEnumerable<Article>> ParseRoot()
        {
            throw new NotImplementedException();
        }
    }
}
