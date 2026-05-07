using Exercise_Application.DTO;
using Exercise_Application.Helper;
using Exercise_Application.Interfaces.Repositories;

namespace Exercise_Application.Implementations
{
    public class ExerciseQuestionSolutionService : IExerciseQuestionSolutionService
    {
        private readonly IExerciseRepository _repository;
        public ExerciseQuestionSolutionService(IExerciseRepository repository)
        {
            _repository = repository;
        }

        public async Task<ExerciseDTO> RemoveQuestionSolutionAsync(Guid exerciseId, RemoveQuestionSolutionRequest dto)
        {
            try
            {
                var exercise = await _repository.GetByIdAsync(exerciseId);
                if (exercise == null) throw new KeyNotFoundException($"Exercise with id {exerciseId} not found.");
                if (exercise.Questions.Count == 0 || exercise.Questions == null) throw new Exception("Exercise has no questions to remove solutions for");
                if (exercise.Questions.FirstOrDefault(q => q.Id == dto.QuestionId) == null) throw new KeyNotFoundException($"Question with id {dto.QuestionId} not found.");

                exercise.Questions.FirstOrDefault(q => q.Id == dto.QuestionId).RemoveSolution();

                var result = await _repository.UpdateAsync(exercise, exercise.RowVersion);

                return Mapper.MapToDTO(result);

            }
            catch (Exception ex)
            { throw new Exception("An error occurred while removing questionSolution for question.", ex); }
        }

        public async Task<ExerciseDTO> SetQuestionSolutionAsync(Guid exerciseId, CreateQuestionSolutionRequest dto)
        {
            try
            {
                var exercise = await _repository.GetByIdAsync(exerciseId);
                if (exercise == null) throw new KeyNotFoundException($"Exercise with id {exerciseId} not found.");
                if (exercise.Questions.Count == 0 || exercise.Questions == null) throw new Exception("Exercise has no questions to add solutions for");
                if (exercise.Questions.FirstOrDefault(q => q.Id == dto.QuestionId) == null) throw new KeyNotFoundException($"Question with id {dto.QuestionId} not found.");
                if (exercise.Questions.FirstOrDefault(q => q.Id == dto.QuestionId).Solution != null) throw new Exception($"Question with id {dto.QuestionId} already has a solotuion.");

                exercise.Questions.FirstOrDefault(q => q.Id == dto.QuestionId).SetSolution(dto.Content);

                var result = await _repository.UpdateAsync(exercise, exercise.RowVersion);

                return Mapper.MapToDTO(result);
            }
            catch (Exception ex)
            { throw new Exception("An error occurred while setting questionSolution for question.", ex); }
        }

        public async Task<ExerciseDTO> UpdateQuestionSolutionAsync(Guid exerciseId, UpdateQuestionSolutionRequest dto)
        {
            try
            {
                var exercise = await _repository.GetByIdAsync(exerciseId);
                if (exercise == null) throw new KeyNotFoundException($"Exercise with id {exerciseId} not found.");
                if (exercise.Questions.Count == 0 || exercise.Questions == null) throw new Exception("Exercise has no questions to add solutions for");
                if (exercise.Questions.FirstOrDefault(q => q.Id == dto.QuestionId) == null) throw new KeyNotFoundException($"Question with id {dto.QuestionId} not found.");

                exercise.Questions.FirstOrDefault(q => q.Id == dto.QuestionId).SetSolution(dto.Content);

                var result = await _repository.UpdateAsync(exercise, exercise.RowVersion);

                return Mapper.MapToDTO(result);
            }
            catch (Exception ex)
            { throw new Exception("An error occurred while updating questionSolution for question.", ex); }
        }
    }
}


