namespace UpworkFeed.Core.Options;

public class DatabaseOptions
{
    public string ConnectionString { get; set; } = string.Empty;
    public string DbName { get; set; } = string.Empty;
    public string UserCollectionName { get; set; } = string.Empty;
    public string FeedCollectionName { get; set; } = string.Empty;
}