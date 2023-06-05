using UpworkFeed.Data.Entities;

namespace UpworkFeed.Data.Services;

public interface IFeedProvider
{
    Task<bool> DeleteFeed(Guid feedId);
    Task<Feed> GetFeedAsync(Guid feedId);
    Task<Feed> GetOrCreateFeedAsync(string feedFilter);
    Task<List<Feed>> ListFeedsAsync(TimeSpan period);
    Task<bool> MarkFeedAsRequestedAsync(Guid feedId, string lastKnownJobId);
}
