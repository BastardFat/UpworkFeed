using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UpworkFeed.Bot;

namespace UpworkFeed.Services.Bot.Routes;

public class StartRoute : BaseRoute, IBotRoute
{
    private readonly IBot _bot;

    public StartRoute(IBot bot, 
        IHostEnvironment environment) : base(environment)
    {
        _bot = bot;
    }

    public Task<bool> IsApplicableAsync(string message, long userId) => 
        Task.FromResult(message == "/start");

    public async Task ApplyAsync(string message, long userId)
    {
        string md = await MarkdownTemplate("Start", new()
        {
            { "inserthere", "Love" },
            { "chatid", userId.ToString() }
        });
        await _bot.SendMarkdownMessage(userId, md);
    }
}
