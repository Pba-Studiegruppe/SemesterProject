using Exercise_Application.DTO;
using Exercise_Application.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exercise_Application.Implementations
{
    public class ExerciseKeywordService : IExerciseKeywordService
    {
        public Task<ExerciseKeyword> AddExerciseKeywordAsync(ExerciseKeywordDTO dto)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<ExerciseKeyword>> GetExerciseKeywordsByExerciseIdAsync(Guid exerciseId)
        {
            throw new NotImplementedException();
        }
    }
}
