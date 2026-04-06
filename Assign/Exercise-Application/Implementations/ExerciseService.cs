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
    public class ExerciseService : IExerciseService
    {
        private readonly IExerciseRepository _repository;

        public ExerciseService(IExerciseRepository repository)
        {
            _repository = repository;
        }


        public Task<ExerciseDTO> CreateExerciseAsync(CreateExerciseRequest dto)
        {
            var exercise = new Exercise(dto.Title, dto.Content, dto.CreatedByTeacherId);

            _repository.AddExerciseAsync(exercise);

            return Task.FromResult(MapToDTO(exercise));
        }

        public Task<IEnumerable<ExerciseDTO?>> GetExerciseByExerciseKeywords(List<Guid> keywordIds)
        {
            var exercises = _repository.GetExercisesByKeywordsAsync(keywordIds);
            if (exercises == null) {return Task.FromResult(Enumerable.Empty<ExerciseDTO?>());}

            return Task.FromResult(exercises.Result.Select(e => MapToDTO(e)));
        }

        public Task<ExerciseDTO?> GetExerciseByIdAsync(Guid id)
        {
            var exercise = _repository.GetExerciseByIdAsync(id);
            if (exercise == null) { return Task.FromResult<ExerciseDTO?>(null); }

            return Task.FromResult<ExerciseDTO?>(MapToDTO(exercise.Result));

        }

        public Task<IEnumerable<ExerciseDTO?>> GetExercisesByTeacherIdAsync(Guid teacherId)
        {
            throw new NotImplementedException();
        }

        public ExerciseDTO MapToDTO(Exercise exercise)
        {
            return new ExerciseDTO
            {
                Id = exercise.Id,
                Title = exercise.Title,
                Content = exercise.Content,
                CreatedAt = exercise.CreatedAt,
                CreatedByTeacherId = exercise.CreatedByTeacherId,
                Questions = exercise.Questions.Select(q => new QuestionDTO
                {
                    Id = q.Id,
                    Title = q.Title,
                    Content = q.Content
                }).ToList(),
                ExerciseKeywords = exercise.ExerciseKeywords.Select(ek => new ExerciseKeywordDTO
                {
                    ExerciseId = ek.ExerciseId,
                    KeywordId = ek.KeywordId
                }).ToList(),
                Solution = exercise.Solution != null ? new ExerciseSolutionDTO
                {
                    Id = exercise.Solution.Id,
                    Content = exercise.Solution.Content
                } : null
            };
        }
    }
}
