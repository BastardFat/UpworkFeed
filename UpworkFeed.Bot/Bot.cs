using Microsoft.Extensions.Options;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using UpworkFeed.Core.Options;

namespace UpworkFeed.Bot;

public class Bot : IDisposable, IBot
{
    public BotRouter Router { get; } = new();

    private readonly TelegramBotClient _client;
    private readonly CancellationTokenSource _cts = new();
    private bool disposedValue;

    public Bot(IOptions<ApplicationOptions> _options)
    {
        _client = new TelegramBotClient(_options.Value.Bot.ApiKey);
    }

    public async Task<Dictionary<string, object?>> GetBotInfoAsync()
    {
        var user = await _client.GetMeAsync();
        return new()
        {
            { "Id", user.Id },
            { "Username", user.Username },
            { "LanguageCode", user.LanguageCode },
        };
    }

    public void Start(CancellationToken cancellationToken)
    {
        _client.StartReceiving(
            updateHandler: HandleUpdateAsync,
            pollingErrorHandler: HandlePollingErrorAsync,
            //receiverOptions: new ReceiverOptions() { AllowedUpdates = new[] { UpdateType.Message } },
            receiverOptions: new ReceiverOptions() { AllowedUpdates = Array.Empty<UpdateType>() },
            cancellationToken: cancellationToken);
    }

    public async Task SendMarkdownMessage(long chatId, string message) =>
        await _client.SendTextMessageAsync(chatId: chatId, text: message, parseMode: ParseMode.MarkdownV2);

    public async Task SendHtmlMessage(long chatId, string message) =>
        await _client.SendTextMessageAsync(chatId: chatId, text: message, parseMode: ParseMode.Html);

    public async Task SendTextMessage(long chatId, string message) =>
        await _client.SendTextMessageAsync(chatId: chatId, text: message);


    async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
    {
        if (update.Message is not { } message)
            return;
        if (message.Text is not { } messageText)
            return;

        var chatId = message.Chat.Id;

        await Router.ProcessMessage(messageText, chatId);
    }

    Task HandlePollingErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
    {
        var ErrorMessage = exception switch
        {
            ApiRequestException apiRequestException
                => $"Telegram API Error:\n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}",
            _ => exception.ToString()
        };

        Console.WriteLine(ErrorMessage);
        return Task.CompletedTask;
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!disposedValue)
        {
            if (disposing)
                _cts.Cancel();
            disposedValue = true;
        }
    }

    public void Dispose()
    {
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }
}