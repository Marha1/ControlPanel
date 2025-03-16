﻿using BellManager.Models;
using BellManager.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BellManager.Service
{
    public class BreakService
    {
        private readonly BreakRepository _breakRepository;
        public BreakService()
        {
                _breakRepository = new BreakRepository();
        }

        public   async Task AddBreaksBetweenLessons(List<Lesson> lessons,string? sound)
        {
            if(lessons.Count==0) return;
            await  _breakRepository.AddBreaksBetweenLessons(lessons,sound);
        }
        public async Task AddBreak(Break breakItem)
        {
            if (breakItem == null)
                throw new ArgumentNullException(nameof(breakItem));
          await  _breakRepository.AddAsync(breakItem);
        }
        public async Task<List<Break>> GetBreaks()
        {
           return await _breakRepository.GetAllAsync();
        }
        public async Task Delete(Break lesson)
        {
            await _breakRepository.DeleteAsync(lesson);
        }

        public async Task Update(Break _break)
        {
            if (_break == null) MessageBox.Show("Перемена! для удаления не найден!");
            await _breakRepository.UpdateAsync(_break);
        }
        public async Task Delete(int id)
        {
            var les = await  _breakRepository.GetByIdAsync(id);
            if (les == null) MessageBox.Show("Перемена! для удаления не найден!");
            else await _breakRepository.DeleteAsync(les);
        }
    }
}
