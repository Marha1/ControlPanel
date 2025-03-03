using BellManager.DataBase;
using BellManager.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BellManager.Repository
{
    public class BreakRepository : BaseRepository<Break>
    {
        private readonly ApplicationContextFactory _contextFactory;

        public BreakRepository()
        {
            _contextFactory = new ApplicationContextFactory();
        }
        public async Task AddBreaksBetweenLessons(List<Lesson> lessons)
        {
            if (lessons == null || lessons.Count < 2)
                return;

            for (int i = 0; i < lessons.Count - 1; i++)
            {
                var currentLesson = lessons[i];
                var nextLesson = lessons[i + 1];

                if (currentLesson.EndTime >= nextLesson.StartTime)
                {
                    throw new InvalidOperationException($"Некорректное время: урок '{currentLesson.Name}' заканчивается позже начала урока '{nextLesson.Name}'.");
                }

                var breakStartTime = currentLesson.EndTime;
                var breakEndTime = nextLesson.StartTime;
                var breakDuration = breakEndTime - breakStartTime;

                if (breakDuration.TotalMinutes >= 5)
                {
                    var breakItem = new Break
                    {
                        Name = $"Перемена между {currentLesson.Name} и {nextLesson.Name}",
                        StartTime = breakStartTime,
                        EndTime = breakEndTime,
                        MusicFile = "Sounds/lesson_end.mp3"
                    };

                    using var _context = _contextFactory.CreateDbContext();
                   await _context.Breaks.AddAsync(breakItem);
                   await _context.SaveChangesAsync();
                }
            }
        }
    }
}
