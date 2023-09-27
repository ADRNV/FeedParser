using AngleSharp;
using AngleSharp.Dom;
using FeedParser.Core.Models;
using Microsoft.Extensions.Logging;
using System.Linq;

namespace FeedParser.Parsers.TProger
{
    public class TProgerParser : ParserBase
    {
        private string _url = "https://tproger.ru/";

        private readonly ILogger _logger;

        public TProgerParser(IConfiguration configuration, ILogger<TProgerParser> logger = null) : base(configuration)
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
            var document = await context.OpenAsync(Url);

            var articles = document.GetElementsByClassName("tp-post-card__link")
                .Select(c => new Article
                {
                    Header = c.TextContent,
                    Link = c.GetAttribute("href")!
                }).AsEnumerable();

            return articles;
        }

        public async override IAsyncEnumerable<Article> ParseRoot(IEnumerable<Article> articles)
        {
            foreach (var article in articles)
            {
                IDocument document = await context.OpenAsync("https://tproger.ru/" + article.Link);

                var readynArticle = await ParseArticleContent(document, article);

                yield return readynArticle;
            }
        }

        private Task<Article> ParseArticleContent(IDocument document, Article article)
        {
            var dirtyContent = document.All
                .Where(c => c.ClassName == "tp-post-page__wrapper")
                .Select(c => c.TextContent);

            dirtyContent.ToList()
                .ForEach(c =>
                {
                    article.Content.Add(c);
                });

            _logger?.LogInformation($"Find text {dirtyContent.Count()} parts");

            return Task.FromResult(article);
        }
    }
}
