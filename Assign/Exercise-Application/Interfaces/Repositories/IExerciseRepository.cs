using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Exercise_Application.DTO;
using Exercise_Domain.Entities;

namespace Exercise_Application.Interfaces.Repositories
{
    public interface IExerciseRepository
    {
        Task<Exercise> AddAsync(Exercise exercise);
        Task<Exercise?> GetByIdAsync(Guid exerciseId);
        Task<IEnumerable<Exercise>> GetByTeacherIdAsync(Guid teacherId);
        Task<IEnumerable<Exercise>> GetByKeywordsAsync(IEnumerable<Guid> keywordIds);
        Task<Exercise> UpdateAsync(Exercise exercise, byte[] rowVersion);
        Task<Exercise?> GetForSnapshotAsync(Guid exerciseId);
        Task<Exercise?> GetForEvaluationAsync(Guid exerciseId);
    }
}
