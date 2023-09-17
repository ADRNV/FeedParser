namespace FeedParser.Core
{
    public interface ISerializer
    {
        public string Serialize(object obj);

        public object Deserialize(object obj);
    }
}
