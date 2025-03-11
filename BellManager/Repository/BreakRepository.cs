using BellManager.DataBase;
using BellManager.Models;
using Microsoft.EntityFrameworkCore;
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
        private static readonly Random _random = new Random();

        public BreakRepository()
        {
            _contextFactory = new ApplicationContextFactory();
        }
        public async Task AddBreaksBetweenLessons(List<Lesson> lessons,string? sound)
        {
            if (lessons == null || lessons.Count < 2)
                return;

            var sortedLessons = lessons.OrderBy(l => l.StartTime).ToList();

            for (int i = 0; i < sortedLessons.Count - 1; i++)
            {
                var currentLesson = sortedLessons[i];
                var nextLesson = sortedLessons[i + 1];

                // Проверка корректности времени
                if (currentLesson.EndTime >= nextLesson.StartTime)
                {
                    throw new InvalidOperationException(
                        $"Некорректное время: урок '{currentLesson.Name}' заканчивается позже начала урока '{nextLesson.Name}'."
                    );
                }

                // Рассчитываем время перемены
                var breakStartTime = currentLesson.EndTime;
                var breakEndTime = nextLesson.StartTime;
                var breakDuration = breakEndTime - breakStartTime;

                // Если перемена длится больше или равно 5 минутам, создаем её
                if (breakDuration.TotalMinutes >= 5)
                {
                    var breakItem = new Break
                    {
                        Name = $"Перемена между {currentLesson.Name} и {nextLesson.Name}",
                        StartTime = breakStartTime,
                        EndTime = breakEndTime,
                        MusicFile = String.IsNullOrEmpty(sound) ? GetRandom() : sound
                    };

                    using var _context = _contextFactory.CreateDbContext();

                    // Проверяем, существует ли такая перемена
                    bool exists = await _context.Breaks.AnyAsync(b =>
                        b.StartTime == breakItem.StartTime &&
                        b.EndTime == breakItem.EndTime &&
                        b.Name == breakItem.Name
                    );

                    if (!exists)
                    {
                        await _context.Breaks.AddAsync(breakItem);
                        await _context.SaveChangesAsync();
                    }
                }
            }
        }
        private string GetRandom()
        {
            string soundsFolderPath = "Sounds";

            if (Directory.Exists(soundsFolderPath))
            {
                var mp3Files = Directory.GetFiles(soundsFolderPath, "*.mp3");

                if (mp3Files.Length > 0)
                {
                    int randomIndex = _random.Next(0, mp3Files.Length);
                    return Path.GetFileName(mp3Files[randomIndex]);
                }
            }
            return null;
        }
    }
}
