using Exercise_Application.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exercise_Infrastructure.DataAccess
{
    public class ExerciseSolutionRepository : IExerciseSolutionRepository
    {
        private DbContext dbContext;

        public ExerciseSolutionRepository(DbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<ExerciseSolution> AddExerciseSolutionAsync(ExerciseSolution exerciseSolution)
        {
            throw new NotImplementedException();
        }

        public async Task<ExerciseSolution> GetExerciseSolutionByIdAsync(Guid solutionId)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<ExerciseSolution>> GetExerciseSolutionsByExerciseIdAsync(Guid exerciseId)
        {
            throw new NotImplementedException();
        }

        public async Task<ExerciseSolution> UpdateExerciseSolutionAsync(Guid solutionId, byte[] rowVersion)
        {
            throw new NotImplementedException();
        }
    }
}
