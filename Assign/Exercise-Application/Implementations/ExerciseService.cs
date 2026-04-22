using Exercise_Application.DTO;
using Exercise_Application.Interfaces.Repositories;
using Exercise_Application.Interfaces.Services;
using Exercise_Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exercise_Application.Implementations
{
    public partial class ExerciseService : IExerciseService
    {
        private readonly IExerciseRepository _repository;

        public ExerciseService(IExerciseRepository repository)
        {
            _repository = repository;
        }

        public async Task<ExerciseDTO> CreateExerciseAsync(CreateExerciseRequest dto)
        {
            try { var exercise = new Exercise(dto.Title, dto.Content, dto.CreatedByTeacherId); var createdExercise = await _repository.AddExerciseAsync(exercise); return MapToDTO(createdExercise); } catch { throw new Exception(); }
        }

        public Task<IEnumerable<ExerciseDTO?>> GetExerciseByExerciseKeywords(List<Guid> keywordIds)
        {
            throw new NotImplementedException();
        }

        public Task<ExerciseDTO?> GetExerciseByIdAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<ExerciseDTO?>> GetExercisesByTeacherIdAsync(Guid teacherId)
        {
            throw new NotImplementedException();
        }
        public Task<ExerciseDTO?> UpdateExerciseAsync(UpdateExerciseRequest dto)
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
                    Content = exercise.Solution.Content,
                    VideoUrl = exercise.Solution.VideoUrl
                } : null
            };
        }
    }
}
