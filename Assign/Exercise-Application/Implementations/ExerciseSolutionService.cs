using Exercise_Application.DTO;
using Exercise_Application.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exercise_Application.Implementations
{
    public class ExerciseSolutionService : IExerciseSolutionService
    {
        public Task<ExerciseSolutionDTO> CreateExerciseSolutionAsync(CreateExerciseSolutionRequest dto)
        {
            throw new NotImplementedException();
        }

        public Task<ExerciseSolutionDTO?> GetExerciseSolutionByIdAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<ExerciseSolutionDTO?>> GetExerciseSolutionsByExerciseIdAsync(Guid exerciseId)
        {
            throw new NotImplementedException();
        }
    }
}
