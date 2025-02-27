using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;

namespace LibWorkWithFiles;

public static class TelegramBot
{
    /// <summary>
    /// Токен бота
    /// </summary>
    private static readonly string botToken = "7661409563:AAE8_0rj5Ya3Lx_Kq-WImJ3GxFj0NQBAd_A"; 
    
    /// <summary>
    /// Клиент/пользователь бота
    /// </summary>
    private static TelegramBotClient botUser;
    
    /// <summary>
    /// ID пользователей
    /// </summary>
    private static List<long> subs = new List<long>(); 

    /// <summary>
    /// МетодБ запускающий работу бота
    /// </summary>
    public static async Task SendMessageAsync()
    {
        botUser = new TelegramBotClient(botToken);
        using CancellationTokenSource cts = new CancellationTokenSource();
        ReceiverOptions receiverOptions = new ReceiverOptions //получение доступных команд
        {
            AllowedUpdates = { } 
        };
        botUser.StartReceiving(GetMessageAsync, ErrorsAsync, receiverOptions, cts.Token);
        Console.WriteLine("Бот запущен. ...");
        await Task.Delay(-1); //бесконечное ожидание для непрерывной работы бота
    }

    /// <summary>
    /// Метод, обрабатывающий полученные сообщения
    /// </summary>
    /// <param name="botClient">Пользователь бота</param>
    /// <param name="update">Доступные обновления</param>
    /// <param name="cancellationToken">Токен для прерывания работы</param>
    private static async Task GetMessageAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
    {
        //проверка, существует ли команда, введённая пользователем
        if (update.Message == null || update.Message.Text == null)
        {
            return;
        }
        
        string messageText = update.Message.Text; //получения текста сообщения
        long chatId = update.Message.Chat.Id;
        
        if (messageText == "/start")
        {
            if (!subs.Contains(chatId))
            {
                subs.Add(chatId);
                await botClient.SendTextMessageAsync(chatId, "Привет! Теперь ты будешь получать уведомления о дедлайнах.", cancellationToken: cancellationToken);
            }
            else
            {
                await botClient.SendTextMessageAsync(chatId, "Ты уже подписан на уведомления!", cancellationToken: cancellationToken);
            }
        }

        if (messageText == "/stop")
        {
            if (subs.Contains(chatId))
            {
                subs.Remove(chatId);
                await botClient.SendTextMessageAsync(chatId, "Бот останавливает свою работу, ждём вашего возвращения!", cancellationToken: cancellationToken);
            }
        }
    }

    /// <summary>
    /// Обработчик ошибок работы бота
    /// </summary>
    /// <param name="botClient">Пользователь бота</param>
    /// <param name="exception">Исключение, которое отловилось</param>
    /// <param name="cancellationToken">Токен для прерывания работы</param>
    /// <returns>Возврат Task для соответствия сигнатуре</returns>
    private static Task ErrorsAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
    {
        Console.WriteLine($"Ошибка: {exception.Message}");
        return Task.CompletedTask;
    }

    /// <summary>
    /// Метод для оправки уведомлений пользователям
    /// </summary>
    /// <param name="message">Сообщение, которое нужно отправить</param>
    public static async Task NotificationAcync(string message)
    {
        foreach (long chatId in subs)
        {
            await botUser.SendTextMessageAsync(chatId, message);
        }
    }
}