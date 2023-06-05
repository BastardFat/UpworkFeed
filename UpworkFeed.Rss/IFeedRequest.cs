namespace UpworkFeed.Rss;

public interface IFeedRequest<T>
{
    Task<FeedResponse<T>> PerformAsync(string feedFilter, string? lastKnownId);
}
