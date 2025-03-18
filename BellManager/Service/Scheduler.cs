    using BellManager.Models;
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
            private Timer _timer;
            private bool _isProcessing = false;

            // Флаги для отслеживания срабатывания событий
            private bool _isLessonStartedTriggered = false;
            private bool _isLessonEndedTriggered = false;
            private bool _isBreakStartedTriggered = false;
            private bool _isBreakEndingSoonTriggered = false;
            
            //public event EventHandler<Lesson> LessonEnded;
            public event EventHandler<Break> BreakStarted;
            public event EventHandler<Break> BreakEndingSoon;
            public event Func<object, Lesson, Task> LessonStarted;
            public event EventHandler StopAllSounds;

            public Scheduler()
            {
                _lessonService = new LessonService();
                _breakService = new BreakService();
                _timer = new Timer(5000); // Проверка каждые 5 секунд
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
                   var lessons = await _lessonService.GetLessons();
                    var breaks = await _breakService.GetBreaks();

                    await _breakService.AddBreaksBetweenLessons(lessons, string.Empty);

                    var now = DateTime.Now.TimeOfDay;

                    foreach (var lesson in lessons)
                    {
                        if (lesson.IsActive)
                        {
                            // Если текущее время попадает в диапазон времени урока
                            if (now >= lesson.StartTime && now <= lesson.EndTime)
                            {
                                StopAllSounds?.Invoke(this, null);
                            }

                            // За 15 секунд до урока
                            if (now >= lesson.StartTime.Add(TimeSpan.FromSeconds(-3)) && now < lesson.EndTime)
                            {
                                if (!_isLessonStartedTriggered)
                                {
                                    _isLessonStartedTriggered = true;
                                    await LessonStarted?.Invoke(this, lesson);
                                }
                            }
                            else if (now >= lesson.EndTime)
                            {
                                _isLessonStartedTriggered = false;
                            }

                            if (now >= lesson.EndTime && now < lesson.EndTime.Add(TimeSpan.FromSeconds(59)))
                            {
                                if (!_isLessonEndedTriggered)
                                {
                                    _isLessonEndedTriggered = true; 
                                    //LessonEnded?.Invoke(this, lesson);
                                    await _lessonService.MarkLessonAsInactiveAsync(lesson.Id);
                                }
                            }
                            else if (now < lesson.EndTime)
                            {
                                _isLessonEndedTriggered = false; 
                            }
                        }
                        
                    }

                    // Обработка перерывов
                    foreach (var breakItem in breaks)
                    {
                        // В момент начала перемены
                        if (now >= breakItem.StartTime && now < breakItem.StartTime.Add(TimeSpan.FromSeconds(59)))
                        {
                            if (!_isBreakStartedTriggered)
                            {
                                _isBreakStartedTriggered = true;
                                BreakStarted?.Invoke(this, breakItem);
                            }
                        }
                        else if (now >= breakItem.StartTime)
                        {
                            _isLessonStartedTriggered = false;
                        }

                        // За 2 минуты до окончания перемены
                        if (now >= breakItem.EndTime.Add(TimeSpan.FromMinutes(-2)) && now < breakItem.EndTime)
                        {
                            if (!_isBreakEndingSoonTriggered)
                            {
                                _isBreakEndingSoonTriggered = true;
                                _isBreakStartedTriggered = false;
                                BreakEndingSoon?.Invoke(this, breakItem);
                            }
                        }
                        else if (now > breakItem.EndTime)
                        {
                            _isBreakEndingSoonTriggered = false; 
                        }
                    }

                    MessageBox.Show($"Отлад данные:состояния: \n _isLessonStartedTriggered:{_isLessonStartedTriggered}\n_isLessonEndedTriggered:{_isLessonEndedTriggered}\n_isBreakStartedTriggered:{_isBreakStartedTriggered}\n_isBreakEndingSoonTriggered:{_isBreakEndingSoonTriggered}");
                }
                catch (Exception ex)
                {
                    // Логирование ошибки
                    MessageBox.Show($"Ошибка в обработке событий: {ex.Message}");
                }
                finally
                {
                    _isProcessing = false;
                }
            }
        }
    }