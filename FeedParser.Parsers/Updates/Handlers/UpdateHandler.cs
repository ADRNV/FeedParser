using FeedParser.Core;
using FeedParser.Core.Models;
using FeedParser.Parsers.Habr;
using Microsoft.Extensions.Logging;

namespace FeedParser.Parsers.Updates.Handlers
{
    public class UpdateHandler : IUpdateHandler<IEnumerable<Article>>
    {
        private IEnumerable<IParser> _parsers;

        private readonly ILogger<UpdateHandler> _logger;

        public UpdateHandler(IEnumerable<IParser> parsers, ILogger<UpdateHandler> logger)
        {
            _parsers = parsers;

            _logger = logger;
        }

        public async Task OnUpdate(IEnumerable<Article> update)
        {
            var habrsParser = _parsers.OfType<HabrParser>()
                .Single();

            var article = habrsParser.ParseRoot(update);

            await foreach (var a in article)
            {
                _logger.LogInformation($"Article from {a.Link}:{a.Header}");

                foreach (var c in a.Content)
                {
                    _logger.LogInformation($"Content:{c}");
                }
            }
        }
    }
}
