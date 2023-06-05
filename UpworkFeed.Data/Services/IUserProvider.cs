using UpworkFeed.Core.Models;
using UpworkFeed.Data.Entities;

namespace UpworkFeed.Data.Services;

public interface IUserProvider
{
    Task<User> GetOrCreateUserAsync(long userId);
    Task<List<User>> ListActiveUsersByFeedAsync(Guid feedId);
    Task RemoveFeedFromUsers(Guid feedId);
    Task<bool> SetUserFeedAsync(long userId, Guid feedId, TimeSpan feedingDuration);
    Task<bool> SetUserStateAsync(long userId, UserState state);
}
