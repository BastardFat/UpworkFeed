namespace UpworkFeed.Core;

public class UrlHelper : IUrlHelper
{
    private const string baseUrl = "https://www.upwork.com/ab/feed/jobs/rss?";
    public bool TryExtractFilterFromUrl(string url, out string filter)
    {
        filter = null;
        if (string.IsNullOrEmpty(url))
            return false;
        if (!url.StartsWith(baseUrl))
            return false;
        filter = url.Replace(baseUrl, string.Empty);
        return true;
    }

    public string GetUrlFromFilter(string filter)
    {
        return baseUrl + filter;
    }
}
