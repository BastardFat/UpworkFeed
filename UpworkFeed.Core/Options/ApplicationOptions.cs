namespace UpworkFeed.Core.Options;
public class ApplicationOptions
{
    public const string OptionName = "ApplicationOptions";

    public DatabaseOptions Database { get; set; } = new();
    public BotOptions Bot { get; set; } = new();
}
