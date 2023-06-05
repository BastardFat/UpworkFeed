namespace UpworkFeed.Bot;

internal class BotRoute : IBotRoute
{
    public Func<string, long, Task<bool>> Condition { get; set; }
    public Func<string, long, Task> Action { get; set; }

    public BotRoute(Func<string, long, Task<bool>> condition, Func<string, long, Task> action)
    {
        Condition = condition;
        Action = action;
    }

    public Task<bool> IsApplicableAsync(string message, long userId) => Condition(message, userId);

    public Task ApplyAsync(string message, long userId) => Action(message, userId);
}
