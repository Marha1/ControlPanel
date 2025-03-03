using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BellManager.Models;
using BellManager.Repository;
namespace BellManager.Service
{
   
    namespace BellManager
    {
        public class BellManagerService
        {

            private readonly Scheduler _scheduler;
            public BellManagerService()
            {
                _scheduler = new Scheduler();
                _scheduler.LessonStarted += OnLessonStarted;
                _scheduler.LessonEnded += OnLessonEnded;
                _scheduler.BreakStarted += OnBreakStarted;
                _scheduler.BreakEndingSoon += OnBreakEndingSoon;
                _scheduler.StopAllSounds += _scheduler_StopAllSounds;
                var schedullerThread = new Thread(() => {
                    _scheduler.Start();
                });

                schedullerThread.IsBackground = true;
                schedullerThread.Start();
            }

            private void _scheduler_StopAllSounds(object? sender, EventArgs e)
            {
                throw new NotImplementedException();
            }

           
            private void OnLessonStarted(object sender, Lesson lesson)
            {
                MessageBox.Show($"Урок начался: {lesson.Name}");
            }
            

            private void OnLessonEnded(object sender, Lesson lesson)
            {
                MessageBox.Show($"Урок закончился: {lesson.Name}");
            }

            private void OnBreakStarted(object sender, Break breakItem)
            {
                MessageBox.Show($"Перемена началась: {breakItem.Name}");
            }

            private void OnBreakEndingSoon(object sender, Break breakItem)
            {
                MessageBox.Show($"Перемена заканчивается через 2 минуты: {breakItem.Name}");
            }

        }
    }
}
