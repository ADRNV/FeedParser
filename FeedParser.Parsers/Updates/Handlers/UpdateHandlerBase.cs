using FeedParser.Core;

namespace FeedParser.Parsers.Updates.Handlers
{
    public abstract class UpdateHandlerBase<T> : IUpdateHandler<T>
    {
        public abstract Task OnUpdate(T update);
    }
}
