using UpworkFeed.Core.Models;

namespace UpworkFeed.Rss;

public class FeedResponse<T>
{
    public IEnumerable<T> Items { get; set; }
    public string LastId { get; set; }
}

