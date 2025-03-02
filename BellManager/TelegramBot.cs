using System;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using System.Windows.Forms;

namespace BellManager
{
    public class TelegramBot
    {
        private readonly ITelegramBotClient _botClient;
        private bool _isForm1Open = false; 
        private MainForm _form1;
        private CancellationTokenSource _cancellationTokenSource;

        public TelegramBot(string token)
        {
            _botClient = new TelegramBotClient(token);
            _cancellationTokenSource = new CancellationTokenSource();
        }

        public async Task StartAsync()
        {
            var me = await _botClient.GetMeAsync();

            // Настройка обработчика обновлений
            var receiverOptions = new ReceiverOptions
            {
                AllowedUpdates = Array.Empty<UpdateType>() // Получать все типы обновлений
            };

            // Запуск обработки обновлений
            _botClient.StartReceiving(
                updateHandler: HandleUpdateAsync,
                errorHandler: HandlePollingErrorAsync,
                receiverOptions: receiverOptions,
                cancellationToken: _cancellationTokenSource.Token
            );
        }

        private async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            if (update.Type == UpdateType.Message && update.Message?.Type == MessageType.Text)
            {
                var chatId = update.Message.Chat.Id;
                var text = update.Message.Text;

                if (text == "/start")
                {
                    var replyMarkup = new ReplyKeyboardMarkup(new[]
                    {
                new KeyboardButton(_isForm1Open ? "Выключить" : "Включить")
            })
                    {
                        ResizeKeyboard = true,
                        OneTimeKeyboard = true
                    };

                    await botClient.SendTextMessageAsync(
                        chatId: chatId,
                        text: "Выберите действие:",
                        replyMarkup: replyMarkup,
                        cancellationToken: cancellationToken);
                }
                else if (text == "Включить" || text == "Выключить")
                {
                    if (text == "Включить" && !_isForm1Open)
                    {
                        OpenMainForm(); // Запускаем через метод
                        _isForm1Open = true;

                        await botClient.SendTextMessageAsync(
                            chatId: chatId,
                            text: "Менеджер Запущен",
                            replyMarkup: new ReplyKeyboardMarkup(new[]
                            {
                        new KeyboardButton("Выключить")
                            })
                            {
                                ResizeKeyboard = true,
                                OneTimeKeyboard = true
                            },
                            cancellationToken: cancellationToken);
                    }
                    else if (text == "Выключить" && _isForm1Open)
                    {
                        CloseMainForm(); // Закрываем через метод
                        _isForm1Open = false;

                        await botClient.SendTextMessageAsync(
                            chatId: chatId,
                            text: "Менеджер выключен.",
                            replyMarkup: new ReplyKeyboardMarkup(new[]
                            {
                        new KeyboardButton("Включить")
                            })
                            {
                                ResizeKeyboard = true,
                                OneTimeKeyboard = true
                            },
                            cancellationToken: cancellationToken);
                    }
                }
            }
        }
        /// <summary>
        /// Используем Invoke для управления UI из потока бота, а также передаем управление новому MainForm(основной поток)
        /// </summary>
        private void OpenMainForm()
        {
            if (Application.OpenForms["MainForm"] == null)
            {
                Application.OpenForms[0].Invoke(new Action(() =>
                {
                    _form1 = new MainForm();
                    _form1.Show();
                }));
            }
        }
        /// <summary>
        /// Используем Invoke для управления UI из потока бота, а также передаем управление новому MainForm(Основной поток)
        /// </summary>
        private void CloseMainForm()
        {
            if (_form1 != null && !_form1.IsDisposed)
            {
                Application.OpenForms[0].Invoke(new Action(() =>
                {
                    _form1.SafeClose();
                    _form1 = null;
                }));
            }
        }
        /// <summary>
        /// Отлов ошибок
        /// </summary>
        /// <param name="botClient"></param>
        /// <param name="exception"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        private Task HandlePollingErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
        {
            var errorMessage = exception switch
            {
                ApiRequestException apiRequestException
                    => $"Ошибка API Telegram: {apiRequestException.ErrorCode}\n{apiRequestException.Message}",
                _ => exception.ToString()
            };

            Console.WriteLine(errorMessage);
            return Task.CompletedTask;
        }

        public void Stop()
        {
            _cancellationTokenSource.Cancel();
        }
    }
}