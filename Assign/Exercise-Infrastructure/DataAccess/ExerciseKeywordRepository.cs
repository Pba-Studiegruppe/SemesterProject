using Exercise_Application.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exercise_Infrastructure.DataAccess
{
    public class ExerciseKeywordRepository : IExerciseKeywordRepository
    {
        public Task<ExerciseKeyword> AddExerciseKeywordAsync(ExerciseKeyword exerciseKeyword)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<ExerciseKeyword>> GetExerciseKeywordsByExerciseIdAsync(Guid exerciseId)
        {
            throw new NotImplementedException();
        }
    }
}
