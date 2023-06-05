
namespace UpworkFeed.Bot
{
    public interface IBot
    {
        BotRouter Router { get; }

        Task<Dictionary<string, object?>> GetBotInfoAsync();
        Task SendHtmlMessage(long chatId, string message);
        Task SendMarkdownMessage(long chatId, string message);
        Task SendTextMessage(long chatId, string message);
        void Start(CancellationToken cancellationToken);
    }
}