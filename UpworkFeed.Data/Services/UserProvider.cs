using MongoDB.Driver;
using UpworkFeed.Core.Models;
using UpworkFeed.Data.Entities;

namespace UpworkFeed.Data.Services;

public class UserProvider : BaseProvider<User>, IUserProvider
{
    public UserProvider(IMongoDatabase db, string collectionName) 
        : base(db, collectionName) { }

    public async Task<User> GetOrCreateUserAsync(long userId)
    {
        var user = await Collection
            .Find(x => x.Id == userId)
            .FirstOrDefaultAsync();

        if (user != null)
            return user;

        user = new User
        {
            Id = userId,
            State = UserState.Idle,
            FeedId = null
        };

        await Collection.InsertOneAsync(user);
        return user;
    }

    public async Task<List<User>> ListActiveUsersByFeedAsync(Guid feedId)
    {
        return await Collection
            .Find(x => x.FeedId == feedId && x.FeedingTill > DateTime.Now)
            .ToListAsync();
    }

    public async Task<bool> SetUserStateAsync(long userId, UserState state)
    {
        var update = Update
            .Set(x => x.State, state);

        var result = await Collection
            .UpdateOneAsync(x => x.Id == userId, update);

        return result.ModifiedCount == 1;
    }

    public async Task<bool> SetUserFeedAsync(long userId, Guid feedId, TimeSpan feedingDuration)
    {
        var update = Update
            .Set(x => x.FeedId, feedId)
            .Set(x => x.FeedingTill, DateTime.Now + feedingDuration)
            .Set(x => x.State, UserState.Idle);

        var result = await Collection
            .UpdateOneAsync(x => x.Id == userId, update);

        return result.ModifiedCount == 1;
    }

    public async Task RemoveFeedFromUsers(Guid feedId)
    {
        var update = Update
            .Set(x => x.FeedId, null);

        await Collection
            .UpdateManyAsync(x => x.FeedId == feedId, update);
    }
}