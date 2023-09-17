using FeedParser.Core;
using Newtonsoft.Json;

namespace FeedParser.Serialization
{
    public class JsonSerialize : ISerializer
    {
        private JsonSerializer _jsonSerializer = new JsonSerializer();

        public object Deserialize(object obj)
        {
            throw new NotImplementedException();
        }

        public string Serialize(object obj)
        {
            using()
            _jsonSerializer.Serialize(,);
        }
    }
}
