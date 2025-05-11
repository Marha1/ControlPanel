using BellManager.Models;
using BellManager.Service;
using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;

public sealed class Scheduler : IDisposable
{
    private const int TimerIntervalMs = 1000;
    private const int LessonEndWindowSec = 59;
    private const int BreakEndWarningMinutes = 2;

    private readonly LessonService _lessonService = new();
    private readonly BreakService _breakService = new();
    private readonly System.Timers.Timer _timer;
    private readonly SemaphoreSlim _semaphore = new(1, 1);
    private bool _disposed;

    // Потокобезопасные коллекции для отслеживания состояний
    private readonly ConcurrentDictionary<int, byte> _startedLessons = new();
    private readonly ConcurrentDictionary<int, byte> _endedLessons = new();
    private readonly ConcurrentDictionary<int, byte> _startedBreaks = new();
    private readonly ConcurrentDictionary<int, byte> _endingSoonBreaks = new();

    public event EventHandler<Break> BreakStarted;
    public event EventHandler<Break> BreakEndingSoon;
    public event Func<object, Lesson, Task> LessonStarted;
    public event EventHandler StopAllSounds;

    public Scheduler()
    {
        _timer = new System.Timers.Timer(TimerIntervalMs);
        _timer.Elapsed += OnTimerElapsedSafe;
    }

    private async void OnTimerElapsedSafe(object sender, ElapsedEventArgs e)
    {
        try
        {
            await ProcessScheduleAsync(e.SignalTime);
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"[Scheduler Error] {ex}");
        }
    }

    public void Start() => _timer.Start();
    public void Stop() => _timer.Stop();

    private async Task ProcessScheduleAsync(DateTime nowTime)
    {
        if (!await _semaphore.WaitAsync(0)) return;

        try
        {
            var now = nowTime.TimeOfDay;
            var lessons = await GetValidLessonsAsync();
            var breaks = await GetValidBreaksAsync();

            if (lessons == null || breaks == null) return;

            await ProcessLessonsAsync(lessons, now);
            await ProcessBreaksAsync(breaks, now);
        }
        finally
        {
            _semaphore.Release();
        }
    }

    private async Task<List<Lesson>> GetValidLessonsAsync()
    {
        var lessons = (await _lessonService.GetLessons())?
            .Where(l => l != null && l.IsActive && l.StartTime != null && l.EndTime != null)
            .ToList();

        if (lessons == null) Debug.WriteLine("[Warning] No valid lessons");
        return lessons;
    }

    private async Task<List<Break>> GetValidBreaksAsync()
    {
        var breaks = (await _breakService.GetBreaks())?
            .Where(b => b != null && b.StartTime != null && b.EndTime != null)
            .ToList();

        if (breaks == null) Debug.WriteLine("[Warning] No valid breaks");
        return breaks;
    }

    private async Task ProcessLessonsAsync(List<Lesson> lessons, TimeSpan now)
    {
        foreach (var lesson in lessons)
        {
            if (now >= lesson.StartTime && now < lesson.EndTime)
            {
                await HandleLessonStart(lesson);
            }
            else
            {
                _startedLessons.TryRemove(lesson.Id, out _);
                await HandleLessonEnd(lesson, now);
            }
        }
    }

    private async Task HandleLessonStart(Lesson lesson)
    {
        if (_startedLessons.TryAdd(lesson.Id, 0))
        {
            StopAllSounds?.Invoke(this, EventArgs.Empty);
            try
            {
                await (LessonStarted?.Invoke(this, lesson) ?? Task.CompletedTask);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[Lesson Start Error] {ex}");
                _startedLessons.TryRemove(lesson.Id, out _);
            }
        }
    }


    private async Task HandleLessonEnd(Lesson lesson, TimeSpan now)
    {
        var endWindow = lesson.EndTime.Add(TimeSpan.FromSeconds(LessonEndWindowSec));
        if (now >= lesson.EndTime && now < endWindow && lesson.IsActive)
        {
            if (_endedLessons.TryAdd(lesson.Id, 0))
            {
                try
                {
                    await _lessonService.MarkLessonAsInactiveAsync(lesson.Id);
                }
                finally
                {
                    _endedLessons.TryRemove(lesson.Id, out _);
                }
            }
        }
    }

    private async Task ProcessBreaksAsync(List<Break> breaks, TimeSpan now)
    {
        foreach (var br in breaks)
        {
            // 1. Проверка начала перемены
            if (now >= br.StartTime && now < br.EndTime)
            {
                HandleBreakStart(br);

                // 2. Проверка приближения конца перемены
                var soonTime = br.EndTime.Subtract(TimeSpan.FromMinutes(BreakEndWarningMinutes));
                if (now >= soonTime)
                {
                    HandleBreakEnd(br, now);
                }
            }
            else
            {
                // 3. Очистка флагов, если перемена завершилась
                _startedBreaks.TryRemove(br.Id, out _);
                _endingSoonBreaks.TryRemove(br.Id, out _);
            }
        }
    }

    private void HandleBreakStart(Break br)
    {
        if (_startedBreaks.TryAdd(br.Id, 0))
        {
            try
            {
                BreakStarted?.Invoke(this, br);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[Break Start Error] {ex}");
                _startedBreaks.TryRemove(br.Id, out _);
            }
        }
    }

    private void HandleBreakEnd(Break br, TimeSpan now)
    {
        if (_endingSoonBreaks.TryAdd(br.Id, 0)) 
        {
            try
            {
                Debug.WriteLine($"[Break Warning] Processing for break {br.Id}");
                BreakEndingSoon?.Invoke(this, br);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[HandleBreakEnd Error] {ex}");
            }
        }
    }


    public void Dispose()
    {
        if (_disposed) return;

        _timer?.Dispose();
        _semaphore?.Dispose();
        _disposed = true;
    }
}
