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
        }
        public async Task MarkLessonAsInactiveAsync(int lessonId)
        {
            var lesson = await _repository.GetByIdAsync(lessonId);
            if (lesson != null)
            {
                lesson.IsActive = false;
                await _repository.UpdateAsync(lesson);
            }
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
    }
}
