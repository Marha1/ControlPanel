using BellManager.Models;
using BellManager.Service;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace BellManager
{
    public class TelegramBot
    {
        private readonly ITelegramBotClient _botClient;
        private readonly CancellationTokenSource _cancellationTokenSource;
        private readonly BellManagerService _bellManagerService;
        private readonly LessonService _lessonService;
        private MainForm _form1;
        private bool _isForm1Open;
        private readonly Dictionary<long, UserEditState> _userStates = new Dictionary<long, UserEditState>();

        public TelegramBot(string token)
        {
            _botClient = new TelegramBotClient(token);
            _cancellationTokenSource = new CancellationTokenSource();
            _bellManagerService = new BellManagerService();
            _lessonService = new LessonService();
        }

        public async Task StartAsync()
        {
            var me = await _botClient.GetMeAsync();

            var receiverOptions = new ReceiverOptions
            {
                AllowedUpdates = Array.Empty<UpdateType>()
            };

            _botClient.StartReceiving(
                HandleUpdateAsync,
                HandlePollingErrorAsync,
                receiverOptions,
                _cancellationTokenSource.Token
            );
        }

        private async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            // Обработка callback-запросов (инлайн кнопки)
            if (update.Type == UpdateType.CallbackQuery)
            {
                var callbackData = update.CallbackQuery.Data;
                var chatId = update.CallbackQuery.Message.Chat.Id;

                if (callbackData.StartsWith("edit_"))
                {
                    // Получаем ID урока из callback данных (например, "edit_5")
                    var lessonIdStr = callbackData.Split('_')[1];
                    if (int.TryParse(lessonIdStr, out int lessonId))
                    {
                        await HandleLessonEditAsync(botClient, chatId, lessonId, cancellationToken);
                    }
                }
                else if (callbackData.StartsWith("changeTime_"))
                {
                    // Извлекаем ID урока из callback (например, "changeTime_5")
                    var lessonIdStr = callbackData.Split('_')[1];
                    if (int.TryParse(lessonIdStr, out int lessonId))
                    {
                        // Устанавливаем состояние пользователя на редактирование времени
                        _userStates[chatId] = new UserEditState { LessonId = lessonId, Field = EditField.Time };

                        await botClient.SendTextMessageAsync(
                            chatId,
                            "Введите новое время урока в формате HH:mm-HH:mm (например, 08:00-08:45):",
                            cancellationToken: cancellationToken);
                    }
                }
                else if (callbackData == "addLesson")
                {
                    // Устанавливаем состояние на добавление нового урока (ввод названия)
                    _userStates[chatId] = new UserEditState { Field = EditField.AddLessonName };
                    await botClient.SendTextMessageAsync(
                        chatId,
                        "Введите название нового урока:",
                        cancellationToken: cancellationToken);
                }
                else if (callbackData.StartsWith("toggleActive_"))
                {
                    // Извлекаем ID урока и переключаем его состояние (IsActive)
                    var lessonIdStr = callbackData.Split('_')[1];
                    if (int.TryParse(lessonIdStr, out int lessonId))
                    {
                        var lesson = await _lessonService.GetById(lessonId);
                        if (lesson != null)
                        {
                            bool newState = !lesson.IsActive;
                            // Метод UpdateLessonActiveState должен обновлять свойство IsActive урока
                            await _lessonService.UpdateLessonActiveState(lessonId, newState);
                            await botClient.SendTextMessageAsync(
                                chatId,
                                $"Урок '{lesson.Name}' теперь {(newState ? "включен" : "выключен")}.",
                                cancellationToken: cancellationToken);
                        }
                        else
                        {
                            await botClient.SendTextMessageAsync(
                                chatId,
                                "Урок не найден.",
                                cancellationToken: cancellationToken);
                        }
                    }
                }
                else if (callbackData == "back_to_main")
                {
                    await HandleBackCommandAsync(botClient, chatId, cancellationToken);
                }
                else if (callbackData == "back_to_schedule")
                {
                    await HandleScheduleManagementMenuAsync(botClient, chatId, cancellationToken);
                }

                await botClient.AnswerCallbackQueryAsync(update.CallbackQuery.Id, cancellationToken: cancellationToken);
            }

            // Обработка текстовых сообщений
            if (update.Type == UpdateType.Message && update.Message?.Type == MessageType.Text)
            {
                var chatId = update.Message.Chat.Id;
                var text = update.Message.Text;

                // Обработка ввода для изменения времени урока
                if (_userStates.ContainsKey(chatId) && _userStates[chatId].Field == EditField.Time)
                {
                    if (TryParseLessonTime(text, out TimeSpan newStartTime, out TimeSpan newEndTime))
                    {
                        int lessonId = _userStates[chatId].LessonId;
                        // Обновление времени урока через LessonService
                        await _lessonService.UpdateLessonTime(lessonId, newStartTime, newEndTime);

                        // Сбрасываем состояние редактирования
                        _userStates[chatId].Field = EditField.None;

                        await botClient.SendTextMessageAsync(
                            chatId,
                            $"Время урока обновлено на: {newStartTime:hh\\:mm} - {newEndTime:hh\\:mm}",
                            cancellationToken: cancellationToken);
                    }
                    else
                    {
                        await botClient.SendTextMessageAsync(
                            chatId,
                            "Неверный формат времени. Введите время в формате HH:mm-HH:mm",
                            cancellationToken: cancellationToken);
                    }
                    return;
                }

                // Обработка ввода для добавления нового урока
                if (_userStates.ContainsKey(chatId))
                {
                    var state = _userStates[chatId];
                    if (state.Field == EditField.AddLessonName)
                    {
                        string lessonName = text.Trim();
                        var existingLesson = (await _lessonService.GetLessons())
                            .FirstOrDefault(l => l.Name.Equals(lessonName, StringComparison.OrdinalIgnoreCase));
                        if (existingLesson != null)
                        {
                            await botClient.SendTextMessageAsync(
                                chatId,
                                "Урок с таким названием уже существует. Попробуйте другое название.",
                                cancellationToken: cancellationToken);
                            return;
                        }
                        state.NewLessonName = lessonName;
                        state.Field = EditField.AddLessonTime;
                        await botClient.SendTextMessageAsync(
                            chatId,
                            "Введите время урока в формате HH:mm-HH:mm (например, 08:00-08:45):",
                            cancellationToken: cancellationToken);
                        return;
                    }
                    else if (state.Field == EditField.AddLessonTime)
                    {
                        if (TryParseLessonTime(text, out TimeSpan startTime, out TimeSpan endTime))
                        {
                            Lesson newLesson = new Lesson
                            {
                                Name = state.NewLessonName,
                                StartTime = startTime,
                                EndTime = endTime,
                                IsActive = true
                            };
                            await _lessonService.AddLesson(newLesson);
                            state.Field = EditField.None;
                            await botClient.SendTextMessageAsync(
                                chatId,
                                $"Урок '{newLesson.Name}' добавлен с временем {startTime:hh\\:mm} - {endTime:hh\\:mm}.",
                                cancellationToken: cancellationToken);
                        }
                        else
                        {
                            await botClient.SendTextMessageAsync(
                                chatId,
                                "Неверный формат времени. Введите время в формате HH:mm-HH:mm",
                                cancellationToken: cancellationToken);
                        }
                        return;
                    }
                }

                var response = text switch
                {
                    "/start" => HandleStartCommandAsync(botClient, chatId, cancellationToken),
                    "Включить" => HandleTurnOnCommandAsync(botClient, chatId, cancellationToken),
                    "Выключить" => HandleTurnOffCommandAsync(botClient, chatId, cancellationToken),
                    "Управление музыкой" => HandleMusicControlMenuAsync(botClient, chatId, cancellationToken),
                    "Управление расписанием" => HandleScheduleManagementMenuAsync(botClient, chatId, cancellationToken),
                    "Следующая песня" => HandleNextSongCommandAsync(botClient, chatId, cancellationToken),
                    "Предыдущая песня" => HandlePreviousSongCommandAsync(botClient, chatId, cancellationToken),
                    "Пауза" => HandlePauseCommandAsync(botClient, chatId, cancellationToken),
                    "Продолжить" => HandleResumeCommandAsync(botClient, chatId, cancellationToken),
                    "Назад" => HandleBackCommandAsync(botClient, chatId, cancellationToken),
                    "Тревога" => HandleAlarmCommandAsync(botClient, chatId, cancellationToken),
                    "Добавить песню" => HandleAddSongCommandAsync(botClient, chatId, cancellationToken),
                    _ => HandleUnknownCommandAsync(botClient, chatId, cancellationToken)
                };

                if (response != null)
                {
                    await response;
                }
            }

            // Обработка аудио сообщений (для песен)
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

        private async Task HandleStartCommandAsync(ITelegramBotClient botClient, long chatId, CancellationToken cancellationToken)
        {
            var replyKeyboardMarkup = new ReplyKeyboardMarkup(new[]
            {
                new KeyboardButton[] { "Включить", "Выключить" },
                new KeyboardButton[] { "Управление музыкой", "Управление расписанием" },
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

        private async Task HandleTurnOffCommandAsync(ITelegramBotClient botClient, long chatId, CancellationToken cancellationToken)
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
                        new KeyboardButton("Выключить"),
                        new KeyboardButton("Управление музыкой"),
                        new KeyboardButton("Управление расписанием"),
                        new KeyboardButton("Тревога")
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

            var safeFileName = Path.GetInvalidFileNameChars()
                .Aggregate(fileName, (current, invalidChar) => current.Replace(invalidChar, '_'));

            var filePath = Path.Combine("Sounds", safeFileName);
            Directory.CreateDirectory("Sounds");

            using (var saveStream = System.IO.File.Open(filePath, FileMode.Create))
            {
                await botClient.DownloadFile(file.FilePath, saveStream, cancellationToken);
            }

            await botClient.SendTextMessageAsync(
                chatId: chatId,
                text: $"Песня '{safeFileName}' успешно добавлена!",
                cancellationToken: cancellationToken);
        }

        private async Task HandleTurnOnCommandAsync(ITelegramBotClient botClient, long chatId, CancellationToken cancellationToken)
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
                        new KeyboardButton[] { "Выключить" },
                        new KeyboardButton[] { "Управление музыкой", "Управление расписанием" },
                        new KeyboardButton[] { "Тревога" }
                    })
                    {
                        ResizeKeyboard = true,
                        OneTimeKeyboard = true
                    },
                    cancellationToken: cancellationToken);
            }
        }

        private async Task HandleMusicControlMenuAsync(ITelegramBotClient botClient, long chatId, CancellationToken cancellationToken)
        {
            await botClient.SendTextMessageAsync(
                chatId,
                "Выберите действие:",
                replyMarkup: GetMusicControlMenuKeyboard(),
                cancellationToken: cancellationToken);
        }

        private async Task HandleNextSongCommandAsync(ITelegramBotClient botClient, long chatId, CancellationToken cancellationToken)
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

        private async Task HandlePauseCommandAsync(ITelegramBotClient botClient, long chatId, CancellationToken cancellationToken)
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

        private async Task HandleResumeCommandAsync(ITelegramBotClient botClient, long chatId, CancellationToken cancellationToken)
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

        private async Task HandleBackCommandAsync(ITelegramBotClient botClient, long chatId, CancellationToken cancellationToken)
        {
            await botClient.SendTextMessageAsync(
                chatId,
                "Возврат в главное меню.",
                replyMarkup: GetMainMenuKeyboard(),
                cancellationToken: cancellationToken);
        }

        private async Task HandleUnknownCommandAsync(ITelegramBotClient botClient, long chatId, CancellationToken cancellationToken)
        {
            await botClient.SendTextMessageAsync(chatId, "Неизвестная команда.", cancellationToken: cancellationToken);
        }

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

        private void CloseMainForm()
        {
            if (_form1 != null && !_form1.IsDisposed)
                Application.OpenForms[0].Invoke(() =>
                {
                    _form1.SafeClose();
                    _form1 = null;
                });
        }

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

        private ReplyKeyboardMarkup GetMainMenuKeyboard()
        {
            return new ReplyKeyboardMarkup(new[]
            {
                new KeyboardButton[] { _isForm1Open ? "Выключить" : "Включить" },
                new KeyboardButton[] { "Управление музыкой", "Управление расписанием" },
                new KeyboardButton[] { "Тревога" }
            })
            {
                ResizeKeyboard = true,
                OneTimeKeyboard = true
            };
        }

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

        /// <summary>
        /// Меню управления расписанием.
        /// Получаем список уроков через LessonService и выводим их в виде inline-клавиатуры.
        /// </summary>
        private async Task HandleScheduleManagementMenuAsync(ITelegramBotClient botClient, long chatId, CancellationToken cancellationToken)
        {
            List<Lesson> lessons = await _lessonService.GetLessons();

            var inlineButtons = new List<InlineKeyboardButton[]>();
            foreach (var lesson in lessons)
            {
                inlineButtons.Add(new[]
                {
                    InlineKeyboardButton.WithCallbackData(lesson.Name, $"edit_{lesson.Id}")
                });
            }

            inlineButtons.Add(new[]
            {
                InlineKeyboardButton.WithCallbackData("Добавить урок", "addLesson")
            });
            inlineButtons.Add(new[]
            {
                InlineKeyboardButton.WithCallbackData("Назад", "back_to_main")
            });

            var inlineKeyboard = new InlineKeyboardMarkup(inlineButtons);

            await botClient.SendTextMessageAsync(
                chatId: chatId,
                text: "Список уроков:",
                replyMarkup: inlineKeyboard,
                cancellationToken: cancellationToken);
        }

        /// <summary>
        /// Меню редактирования конкретного урока.
        /// Выводится информация об уроке с его названием, временем и состоянием (включён/выключен),
        /// а также inline-кнопки для изменения времени и переключения состояния урока.
        /// </summary>
        private async Task HandleLessonEditAsync(ITelegramBotClient botClient, long chatId, int lessonId, CancellationToken cancellationToken)
        {
            // Получаем урок через выбор из списка. Рекомендуется добавить метод GetLessonById.
            Lesson lesson = (await _lessonService.GetLessons()).FirstOrDefault(l => l.Id == lessonId);

            if (lesson != null)
            {
                // Текст кнопки зависит от текущего состояния урока
                string toggleButtonText = lesson.IsActive ? "Выключить урок" : "Включить урок";
                var inlineKeyboard = new InlineKeyboardMarkup(new[]
                {
                    new []
                    {
                        InlineKeyboardButton.WithCallbackData("Изменить время", $"changeTime_{lesson.Id}"),
                        InlineKeyboardButton.WithCallbackData(toggleButtonText, $"toggleActive_{lesson.Id}")
                    },
                    new []
                    {
                        InlineKeyboardButton.WithCallbackData("Назад", "back_to_schedule")
                    }
                });

                string timeText = $"{lesson.StartTime:hh\\:mm} - {lesson.EndTime:hh\\:mm}";
                await botClient.SendTextMessageAsync(
                    chatId: chatId,
                    text: $"Редактирование урока:\nНазвание: {lesson.Name}\nВремя: {timeText}\nСтатус: {(lesson.IsActive ? "Включен" : "Выключен")}",
                    replyMarkup: inlineKeyboard,
                    cancellationToken: cancellationToken);
            }
            else
            {
                await botClient.SendTextMessageAsync(
                    chatId: chatId,
                    text: "Урок не найден.",
                    cancellationToken: cancellationToken);
            }
        }

        // Метод для разбора времени урока в формате HH:mm-HH:mm
        public bool TryParseLessonTime(string input, out TimeSpan startTime, out TimeSpan endTime)
        {
            startTime = TimeSpan.Zero;
            endTime = TimeSpan.Zero;

            var parts = input.Split('-');
            if (parts.Length != 2)
                return false;

            if (TimeSpan.TryParse(parts[0].Trim(), out startTime) &&
                TimeSpan.TryParse(parts[1].Trim(), out endTime))
            {
                return true;
            }

            return false;
        }

        public void Stop()
        {
            _cancellationTokenSource.Cancel();
        }

        // Перечисление состояний редактирования
        public enum EditField
        {
            None,
            Time,
            AddLessonName,
            AddLessonTime
        }

        // Класс для хранения состояния редактирования пользователя
        public class UserEditState
        {
            public int LessonId { get; set; }
            public EditField Field { get; set; } = EditField.None;
            public string NewLessonName { get; set; }
        }

        private async Task HandleAddSongCommandAsync(ITelegramBotClient botClient, long chatId, CancellationToken cancellationToken)
        {
            await botClient.SendTextMessageAsync(
                chatId: chatId,
                text: "Пожалуйста, отправьте песню в формате MP3.",
                cancellationToken: cancellationToken);
        }
    }
}
