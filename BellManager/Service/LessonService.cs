using BellManager.Models;
using BellManager.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace BellManager.Service
{
    public class LessonService
    {
        private readonly LessonRepository _repository;
        public event Action LessonAdded;
        public LessonService()
        {
                _repository = new LessonRepository();
        }
        public async Task <List<Lesson>> GetLessons()
        {
            return await _repository.GetAllAsync();
        }
        public async Task AddLesson(Lesson lesson)
        {
            if (lesson == null)
                throw new ArgumentNullException(nameof(lesson));
            await _repository.AddAsync(lesson);
            LessonAdded?.Invoke();
        }
        public async Task MarkLessonAsInactiveAsync(int lessonId)
        {
            var lesson = await _repository.GetByIdAsync(lessonId);
            if (lesson != null)
            {
                lesson.IsActive = false;
                await _repository.UpdateAsync(lesson);
                LessonAdded?.Invoke();
            }
        }

        public async Task<Lesson> GetById(int id)
        {
            var lesson = await _repository.GetByIdAsync(id);
            if (lesson != null)
            {
                return lesson;
            }

            return null;
        }

        public async Task Delete(Lesson lesson)
        {
            await _repository.DeleteAsync(lesson);
        }
        public async Task Delete(int id)
        {
          var les=  await _repository.GetByIdAsync(id);
            if (les == null) MessageBox.Show("Урок для удаления не найден!");
            else await _repository.DeleteAsync(les);
        }

        public async Task UpdateLessonTime(int lessonId, TimeSpan newStartTime, TimeSpan newEndTime)
        {
            var less = await _repository.GetByIdAsync(lessonId);
            less.StartTime = newStartTime;
            less.EndTime = newEndTime;
            less.IsActive = true;
            await _repository.UpdateAsync(less);
            LessonAdded?.Invoke();
        }

        public async Task UpdateLessonActiveState(int lessonId, bool newState)
        {
            var lesson = await _repository.GetByIdAsync(lessonId);
            if (lesson == null) MessageBox.Show("Ошибка,сообщите администратору");
            lesson.IsActive =newState;
           await _repository.UpdateAsync(lesson);
           LessonAdded?.Invoke();
        }
    }
}
