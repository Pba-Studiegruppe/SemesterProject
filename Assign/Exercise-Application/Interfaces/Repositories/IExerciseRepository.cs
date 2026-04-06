using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exercise_Application.Interfaces.Repositories
{
    public interface IExerciseRepository
    {
        Task<Exercise> AddExerciseAsync(Exercise exercise);
        Task<IEnumerable<Exercise>> GetExercisesByKeywordsAsync(IEnumerable<Guid> keywordIds);
        Task<IEnumerable<Exercise>> GetExercisesByTeacherIdAsync(Guid teacherId);

        Task<Exercise?> GetExerciseByIdAsync(Guid exerciseId);
        Task<Exercise> UpdateExerciseAsync(Exercise exercise, byte[] rowVersion);
    }
}
