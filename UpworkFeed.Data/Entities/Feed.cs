namespace UpworkFeed.Data.Entities;

public class Feed
{
    public Guid Id { get; set; }
    public string FeedFilter { get; set; }
    public DateTime LastRequested { get; set; }
    public string? LastKnownJobId { get; set; }
}
