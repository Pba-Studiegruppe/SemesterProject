using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exercise_Application.Interfaces.Repositories
{
    public interface IExerciseSolutionRepository
    {
        void AddExerciseSolution(ExerciseSolution exerciseSolution);
        void DeleteExerciseSolution(Guid solutionId, byte[] rowVersion);
        void GetExerciseSolution(Guid solutionId);
    }
}
