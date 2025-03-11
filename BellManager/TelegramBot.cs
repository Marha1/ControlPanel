using Microsoft.VisualBasic.Devices;
using System.Reflection.Metadata;
using System.Windows.Forms;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace BellManager;

public class TelegramBot
{
    private readonly ITelegramBotClient _botClient;
    private readonly CancellationTokenSource _cancellationTokenSource;
    private readonly BellManagerService _bellManagerService;
    private MainForm _form1;
    private bool _isForm1Open;

    public TelegramBot(string token)
    {
        _botClient = new TelegramBotClient(token);
        _cancellationTokenSource = new CancellationTokenSource();
        _bellManagerService = new BellManagerService();
    }

    public async Task StartAsync()
    {
        var me = await _botClient.GetMeAsync();

        // Настройка обработчика обновлений
        var receiverOptions = new ReceiverOptions
        {
            AllowedUpdates = Array.Empty<UpdateType>()
        };

        // Запуск обработки обновлений
        _botClient.StartReceiving(
            HandleUpdateAsync,
            HandlePollingErrorAsync,
            receiverOptions,
            _cancellationTokenSource.Token
        );
    }

    private async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update,
        CancellationToken cancellationToken)
    {
        if (update.Type == UpdateType.Message && update.Message?.Type == MessageType.Text)
        {
            var chatId = update.Message.Chat.Id;
            var text = update.Message.Text;

            var action = update.Message.Text;
            var response = action switch
            {
                "/start" => HandleStartCommandAsync(botClient, chatId, cancellationToken),
                "Включить" => HandleTurnOnCommandAsync(botClient, chatId, cancellationToken),
                "Выключить" => HandleTurnOffCommandAsync(botClient, chatId, cancellationToken),
                "Управление музыкой" => HandleMusicControlMenuAsync(botClient, chatId, cancellationToken),
                "Следующая песня" => HandleNextSongCommandAsync(botClient, chatId, cancellationToken),
                "Предыдущая песня" => HandlePreviousSongCommandAsync(botClient, chatId, cancellationToken),
                "Пауза" => HandlePauseCommandAsync(botClient, chatId, cancellationToken),
                "Продолжить" => HandleResumeCommandAsync(botClient, chatId, cancellationToken),
                "Назад" => HandleBackCommandAsync(botClient, chatId, cancellationToken),
                "Тревога"=> HandleAlarmCommandAsync(botClient, chatId, cancellationToken),
                "Добавить песню" => HandleAddSongCommandAsync(botClient, chatId, cancellationToken), 
                _ => HandleUnknownCommandAsync(botClient, chatId, cancellationToken)
            };

            if (response != null)
            {
                await response;
            }
        }
        if (update.Type == UpdateType.Message && update.Message?.Type == MessageType.Audio)
        {
            var chatId = update.Message.Chat.Id;
            var document = update.Message.Audio;

            if (document.MimeType == "audio/mpeg")
            {
                await HandleReceivedSongAsync(botClient, chatId, document.FileId, document.FileName, cancellationToken);
            }
            else
            {
                await botClient.SendTextMessageAsync(
                    chatId: chatId,
                    text: "Пожалуйста, отправьте файл в формате MP3.",
                    cancellationToken: cancellationToken);
            }
        }
    }
    private async Task HandleAlarmCommandAsync(ITelegramBotClient botClient, long chatId, CancellationToken cancellationToken)
    {

        _bellManagerService.ToggleAlarm();

        await botClient.SendTextMessageAsync(
            chatId: chatId,
            text: _bellManagerService._isAlarmActive ? "Тревога включена!" : "Тревога выключена!",
            cancellationToken: cancellationToken);
    }

    /// <summary>
    ///     Метод для старт
    /// </summary>
    /// <param name="botClient"></param>
    /// <param name="chatId"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    private async Task HandleStartCommandAsync(ITelegramBotClient botClient, long chatId,
        CancellationToken cancellationToken)
    {
        var replyKeyboardMarkup = new ReplyKeyboardMarkup(new[]
        {
            new KeyboardButton[] { "Включить", "Выключить" },
            new KeyboardButton[] { "Управление музыкой" },
            new KeyboardButton[] { "Тревога" },
            new KeyboardButton[] { "Назад" }
        })
        {
            ResizeKeyboard = true
        };

        await botClient.SendTextMessageAsync(
            chatId: chatId,
            text: "Выберите действие:",
            replyMarkup: replyKeyboardMarkup,
            cancellationToken: cancellationToken);
    }

    /// <summary>
    ///     Метод для отключения
    /// </summary>
    /// <param name="botClient"></param>
    /// <param name="chatId"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    private async Task HandleTurnOffCommandAsync(ITelegramBotClient botClient, long chatId,
        CancellationToken cancellationToken)
    {
        if (_isForm1Open)
        {
            CloseMainForm();
            _isForm1Open = false;

            await botClient.SendTextMessageAsync(
                chatId,
                "Менеджер выключен",
                replyMarkup: new ReplyKeyboardMarkup(new[]
                {
                    new KeyboardButton("Включить"),
                    new KeyboardButton("Управление музыкой"),
                    new KeyboardButton("Тревога" )

                })
                {
                    ResizeKeyboard = true,
                    OneTimeKeyboard = true
                },
                cancellationToken: cancellationToken);
        }
    }
    private async Task HandleReceivedSongAsync(ITelegramBotClient botClient, long chatId, string fileId, string fileName, CancellationToken cancellationToken)
    {
        var file = await botClient.GetFileAsync(fileId, cancellationToken);

        // Очищаем имя файла от недопустимых символов
        var safeFileName = Path.GetInvalidFileNameChars()
            .Aggregate(fileName, (current, invalidChar) => current.Replace(invalidChar, '_'));

        // Сохраняем файл на сервере
        var filePath = Path.Combine("Sounds", safeFileName);
        Directory.CreateDirectory("Sounds"); // Создаем папку, если её нет

        using (var saveStream = System.IO.File.Open(filePath, FileMode.Create))
        {
            await botClient.DownloadFile(file.FilePath, saveStream, cancellationToken);
        }

        await botClient.SendTextMessageAsync(
            chatId: chatId,
            text: $"Песня '{safeFileName}' успешно добавлена!",
            cancellationToken: cancellationToken);
    }


    /// <summary>
    ///     Метод включения
    /// </summary>
    /// <param name="botClient"></param>
    /// <param name="chatId"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    private async Task HandleTurnOnCommandAsync(ITelegramBotClient botClient, long chatId,
        CancellationToken cancellationToken)
    {
        if (!_isForm1Open)
        {
            OpenMainForm();
            _isForm1Open = true;

            await botClient.SendTextMessageAsync(
                chatId,
                "Менеджер запущен",
                replyMarkup: new ReplyKeyboardMarkup(new[]
                {
                    new KeyboardButton("Выключить"),
                    new KeyboardButton("Управление музыкой"),                   
                    new KeyboardButton("Тревога" )

                })
                {
                    ResizeKeyboard = true,
                    OneTimeKeyboard = true
                },
                cancellationToken: cancellationToken);
        }
    }

    /// <summary>
    ///     Метод для отображения меню управления музыкой
    /// </summary>
    /// <param name="botClient"></param>
    /// <param name="chatId"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    private async Task HandleMusicControlMenuAsync(ITelegramBotClient botClient, long chatId,
        CancellationToken cancellationToken)
    {
        await botClient.SendTextMessageAsync(
            chatId,
            "Выберите действие:",
            replyMarkup: GetMusicControlMenuKeyboard(),
            cancellationToken: cancellationToken);
    }

    /// <summary>
    ///     Метод для воспроизведения следующей песни
    /// </summary>
    /// <param name="botClient"></param>
    /// <param name="chatId"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    private async Task HandleNextSongCommandAsync(ITelegramBotClient botClient, long chatId,
        CancellationToken cancellationToken)
    {
        try
        {
            await _bellManagerService.PlayNextSongAsync();
            await botClient.SendTextMessageAsync(chatId, "Следующая песня воспроизводится.", cancellationToken: cancellationToken);
        }
        catch (InvalidOperationException ex)
        {
            await botClient.SendTextMessageAsync(chatId, ex.Message, cancellationToken: cancellationToken);
        }
    }

    /// <summary>
    ///     Метод для воспроизведения предыдущей песни
    /// </summary>
    /// <param name="botClient"></param>
    /// <param name="chatId"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    private async Task HandlePreviousSongCommandAsync(ITelegramBotClient botClient, long chatId, CancellationToken cancellationToken)
    {
        try
        {
            await _bellManagerService.PlayPreviousSongAsync();
            await botClient.SendTextMessageAsync(chatId, "Предыдущая песня воспроизводится.", cancellationToken: cancellationToken);
        }
        catch (InvalidOperationException ex)
        {
            await botClient.SendTextMessageAsync(chatId, ex.Message, cancellationToken: cancellationToken);
        }
        catch (Exception ex)
        {
            await botClient.SendTextMessageAsync(chatId, $"Произошла ошибка: {ex.Message}", cancellationToken: cancellationToken);
        }
    }

    /// <summary>
    ///     Метод для паузы музыки
    /// </summary>
    /// <param name="botClient"></param>
    /// <param name="chatId"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    private async Task HandlePauseCommandAsync(ITelegramBotClient botClient, long chatId,
        CancellationToken cancellationToken)
    {
        try
        {
            _bellManagerService.PauseMusic();
            await botClient.SendTextMessageAsync(chatId, "Музыка на паузе.", cancellationToken: cancellationToken);
        }
        catch (InvalidOperationException ex)
        {
            await botClient.SendTextMessageAsync(chatId, ex.Message, cancellationToken: cancellationToken);
        }
    }

    /// <summary>
    ///     Метод для возобновления музыки
    /// </summary>
    /// <param name="botClient"></param>
    /// <param name="chatId"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    private async Task HandleResumeCommandAsync(ITelegramBotClient botClient, long chatId,
        CancellationToken cancellationToken)
    {
        try
        {
            _bellManagerService.ResumeMusic();
            await botClient.SendTextMessageAsync(chatId, "Музыка продолжена.", cancellationToken: cancellationToken);
        }
        catch (InvalidOperationException ex)
        {
            await botClient.SendTextMessageAsync(chatId, ex.Message, cancellationToken: cancellationToken);
        }
    }

    /// <summary>
    ///     Метод для возврата в главное меню
    /// </summary>
    /// <param name="botClient"></param>
    /// <param name="chatId"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    private async Task HandleBackCommandAsync(ITelegramBotClient botClient, long chatId,
        CancellationToken cancellationToken)
    {
        await botClient.SendTextMessageAsync(
            chatId,
            "Возврат в главное меню.",
            replyMarkup: GetMainMenuKeyboard(),
            cancellationToken: cancellationToken);
    }

    /// <summary>
    ///     Метод для обработки неизвестных команд
    /// </summary>
    /// <param name="botClient"></param>
    /// <param name="chatId"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    private async Task HandleUnknownCommandAsync(ITelegramBotClient botClient, long chatId,
        CancellationToken cancellationToken)
    {
        await botClient.SendTextMessageAsync(chatId, "Неизвестная команда.", cancellationToken: cancellationToken);
    }

    /// <summary>
    ///     Используем Invoke для управления UI из потока бота, а также передаем управление новому MainForm(основной поток)
    /// </summary>
    private void OpenMainForm()
    {
        _isForm1Open = true;
        if (Application.OpenForms["MainForm"] == null)
            Application.OpenForms[0].Invoke(() =>
            {
                _form1 = new MainForm(_bellManagerService);
                _form1.Show();
            });
    }

    /// <summary>
    ///     Используем Invoke для управления UI из потока бота, а также передаем управление новому MainForm(Основной поток)
    /// </summary>
    private void CloseMainForm()
    {
        if (_form1 != null && !_form1.IsDisposed)
            Application.OpenForms[0].Invoke(() =>
            {
                _form1.SafeClose();
                _form1 = null;
            });
    }

    /// <summary>
    ///     Отлов ошибок
    /// </summary>
    /// <param name="botClient"></param>
    /// <param name="exception"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    private Task HandlePollingErrorAsync(ITelegramBotClient botClient, Exception exception,
        CancellationToken cancellationToken)
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

    /// <summary>
    ///     Получить клавиатуру для главного меню
    /// </summary>
    /// <returns></returns>
    private ReplyKeyboardMarkup GetMainMenuKeyboard()
    {
        return new ReplyKeyboardMarkup(new[]
        {
            new KeyboardButton(_isForm1Open ? "Выключить" : "Включить"),
            new KeyboardButton("Управление музыкой"),
            new KeyboardButton( "Тревога" )


        })
        {
            ResizeKeyboard = true,
            OneTimeKeyboard = true
        };
    }

    /// <summary>
    ///     Получить клавиатуру для управления музыкой
    /// </summary>
    /// <returns></returns>
    private ReplyKeyboardMarkup GetMusicControlMenuKeyboard()
    {
        return new ReplyKeyboardMarkup(new[]
        {
            new KeyboardButton[] { "Следующая песня", "Предыдущая песня" },
            new KeyboardButton[] { "Пауза", "Продолжить" },
            new KeyboardButton[] { "Добавить песню" }, 
            new KeyboardButton[] { "Назад" }
        })
        {
            ResizeKeyboard = true,
            OneTimeKeyboard = true
        };
    }

    public void Stop()
    {
        _cancellationTokenSource.Cancel();
    }
    private async Task HandleAddSongCommandAsync(ITelegramBotClient botClient, long chatId, CancellationToken cancellationToken)
    {
        await botClient.SendTextMessageAsync(
            chatId: chatId,
            text: "Пожалуйста, отправьте песню в формате MP3.",
            cancellationToken: cancellationToken);
    }
}