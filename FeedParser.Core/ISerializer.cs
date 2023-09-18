namespace FeedParser.Core
{
    public interface ISerializer<T>
    {
        public string Serialize(T obj, TextWriter textWriter);

        public object Deserialize(T obj, TextReader textReader);
    }
}
