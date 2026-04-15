using Exercise_Application.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exercise_Infrastructure.DataAccess
{
    public class ExerciseKeywordRepository : IExerciseKeywordRepository
    {
        private DbContext dbContext;
        public ExerciseKeywordRepository(DbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public async Task<ExerciseKeyword> AddExerciseKeywordAsync(ExerciseKeyword exerciseKeyword)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<ExerciseKeyword>> GetExerciseKeywordsByExerciseIdAsync(Guid exerciseId)
        {
            throw new NotImplementedException();
        }
    }
}
