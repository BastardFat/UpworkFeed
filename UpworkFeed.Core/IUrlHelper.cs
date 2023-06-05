namespace UpworkFeed.Core;

public interface IUrlHelper
{
    string GetUrlFromFilter(string filter);
    bool TryExtractFilterFromUrl(string url, out string filter);
}
