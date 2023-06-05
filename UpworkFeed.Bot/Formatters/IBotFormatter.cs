namespace UpworkFeed.Bot.Formatters;

public interface IBotFormatter<T>
{
    string Format(T obj);
}
