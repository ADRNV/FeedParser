using FeedParser.Core;
using FeedParser.Core.Models;
using Newtonsoft.Json;

namespace FeedParser.Data.Serialization
{
    internal class JSONSerializer : ISerializer<IEnumerable<Article>>
    {
        private readonly JsonSerializer _serializer = new JsonSerializer();

        public object Deserialize(IEnumerable<Article> obj, TextReader textReader)
        {
            throw new NotImplementedException();
        }

        public string Serialize(IEnumerable<Article> obj, TextWriter textWriter)
        {
            _serializer.Serialize(textWriter, obj, typeof(IEnumerable<Article>));

            return JsonConvert.SerializeObject(obj);
        }


    }
}
