using Exercise_Application.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exercise_Infrastructure.DataAccess
{
    public class ExerciseRepository : IExerciseRepository
    {
        public async Task<Exercise> AddExerciseAsync(Exercise exercise)
        {
            throw new NotImplementedException();
        }

        public async Task<Exercise?> GetExerciseByIdAsync(Guid exerciseId)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Exercise>> GetExercisesByKeywordsAsync(IEnumerable<Guid> keywordIds)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Exercise>> GetExercisesByTeacherIdAsync(Guid teacherId)
        {
            throw new NotImplementedException();
        }

        public async Task<Exercise> UpdateExerciseAsync(Exercise exercise, byte[] rowVersion)
        {
            throw new NotImplementedException();
        }

    }
}
