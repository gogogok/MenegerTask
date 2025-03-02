using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;

namespace LibWorkWithFiles
{

    public static class TelegramBot
    {
        /// <summary>
        /// Токен бота
        /// </summary>
        private static readonly string BotToken = "Nonono mistr fish";

        /// <summary>
        /// Клиент/пользователь бота
        /// </summary>
        private static TelegramBotClient _botUser;

        /// <summary>
        /// ID пользователей
        /// </summary>
        private static List<long> _subs = new List<long>();

        /// <summary>
        /// МетодБ запускающий работу бота
        /// </summary>
        public static async Task SendMessageAsync()
        {
            _botUser = new TelegramBotClient(BotToken);
            using CancellationTokenSource cts = new CancellationTokenSource();
            ReceiverOptions receiverOptions = new ReceiverOptions //получение доступных команд
            {
                AllowedUpdates = { }
            };
            _botUser.StartReceiving(GetMessageAsync, ErrorsAsync, receiverOptions, cts.Token);
            Console.WriteLine("Бот запущен. ...");
            await Task.Delay(-1); //бесконечное ожидание для непрерывной работы бота
        }

        /// <summary>
        /// Метод, обрабатывающий полученные сообщения
        /// </summary>
        /// <param name="botClient">Пользователь бота</param>
        /// <param name="update">Доступные обновления</param>
        /// <param name="cancellationToken">Токен для прерывания работы</param>
        private static async Task GetMessageAsync(ITelegramBotClient botClient, Update update,
            CancellationToken cancellationToken)
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
                if (!_subs.Contains(chatId))
                {
                    _subs.Add(chatId);
                    await botClient.SendTextMessageAsync(chatId,
                        "Привет! Теперь ты будешь получать уведомления о дедлайнах.",
                        cancellationToken: cancellationToken);
                }
                else
                {
                    await botClient.SendTextMessageAsync(chatId, "Ты уже подписан на уведомления!",
                        cancellationToken: cancellationToken);
                }
            }

            if (messageText == "/stop")
            {
                if (_subs.Contains(chatId))
                {
                    _subs.Remove(chatId);
                    await botClient.SendTextMessageAsync(chatId,
                        "Бот останавливает свою работу, ждём вашего возвращения!",
                        cancellationToken: cancellationToken);
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
        private static Task ErrorsAsync(ITelegramBotClient botClient, Exception exception,
            CancellationToken cancellationToken)
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
            foreach (long chatId in _subs)
            {
                await _botUser.SendTextMessageAsync(chatId, message);
            }
        }
    }
}