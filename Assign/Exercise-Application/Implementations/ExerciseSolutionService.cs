using Exercise_Application.DTO;
using Exercise_Application.Interfaces.Repositories;
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
        private readonly IExerciseSolutionRepository _repository;

        public ExerciseSolutionService(IExerciseSolutionRepository repository)
        {
            _repository = repository;
        }

        public async Task<ExerciseSolutionDTO> CreateExerciseSolutionAsync(CreateExerciseSolutionRequest dto)
        {
            throw new NotImplementedException();
        }

        public async Task<ExerciseSolutionDTO?> GetExerciseSolutionByIdAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<ExerciseSolutionDTO?>> GetExerciseSolutionsByExerciseIdAsync(Guid exerciseId)
        {
            throw new NotImplementedException();
        }
    }
}
