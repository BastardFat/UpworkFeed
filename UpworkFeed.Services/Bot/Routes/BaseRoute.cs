using Microsoft.Extensions.Hosting;

namespace UpworkFeed.Services.Bot.Routes;

public abstract class BaseRoute
{
    private readonly IHostEnvironment _environment;

    protected BaseRoute(IHostEnvironment environment)
    {
        _environment = environment;
    }

    protected async Task<string> MarkdownTemplate(string templateName, Dictionary<string, string> inserts)
    {
        var path = Path.Combine(_environment.ContentRootPath, templateName + ".md");
        var text = await File.ReadAllTextAsync(path);

        foreach (var insert in inserts)
            text = text.Replace("{" + insert.Key + "}", insert.Value);

        return text;
    }
}
