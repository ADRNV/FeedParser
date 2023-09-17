using FeedParser.Core;
using FeedParser.Core.Models;
using FeedParser.Parsers;
using FeedParser.Parsers.Habr;
using FeedParser.Parsers.Schedulers;

IParser habrParser = new ParserBuilder()
    .FromHabr()
    .PerTime(ArticlesFilters.PerTime.Day, (u, p) => new HabrUrlMapper().MapToString(u, p))
    .Build();

IParser tprogerParser = new ParserBuilder()
    .FromTProger()
    .Build();

var parsers = new List<IParser>() { habrParser, tprogerParser };

var scheduler = new UpdateScheduler(TimeSpan.FromSeconds(5), parsers.AsEnumerable(), OnFetched);

scheduler.Start();

void OnFetched(IEnumerable<Article> articles)
{
    foreach (var article in articles)
    {
        Console.WriteLine(article);
    }
}

Console.ReadLine();