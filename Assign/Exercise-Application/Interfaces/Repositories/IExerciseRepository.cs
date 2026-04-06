using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exercise_Application.Interfaces.Repositories
{
    public interface IExerciseRepository
    {
        void AddExercise(Exercise exercise);
        void GetExerciseById(Guid exerciseId);
        void UpdateExercise(Exercise exercise, byte[] rowVersion);
    }
}
