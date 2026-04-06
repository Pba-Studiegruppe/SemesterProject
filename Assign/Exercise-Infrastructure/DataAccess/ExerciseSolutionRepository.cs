using Exercise_Application.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exercise_Infrastructure.DataAccess
{
    public class ExerciseSolutionRepository : IExerciseSolutionRepository
    {
        public Task<ExerciseSolution> AddExerciseSolutionAsync(ExerciseSolution exerciseSolution)
        {
            throw new NotImplementedException();
        }

        public Task<ExerciseSolution> GetExerciseSolution(Guid solutionId)
        {
            throw new NotImplementedException();
        }

        public Task<ExerciseSolution> UpdateExerciseSolutionAsync(Guid solutionId, byte[] rowVersion)
        {
            throw new NotImplementedException();
        }
    }
}
