using AngleSharp;
using AngleSharp.Dom;
using FeedParser.Core.Models;
using Microsoft.Extensions.Logging;

namespace FeedParser.Parsers.Habr
{
    public class HabrParser : ParserBase
    {
        private string _url = "https://habr.com/ru/articles/top/";

        private readonly ILogger<HabrParser> _logger;

        public HabrParser(IConfiguration configuration, ILogger<HabrParser> logger = null) : base(configuration)
        {
            _logger = logger;
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

            return links;
        }

        public override async IAsyncEnumerable<Article> ParseRoot(IEnumerable<Article> articles)
        {

            foreach (var article in articles)
            {
                IDocument document = await context.OpenAsync("https://habr.com/" + article.Link);

                var readynArticle = await ParseArticleContent(document, article);

                yield return readynArticle;
            }
        }

        private Task<Article> ParseArticleContent(IDocument document, Article article)
        {
            var dirtyContent = document.All
                .Where(c => c.ClassName == "tm-article-body")
                .Where(c => c.Id == "post-content-body")
                .Select(c => c.OuterHtml);

            dirtyContent.ToList()
                .ForEach(s =>
                {
                    article.Content.Add(s);
                });

            _logger?.LogInformation($"Find text {dirtyContent.Count()} parts");

            return Task.FromResult(article);
        }
    }
}
