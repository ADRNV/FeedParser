namespace FeedParser.Core
{
    public interface IUpdateHandler<T>
    {
        Task OnUpdate(T update);
    }
}
