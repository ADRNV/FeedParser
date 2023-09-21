namespace FeedParser.Core.Models
{
    public record Article
    {
        public string Header { get; set; }

        public string Link { get; set; }

        public List<string> Content { get; set; } = new List<string>();
    }
}
