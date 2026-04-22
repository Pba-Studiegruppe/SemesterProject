using Exercise_Application.DTO;
using Exercise_Application.Interfaces.Repositories;

namespace Exercise_Application.Implementations
{
    public class ExerciseSolutionService : IExerciseSolutionService
    {
        private readonly IExerciseRepository _repository;
        public ExerciseSolutionService(IExerciseRepository repository)
        {
            _repository = repository;
        }

        public Task<ExerciseDTO> RemoveExerciseSolutionAsync(Guid exerciseId)
        {
            throw new NotImplementedException();
        }

        public Task<ExerciseDTO> SetExerciseSolution(ExerciseSolutionDTO solution)
        {
            throw new NotImplementedException();
        }

        public Task<ExerciseDTO> UpdateExerciseSolutionAsync(UpdateExerciseSolutionRequest dto)
        {
            throw new NotImplementedException();
        }
    }
}


