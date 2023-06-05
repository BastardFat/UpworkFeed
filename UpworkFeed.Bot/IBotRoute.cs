namespace UpworkFeed.Bot;

public interface IBotRoute
{
    Task<bool> IsApplicableAsync(string message, long userId);
    Task ApplyAsync(string message, long userId);
}
