using BellManager.Models;
using BellManager.Service.BellManager;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Timers;
using Timer = System.Timers.Timer;


namespace BellManager.Service
{
    public class Scheduler
    {
        private readonly LessonService _lessonService;
        private readonly BreakService _breakService;
        private BellManagerService _bellManager;
        private Timer _timer;
        private bool _isProcessing = false;

        public event EventHandler<Lesson> LessonEnded;
        public event EventHandler<Break> BreakStarted;
        public event EventHandler<Break> BreakEndingSoon;
        public event EventHandler<Lesson> LessonStarted;
        public event EventHandler StopAllSounds;


        public Scheduler()
        {
            _lessonService = new LessonService();
            _breakService = new BreakService();
            _timer = new Timer(30000); 
            _timer.Elapsed += OnTimerElapsed;
        }

        public void Start()
        {
            _timer.Start();
        }

        public void Stop()
        {
            _timer.Stop();
        }

        private async void OnTimerElapsed(object sender, ElapsedEventArgs e)
        {
            if (_isProcessing) return;

            _isProcessing = true;
            try
            {
                var now = DateTime.Now.TimeOfDay;
                var lessons = await _lessonService.GetLessons();
                var breaks = await _breakService.GetBreaks();

                // Автоматическое добавление перемен между уроками
                await  _breakService.AddBreaksBetweenLessons(lessons);

                // Обработка уроков
                foreach (var lesson in lessons)
                {
                    if (lesson.IsActive)
                    {
                        // Если текущее время попадает в диапазон времени урока
                        if (now >= lesson.StartTime && now <= lesson.EndTime)
                        {
                            StopAllSounds?.Invoke(this,null);
                        }

                        // За 1 минуту до урока
                        if (now >= lesson.StartTime.Add(TimeSpan.FromMinutes(-1)) && now < lesson.StartTime)
                        {
                            LessonStarted?.Invoke(this, lesson);
                        }

                        // В момент окончания урока
                        if (now >= lesson.EndTime && now < lesson.EndTime.Add(TimeSpan.FromSeconds(59)))
                        {
                            LessonEnded?.Invoke(this, lesson);
                            await _lessonService.MarkLessonAsInactiveAsync(lesson.Id);
                        }
                    }
                }

                // Обработка перерывов
                foreach (var breakItem in breaks)
                {
                    // В момент начала перемены
                    if (now >= breakItem.StartTime && now < breakItem.StartTime.Add(TimeSpan.FromSeconds(59)))
                    {
                        BreakStarted?.Invoke(this, breakItem);
                    }

                    // За 2 минуты до окончания перемены
                    if (now >= breakItem.EndTime.Add(TimeSpan.FromMinutes(-2)) && now < breakItem.EndTime.Add(TimeSpan.FromMinutes(-2)).Add(TimeSpan.FromSeconds(59)))
                    {
                        BreakEndingSoon?.Invoke(this, breakItem);
                    }
                }
            }
            catch (Exception ex)
            {
                // Логирование ошибки
                Console.WriteLine($"Ошибка в обработке событий: {ex.Message}");
            }
            finally
            {
                _isProcessing = false;
            }
        }
        
    }
}