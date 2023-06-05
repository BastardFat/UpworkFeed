using CodeHollow.FeedReader;
using UpworkFeed.Core;
using UpworkFeed.Core.Models;

namespace UpworkFeed.Rss;

public class FeedRequest<T> : IFeedRequest<T>
{
    private readonly IConverter<string, FeedItem, T> _converter;
    private readonly IUrlHelper _urlHelper;

    public FeedRequest(IConverter<string, FeedItem, T> converter, IUrlHelper urlHelper)
    {
        _converter = converter;
        _urlHelper = urlHelper;
    }

    public async Task<FeedResponse<T>> PerformAsync(string feedFilter, string? lastKnownId)
    {
        var url = _urlHelper.GetUrlFromFilter(feedFilter);
        var feed = await FeedReader.ReadAsync(url);

        var lastId = feed.Items.First().Id;

        if (lastKnownId == null)
            return new FeedResponse<T> { Items = new T[0], LastId = lastId };

        var offers = feed.Items
            .TakeWhile(i => i.Id != lastKnownId)
            .Select(_converter.Convert)
            .ToArray();

        return new FeedResponse<T> { Items = offers, LastId = lastId };
    }
}

