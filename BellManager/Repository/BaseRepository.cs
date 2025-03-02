using BellManager.DataBase;
using BellManager.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BellManager.Repository
{
    public class BaseRepository<T> where T : BaseModel
    {
        private readonly ApplicationContextFactory _contextFactory;

        public BaseRepository()
        {
            _contextFactory = new ApplicationContextFactory();
        }

        public async Task AddAsync(T entity)
        {
            using var _application = _contextFactory.CreateDbContext();
            await _application.Set<T>().AddAsync(entity);
            await _application.SaveChangesAsync();
        }

        public async Task UpdateAsync(T entity)
        {
            using var _application = _contextFactory.CreateDbContext();
            _application.Set<T>().Update(entity);
            await _application.SaveChangesAsync();
        }

        public async Task<T> GetByIdAsync(int id)
        {
            using var _application = _contextFactory.CreateDbContext();
            return await _application.Set<T>().FirstOrDefaultAsync(e => e.Id == id);
        }

        public async Task<List<T>> GetAllAsync()
        {
            using var _application = _contextFactory.CreateDbContext();
            return await _application.Set<T>().ToListAsync();
        }

        public async Task DeleteAsync(T entity)
        {
            using var _application = _contextFactory.CreateDbContext();
            _application.Set<T>().Remove(entity);
            await _application.SaveChangesAsync();
        }
    }
}
