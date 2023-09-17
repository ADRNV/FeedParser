namespace FeedParser.Parsers
{
    public class ParserBuilder
    {
        private ParserBase _parser;

        internal ParserBase Parser
        {
            get => _parser;

            set
            {
                _parser = value;
            }
        }

        public ParserBase Build()
        {
            return _parser;
        }
    }
}
