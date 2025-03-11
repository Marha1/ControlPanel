using BellManager.Models;
using BellManager.Service;
using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

public class BellManagerService
{
    private readonly Scheduler _scheduler;

    // Флаги состояния
    public volatile bool _isAlarmActive = false;
    private volatile bool _isMusicPlaying = false;
    private volatile bool _isSoundPlaying = false;

    // Объекты для синхронизации: для музыки и для общих операций
    private readonly object _musicLock = new object();
    private readonly object _globalLock = new object();

    // Постоянный плеер для фоновой музыки и текущий аудиоридер
    private WaveOutEvent _musicPlayer;
    private AudioFileReader _currentMusicReader;

    // Токены отмены для фоновой музыки и тревоги
    private CancellationTokenSource _musicCts;
    private CancellationTokenSource _alarmCts;

    // Список музыкальных файлов и индекс текущей песни
    private List<string> _musicFiles;
    private int _currentSongIndex = 0;

    // Состояние для восстановления воспроизведения после тревоги
    private TimeSpan _savedPosition = TimeSpan.Zero;
    private bool _shouldResumeAfterAlarm = false;

    public BellManagerService()
    {
        _scheduler = new Scheduler();
        _scheduler.LessonStarted += async (s, lesson) => await OnLessonStarted(lesson);
        _scheduler.BreakStarted += async (s, breakItem) => await OnBreakStarted(breakItem);
        _scheduler.BreakEndingSoon += async (s, breakItem) => await OnBreakEndingSoon(breakItem);
        _scheduler.StopAllSounds += (s, e) => StopAllSounds();

        // Запускаем планировщик в отдельном потоке
        Task.Run(() => _scheduler.Start());
    }

    /// <summary>
    /// Останавливает все звуки: фоновые эффекты, музыку и тревогу.
    /// </summary>
    private void StopAllSounds()
    {
        lock (_globalLock)
        {
            _isSoundPlaying = false;
            _isMusicPlaying = false;
            _isAlarmActive = false;

            _musicCts?.Cancel();
            _musicCts?.Dispose();
            _musicCts = null;

            // Останавливаем и освобождаем фоновые ресурсы музыки
            lock (_musicLock)
            {
                _musicPlayer?.Stop();
                _musicPlayer?.Dispose();
                _musicPlayer = null;
                _currentMusicReader?.Dispose();
                _currentMusicReader = null;
            }

            _alarmCts?.Cancel();
            _alarmCts?.Dispose();
            _alarmCts = null;
        }
    }

    private async Task OnLessonStarted(Lesson lesson)
    {
        if (_isSoundPlaying || _isAlarmActive)
            return;

        _isSoundPlaying = true;
        try
        {
            await PlaySoundEffectAsync("zvon.mp3", CancellationToken.None);
            await PlaySoundEffectAsync("LessonStart.mp3", CancellationToken.None);
            ShowMessage($"Урок начался: {lesson.Name}");
        }
        catch (Exception ex)
        {
            ShowMessage(ex.Message);
        }
        finally
        {
            _isSoundPlaying = false;
        }
    }

    private async Task OnBreakStarted(Break breakItem)
    {
        if (_isSoundPlaying || _isAlarmActive)
            return;

        _isSoundPlaying = true;
        try
        {
            await PlaySoundEffectAsync("zvon.mp3", CancellationToken.None);
            await PlaySoundEffectAsync("LessonEnd.mp3", CancellationToken.None);
            ShowMessage($"Перемена началась: {breakItem.Name}");

            lock (_globalLock)
            {
                _musicCts?.Cancel();
                _musicCts?.Dispose();
                _musicCts = new CancellationTokenSource();
            }
            _ = PlayMusicDuringBreakAsync(breakItem, _musicCts.Token);
        }
        catch (Exception ex)
        {
            ShowMessage(ex.Message);
        }
        finally
        {
            _isSoundPlaying = false;
        }
    }

    private async Task OnBreakEndingSoon(Break breakItem)
    {
        if (_isSoundPlaying || _isAlarmActive)
            return;

        _isSoundPlaying = true;
        try
        {
            StopAllSounds();
            await PlaySoundEffectAsync("2min.mp3", CancellationToken.None);
            ShowMessage($"Перемена заканчивается через 2 минуты: {breakItem.Name}");
        }
        catch (Exception ex)
        {
            ShowMessage(ex.Message);
        }
        finally
        {
            _isSoundPlaying = false;
        }
    }

    /// <summary>
    /// Однократно воспроизводит звуковой эффект с использованием временного плеера.
    /// </summary>
    public async Task PlaySoundEffectAsync(string filePath, CancellationToken cancellationToken)
    {
        if (!File.Exists(filePath))
        {
            ShowMessage($"Файл звука не найден: {filePath}");
            return;
        }

        var tcs = new TaskCompletionSource<bool>();

        using (var effectPlayer = new WaveOutEvent())
        using (var reader = new AudioFileReader(filePath))
        {
            effectPlayer.Init(reader);
            effectPlayer.Play();

            effectPlayer.PlaybackStopped += (s, e) =>
            {
                tcs.TrySetResult(true);
            };

            using (cancellationToken.Register(() => tcs.TrySetCanceled()))
            {
                try
                {
                    await tcs.Task;
                }
                catch (TaskCanceledException)
                {
                    effectPlayer.Stop();
                }
            }
        }
    }

    /// <summary>
    /// Воспроизводит фоновые музыкальные треки во время перемены с возможностью управления.
    /// </summary>
    private async Task PlayMusicDuringBreakAsync(Break breakItem, CancellationToken token)
    {
        _isMusicPlaying = true;
        try
        {
            _musicFiles = Directory.GetFiles("Sounds", "*.mp3").ToList();
            if (_musicFiles.Count == 0)
            {
                ShowMessage("Не найдены музыкальные файлы в папке Sounds");
                return;
            }
            _musicFiles = _musicFiles.OrderBy(x => Guid.NewGuid()).ToList();
            _currentSongIndex = 0;

            lock (_musicLock)
            {
                _musicPlayer?.Stop();
                _musicPlayer?.Dispose();
                _musicPlayer = new WaveOutEvent();
            }

            while (DateTime.Now.TimeOfDay < breakItem.EndTime && !token.IsCancellationRequested)
            {
                await PlayMusicTrackAsync(_musicFiles[_currentSongIndex], token);
                lock (_musicLock)
                {
                    _currentSongIndex = (_currentSongIndex + 1) % _musicFiles.Count;
                }
            }
        }
        catch (TaskCanceledException)
        {
            // Выходим по отмене
        }
        finally
        {
            _isMusicPlaying = false;
            lock (_musicLock)
            {
                _musicPlayer?.Stop();
                _musicPlayer?.Dispose();
                _musicPlayer = null;
                _currentMusicReader?.Dispose();
                _currentMusicReader = null;
            }
        }
    }

    /// <summary>
    /// Воспроизводит один музыкальный трек с возможностью отмены.
    /// </summary>
    private Task PlayMusicTrackAsync(string filePath, CancellationToken token)
    {
        var tcs = new TaskCompletionSource<bool>();

        lock (_musicLock)
        {
            _currentMusicReader?.Dispose();
            _currentMusicReader = new AudioFileReader(filePath);
            _musicPlayer.Init(_currentMusicReader);
            _musicPlayer.Play();

            _musicPlayer.PlaybackStopped += (s, e) =>
            {
                tcs.TrySetResult(true);
            };
        }

        token.Register(() =>
        {
            lock (_musicLock)
            {
                _musicPlayer.Stop();
            }
            tcs.TrySetCanceled();
        });

        return tcs.Task;
    }

    /// <summary>
    /// Переключает тревогу: включает, если выключена, и выключает, если включена.
    /// При включении тревоги сохраняется состояние музыки, а при отключении – музыка автоматически возобновляется.
    /// </summary>
    public void ToggleAlarm()
    {
        lock (_globalLock)
        {
            if (_isAlarmActive)
            {
                // Отключаем тревогу и, если музыка была запущена, возобновляем воспроизведение
                _alarmCts?.Cancel();
                _isAlarmActive = false;

                if (_shouldResumeAfterAlarm)
                {
                    ResumeMusicAfterAlarm();
                    _shouldResumeAfterAlarm = false;
                }
            }
            else
            {
                // При включении тревоги сохраняем состояние музыки (если она воспроизводилась)
                lock (_musicLock)
                {
                    if (_musicPlayer != null && _musicPlayer.PlaybackState == PlaybackState.Playing)
                    {
                        _savedPosition = _currentMusicReader?.CurrentTime ?? TimeSpan.Zero;
                        _shouldResumeAfterAlarm = true;
                    }
                }
                // Останавливаем музыку, но не уничтожаем плеер – просто ставим на паузу
                if (_musicPlayer != null)
                {
                    _musicPlayer.Pause();
                }

                // Включаем тревогу
                _alarmCts = new CancellationTokenSource();
                _isAlarmActive = true;
                _ = Task.Run(async () =>
                {
                    while (!_alarmCts.Token.IsCancellationRequested)
                    {
                        await PlaySoundEffectAsync("fire_alarm.mp3", _alarmCts.Token);
                    }
                }, _alarmCts.Token);
            }
        }
    }

    /// <summary>
    /// Автоматически возобновляет воспроизведение музыки с сохранённой позиции.
    /// </summary>
    private void ResumeMusicAfterAlarm()
    {
        lock (_musicLock)
        {
            if (_musicPlayer != null && _currentMusicReader != null)
            {
                _currentMusicReader.CurrentTime = _savedPosition;
                _musicPlayer.Play();
            }
        }
    }

    /// <summary>
    /// Переключает на следующую песню.
    /// </summary>
    public Task PlayNextSongAsync()
    {
        lock (_musicLock)
        {
            if (_musicPlayer != null && _musicFiles != null && _musicFiles.Count > 0)
            {
                _currentSongIndex = (_currentSongIndex + 1) % _musicFiles.Count;
                _musicPlayer.Stop();
            }
        }
        return Task.CompletedTask;
    }

    /// <summary>
    /// Переключает на предыдущую песню.
    /// </summary>
    public Task PlayPreviousSongAsync()
    {
        lock (_musicLock)
        {
            if (_musicPlayer != null && _musicFiles != null && _musicFiles.Count > 0)
            {
                _currentSongIndex = (_currentSongIndex - 1 + _musicFiles.Count) % _musicFiles.Count;
                _musicPlayer.Stop();
            }
        }
        return Task.CompletedTask;
    }

    /// <summary>
    /// Ставит музыку на паузу.
    /// </summary>
    public void PauseMusic()
    {
        lock (_musicLock)
        {
            if (_isAlarmActive)
                throw new InvalidOperationException("Невозможно поставить музыку на паузу, тревога активна.");
            if (_musicPlayer == null)
                throw new InvalidOperationException("Музыкальный плеер не инициализирован.");
            _musicPlayer.Pause();
        }
    }

    /// <summary>
    /// Возобновляет воспроизведение музыки.
    /// </summary>
    public void ResumeMusic()
    {
        lock (_musicLock)
        {
            if (_isAlarmActive)
                throw new InvalidOperationException("Невозможно возобновить музыку, тревога активна.");
            if (_musicPlayer == null)
                throw new InvalidOperationException("Музыкальный плеер не инициализирован.");
            _musicPlayer.Play();
        }
    }

    /// <summary>
    /// Вспомогательный метод для вызова MessageBox в отдельном потоке.
    /// </summary>
    private void ShowMessage(string message)
    {
        Task.Run(() => MessageBox.Show(message));
    }
}
