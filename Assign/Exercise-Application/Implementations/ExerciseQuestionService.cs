using Exercise_Application.DTO;
using Exercise_Application.Helper;
using Exercise_Application.Interfaces.Repositories;
using Exercise_Domain.Entities;

namespace Exercise_Application.Implementations
{
    public class ExerciseQuestionService : IExerciseQuestionService
    {
        private readonly IExerciseRepository _repository;

        public ExerciseQuestionService(IExerciseRepository repository)
        {
            _repository = repository;
        }

        public async Task<ExerciseDTO> AddQuestionsAsync(Guid exerciseId, List<CreateQuestionRequest> questions)
        {
            try
            {
                var exercise = await _repository.GetByIdAsync(exerciseId);
                if (exercise == null) throw new KeyNotFoundException($"Exercise with id {exerciseId} not found.");

                foreach (var q in questions.Where(q => q != null))
                {
                    exercise.AddQuestion(q!.Title, q.Content);

                    if (q.Solution != null)
                    {
                        exercise.Questions.Last().SetSolution(q.Solution.Content);
                    }
                }

                var result = await _repository.UpdateAsync(exercise, exercise.RowVersion);

                return Mapper.MapToDTO(result);

            }
            catch (Exception ex)
            { throw new Exception("An error occurred while adding questions to exercise.", ex); }
        }

        public async Task<ExerciseDTO> RemoveQuestionAsync(Guid exerciseId, RemoveQuestionRequest dto)
        {
            try
            {
                var exercise = await _repository.GetByIdAsync(exerciseId);
                if (exercise == null) throw new KeyNotFoundException($"Exercise with id {exerciseId} not found.");
                if (exercise.Questions.Count == 0 || exercise.Questions == null) throw new Exception("Exercise has no questions to remove");
                if (exercise.Questions.FirstOrDefault(q => q.Id == dto.Id) == null) throw new KeyNotFoundException($"Question with id {dto.Id} not found.");

                exercise.RemoveQuestion(dto.Id);

                var result = await _repository.UpdateAsync(exercise, exercise.RowVersion);

                return Mapper.MapToDTO(result);

            }
            catch (Exception ex)
            {
                { throw new Exception("An error occurred while removing question", ex); }
            }
        }

        public async Task<ExerciseDTO> UpdateQuestionAsync(Guid exerciseId, UpdateQuestionRequest dto)
        {
            try
            {
                var exercise = await _repository.GetByIdAsync(exerciseId);
                if (exercise == null) throw new KeyNotFoundException($"Exercise with id {exerciseId} not found.");
                if (exercise.Questions.Count == 0 || exercise.Questions == null) throw new Exception("Exercise has no questions to update");

                if (exercise.Questions.FirstOrDefault(q => q.Id == dto.Id) == null) throw new KeyNotFoundException($"Question with id {dto.Id} not found.");

                exercise.Questions.FirstOrDefault(q => q.Id == dto.Id).Update(dto.Title, dto.Content);

                var result = await _repository.UpdateAsync(exercise, exercise.RowVersion);

                return Mapper.MapToDTO(result);

            }
            catch (Exception ex)
            { throw new Exception("An error occurred while updating question", ex); }
        }
    }
}


