using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exercise_Application.Interfaces.Repositories
{
    public interface IExerciseSolutionRepository
    {
        Task<ExerciseSolution> AddExerciseSolutionAsync(ExerciseSolution exerciseSolution);
        Task<ExerciseSolution> UpdateExerciseSolutionAsync(Guid solutionId, byte[] rowVersion);
        Task<ExerciseSolution> GetExerciseSolution(Guid solutionId);
    }
}
