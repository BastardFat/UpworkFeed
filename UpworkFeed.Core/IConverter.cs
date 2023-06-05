namespace UpworkFeed.Core;

public interface IConverter<TKey, TSrcItem, TDstItem>
{
    TDstItem Convert(TSrcItem source);
    TKey GetKey(TSrcItem source);
}
