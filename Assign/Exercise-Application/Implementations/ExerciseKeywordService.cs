using Exercise_Application.DTO;
using Exercise_Application.Helper;
using Exercise_Application.Interfaces.Repositories;

namespace Exercise_Application.Implementations
{
    public class ExerciseKeywordService : IExerciseKeywordService
    {
        private readonly IExerciseRepository _repository;
        public ExerciseKeywordService(IExerciseRepository repository)
        {
            _repository = repository;
        }

        public async Task<ExerciseDTO> AddExerciseKeywordsAsync(Guid ExerciseId, List<CreateExerciseKeywordRequest> exerciseKeywords)
        {
            try
            {
                var exercise = await _repository.GetByIdAsync(ExerciseId);
                if (exercise == null) { throw new KeyNotFoundException(nameof(exercise)); }

                var existingKeywordIds = exercise.ExerciseKeywords.Select(ek => ek.KeywordId).ToHashSet();

                foreach (var exerciseKeyword in exerciseKeywords)
                {
                    if (!existingKeywordIds.Contains(exerciseKeyword.KeywordId))
                    {
                        exercise.AddKeyword(exerciseKeyword.KeywordId);
                        existingKeywordIds.Add(exerciseKeyword.KeywordId); // keep in sync
                    }
                }

                exercise = await _repository.UpdateAsync(exercise, exercise.RowVersion);

                return Mapper.MapToDTO(exercise);
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while adding keyword to the exercise.", ex);
            }
        }

        public async Task<ExerciseDTO> RemoveExerciseKeywordAsync(Guid ExerciseId, RemoveExerciseKeywordRequest dto)
        {
            try
            {
                var exercise = await _repository.GetByIdAsync(ExerciseId);
                if (exercise == null) { throw new KeyNotFoundException(nameof(exercise)); }

              
                exercise.RemoveKeyword(dto.KeywordId);
                exercise = await _repository.UpdateAsync(exercise,exercise.RowVersion);

                return Mapper.MapToDTO(exercise);

            }
            catch (Exception ex)
            {
                throw new Exception("An error occured while removing keyword from exercise.", ex);
            }
        }
    }
}


