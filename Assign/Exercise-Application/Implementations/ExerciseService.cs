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
    public class ExerciseService : IExerciseService
    {
        private readonly IExerciseRepository _repository;

        public ExerciseService(IExerciseRepository repository)
        {
            _repository = repository;
        }

        public Task<ExerciseDTO> SetExerciseSolution(ExerciseSolutionDTO ExerciseSolution)
        {
            throw new NotImplementedException();
        }

        public Task<ExerciseDTO> UpdateQuestionAsync(UpdateQuestionRequest dto)
        {
            throw new NotImplementedException();
        }

        public Task<ExerciseDTO> UpdateExerciseSolutionAsync(UpdateExerciseSolutionRequest dto)
        {
            throw new NotImplementedException();
        }

        public Task<ExerciseDTO> UpdateQuestionSolutionAsync(UpdateQuestionSolutionRequest dto)
        {
            throw new NotImplementedException();
        }

        public Task<ExerciseDTO> RemoveQuestionAsync(RemoveQuestionRequest dto)
        {
            throw new NotImplementedException();
        }

        public Task<ExerciseDTO> RemoveExerciseSolutionAsync(Guid exerciseId)
        {
            throw new NotImplementedException();
        }

        public Task<ExerciseDTO> RemoveQuestionSolutionAsync(Guid questionId)
        {
            throw new NotImplementedException();
        }

        public Task<ExerciseDTO> RemoveExerciseKeywordAsync(RemoveExerciseKeywordRequest dto)
        {
            throw new NotImplementedException();
        }

        public Task<ExerciseDTO> CreateExerciseAsync(CreateExerciseRequest dto)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<ExerciseDTO?>> GetExerciseByExerciseKeywords(List<Guid> keywordIds)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<ExerciseDTO?>> GetExercisesByTeacherIdAsync(Guid teacherId)
        {
            throw new NotImplementedException();
        }

        public Task<ExerciseDTO?> GetExerciseByIdAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<ExerciseDTO?> UpdateExerciseAsync(UpdateExerciseRequest dto)
        {
            throw new NotImplementedException();
        }

        public Task<ExerciseDTO> AddQuestionsAsync(List<QuestionDTO> Questions)
        {
            throw new NotImplementedException();
        }

        public Task<ExerciseDTO> AddExerciseKeywordsAsync(List<ExerciseKeywordDTO> ExerciseKeywords)
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
