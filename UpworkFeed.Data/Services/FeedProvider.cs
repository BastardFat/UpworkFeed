using MongoDB.Driver;
using UpworkFeed.Data.Entities;

namespace UpworkFeed.Data.Services;

public class FeedProvider : BaseProvider<Feed>, IFeedProvider
{
    public FeedProvider(IMongoDatabase db, string collectionName) 
        : base(db, collectionName) { }

    public async Task<Feed> GetOrCreateFeedAsync(string feedFilter)
    {
        var feed = await Collection
            .Find(x => x.FeedFilter == feedFilter)
            .FirstOrDefaultAsync();

        if (feed != null)
            return feed;

        feed = new Feed
        {
            Id = Guid.NewGuid(),
            FeedFilter = feedFilter,
            LastRequested = DateTime.Now,
            LastKnownJobId = null
        };

        await Collection.InsertOneAsync(feed);
        return feed;
    }

    public async Task<Feed> GetFeedAsync(Guid feedId)
    {
        var feed = await Collection
            .Find(x => x.Id == feedId)
            .FirstOrDefaultAsync();

        return feed;
    }

    public async Task<List<Feed>> ListFeedsAsync(TimeSpan period)
    {
        var time = DateTime.Now - period;
        return await Collection
            .Find(x => x.LastRequested < time)
            .ToListAsync();
    }

    public async Task<bool> MarkFeedAsRequestedAsync(Guid feedId, string lastKnownJobId)
    {
        var update = Update
            .Set(x => x.LastRequested, DateTime.Now)
            .Set(x => x.LastKnownJobId, lastKnownJobId);

        var result = await Collection
            .UpdateOneAsync(x => x.Id == feedId, update);

        return result.ModifiedCount == 1;
    }

    public async Task<bool> DeleteFeed(Guid feedId)
    {
        var result = await Collection
            .DeleteOneAsync(x => x.Id == feedId);

        return result.DeletedCount == 1;
    }
}