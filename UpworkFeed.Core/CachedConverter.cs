namespace UpworkFeed.Core;

public class CachedConverter<TKey, TSrcItem, TDstItem, TConverter>
    : IConverter<TKey, TSrcItem, TDstItem>
        where TConverter : class, IConverter<TKey, TSrcItem, TDstItem>, new()
        where TKey : notnull
{
    private readonly Dictionary<TKey, TDstItem> _items = new();
    private readonly TConverter _converter = new();

    public TDstItem Convert(TSrcItem source)
    {
        var key = _converter.GetKey(source);
        if (!_items.ContainsKey(key))
            _items[key] = _converter.Convert(source);
        return _items[key];
    }

    // TODO: add cache clearing

    public TKey GetKey(TSrcItem source) => _converter.GetKey(source);
}
