using Exercise_Application.DTO;
using Exercise_Application.Helper;
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

        public async Task<ExerciseDTO> RemoveExerciseSolutionAsync(Guid exerciseId)
        {
            try
            {
                var exercise = await _repository.GetByIdAsync(exerciseId);
                if (exercise == null) { throw new KeyNotFoundException(nameof(exercise)); }

                exercise.RemoveSolution();

                exercise = await _repository.UpdateAsync(exercise, exercise.RowVersion);
                return Mapper.MapToDTO(exercise);

            }
            catch (Exception ex)
            {
                throw new Exception("An error occured while removing solution from exercise.", ex);
            }
        }

        public async Task<ExerciseDTO> SetExerciseSolution(Guid exerciseId, CreateExerciseSolutionRequest solution)
        {
            try
            {
                var exercise = await _repository.GetByIdAsync(exerciseId);
                if (exercise == null) { throw new KeyNotFoundException(nameof(exercise)); }

                exercise.SetSolution(solution.Content, solution.VideoUrl);
                exercise = await _repository.UpdateAsync(exercise, exercise.RowVersion);
                return Mapper.MapToDTO(exercise);

            }
            catch (Exception ex)
            {
                throw new Exception("An error occured while setting solution for exercise.", ex);
            }
        }
    }
}


