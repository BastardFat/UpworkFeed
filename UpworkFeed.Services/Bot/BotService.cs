using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using UpworkFeed.Bot;
using UpworkFeed.Services.Bot.Routes;

namespace UpworkFeed.Services.Bot;

public class BotService : BackgroundService
{
    private readonly ILogger<BotService> _logger;
    private readonly IBot _bot;
    private readonly StartRoute _startRoute;

    public BotService(
        ILogger<BotService> logger,
        IBot bot,
        StartRoute startRoute)
    {
        _logger = logger;
        _bot = bot;
        _startRoute = startRoute;
    }

    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        _bot.Router.AddRoute(_startRoute);
        _bot.Start(cancellationToken);
        var info = await _bot.GetBotInfoAsync();
        _logger.LogInformation("Bot started ( Id:{Id}, Username:{Username}, LanguageCode:{LanguageCode} )",
            info["Id"], info["Username"], info["LanguageCode"]);
    }
}

/*
using MongoDB.Driver;
using UpworkFeed.Bot;
using UpworkFeed.Bot.Formatters;
using UpworkFeed.Core;
using UpworkFeed.Core.Models;
using UpworkFeed.Data.Services;
using UpworkFeed.Rss;

const string _connectionString = "mongodb+srv://app:ur1Vv8pWAYpYyDRR@upwork-feed-cluster.rew6uzt.mongodb.net/?retryWrites=true&w=majority";

var db = new MongoClient(_connectionString).GetDatabase("upwork-feed-bot"); ;
UserProvider userService = new(db);
FeedProvider feedService = new(db);
Bot bot = new Bot();
IBotFormatter<JobOffer> formatter = new MarkdownFormatter();

bot.Router.AddRoute((m, id) => Task.FromResult(m == "/start"), async (m, id) => await bot.SendMarkdownMessage(id, "_Hello_"));

bot.Router.AddRoute((m, id) => Task.FromResult(m == "/status"), async (m, id) =>
{
    var user = await userService.GetOrCreateUserAsync(id);
    await bot.SendTextMessage(id, $"Id:{id}");
    if(user.FeedId == null)
        await bot.SendTextMessage(id, $"No assigned feed");
    else
    {
        var feed = await feedService.GetFeedAsync(user.FeedId.Value);
        await bot.SendTextMessage(id, $"Filter:{feed.FeedFilter}");
        await bot.SendTextMessage(id, $"LastRequested:{feed.LastRequested}");
        await bot.SendTextMessage(id, $"FeedingTill:{user.FeedingTill}");
    }
});

bot.Router.AddRoute((m, id) => Task.FromResult(m == "/feed"), async (m, id) => {
    await userService.SetUserStateAsync(id, UserState.WaitingFilter);
    await bot.SendTextMessage(id, "Paste filter");
});

bot.Router.AddRoute(async (m, id) =>
{
    if (m.StartsWith("/"))
        return false;
    var user = await userService.GetOrCreateUserAsync(id);
    return user.State == UserState.WaitingFilter;
}
, async (m, id) =>
{
    if(UrlHelper.TryExtractFilterFromUrl(m, out string filter))
    {
        var feed = await feedService.GetOrCreateFeedAsync(filter);
        await userService.SetUserFeedAsync(id, feed.Id, TimeSpan.FromMinutes(15));
        await bot.SendTextMessage(id, "Filter successfully set");
    }
    else
    {
        await bot.SendTextMessage(id, "Invalid filter");
    }
});


bot.Start();

Console.WriteLine("Bot started!");

while (true)
{
    var feeds = await feedService.ListFeedsAsync(TimeSpan.FromMinutes(1));
    foreach (var feed in feeds)
    {
        var users = await userService.ListActiveUsersByFeedAsync(feed.Id);
        if (users.Count == 0)
        {
            await userService.RemoveFeedFromUsers(feed.Id);
            await feedService.DeleteFeed(feed.Id);
            Console.WriteLine($"Deleted feed {feed.Id}");
            continue;
        }

        Console.WriteLine($"Requesting feed {feed.Id}");
        var responce = await FeedRequest.PerformAsync(feed.FeedFilter, feed.LastKnownJobId);
        await feedService.MarkFeedAsRequestedAsync(feed.Id, responce.LastJobId);
        Console.WriteLine($"Received feed: {responce.JobOffers.Count()} entries to {users.Count} users");

        foreach (var user in users)
        {
            foreach (var job in responce.JobOffers)
            {
                try
                {
                    await bot.SendMarkdownMessage(user.Id, formatter.Format(job));
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Failed send message: " + ex.Message);
                    Console.WriteLine(ex.StackTrace);
                }
            }
        }
        await Task.Delay(TimeSpan.FromSeconds(5));
    }
    await Task.Delay(TimeSpan.FromSeconds(15));

}

Console.ReadLine();

 */
