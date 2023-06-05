using UpworkFeed.Core.Models;

namespace UpworkFeed.Data.Entities;

public class User
{
    public long Id { get; set; }
    public UserState State { get; set; }

    public Guid? FeedId { get; set; }
    public DateTime FeedingTill { get; set; }
}

