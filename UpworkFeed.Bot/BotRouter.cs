namespace UpworkFeed.Bot;

public class BotRouter
{
    private readonly List<IBotRoute> _rules = new();
    private Func<string, long, Task>? _defaultAction;

    public void AddRoute(Func<string, long, Task<bool>> condition, Func<string, long, Task> action) =>
        _rules.Add(new BotRoute(condition, action));

    public void AddRoute(IBotRoute route) =>
        _rules.Add(route);

    public void SetDefaultRoute(Func<string, long, Task> defaultAction) =>
        _defaultAction = defaultAction;

    internal async Task ProcessMessage(string message, long chatId)
    {
        foreach (var rule in _rules)
        {
            if (await rule.IsApplicableAsync(message, chatId))
            {
                await rule.ApplyAsync(message, chatId);
                return;
            }
        }
        if (_defaultAction != null)
            await _defaultAction(message, chatId);
    }

}
