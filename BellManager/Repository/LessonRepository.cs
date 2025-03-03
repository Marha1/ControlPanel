using BellManager.DataBase;
using BellManager.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BellManager.Repository
{
    public class LessonRepository : BaseRepository<Lesson>
    {
        private readonly ApplicationContextFactory _contextFactory;
        public LessonRepository()
        {
            _contextFactory = new ApplicationContextFactory();
        }

    }
}
