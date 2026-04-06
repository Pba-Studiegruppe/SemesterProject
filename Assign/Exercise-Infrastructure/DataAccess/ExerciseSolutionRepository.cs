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
        public void AddExerciseSolution(ExerciseSolution exerciseSolution)
        {
            throw new NotImplementedException();
        }

        public void DeleteExerciseSolution(Guid solutionId, byte[] rowVersion)
        {
            throw new NotImplementedException();
        }

        public void GetExerciseSolution(Guid solutionId)
        {
            throw new NotImplementedException();
        }
    }
}
