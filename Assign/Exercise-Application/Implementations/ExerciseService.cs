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


        public async Task<ExerciseDTO> CreateExerciseAsync(CreateExerciseRequest dto)
        {
            try
            {
                var exercise = new Exercise(dto.Title, dto.Content, dto.CreatedByTeacherId);
                var result = await _repository.AddExerciseAsync(exercise);

                if(result == null) { throw new Exception("Failed to create the exercise."); }

                return MapToDTO(result);
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while creating the exercise.", ex);
            }
        }

        public async Task<IEnumerable<ExerciseDTO?>> GetExerciseByExerciseKeywords(List<Guid> keywordIds)
        {
            try
            {
                var exercises = await _repository.GetExercisesByKeywordsAsync(keywordIds);
                if (exercises == null) { return Enumerable.Empty<ExerciseDTO?>(); }

                return exercises.Select(e => MapToDTO(e));
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while retrieving exercises by keywords.", ex);
            }
        }

        public async Task<ExerciseDTO?> GetExerciseByIdAsync(Guid id)
        {
            try
            {
                var exercise = await _repository.GetExerciseByIdAsync(id);
                if (exercise == null) { return null; }
                return MapToDTO(exercise);
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while retrieving the exercise by ID.", ex);
            }
        }

        public async Task<IEnumerable<ExerciseDTO?>> GetExercisesByTeacherIdAsync(Guid teacherId)
        {
            try
            {
                var exercises = await _repository.GetExercisesByTeacherIdAsync(teacherId);
                if (exercises == null) { return Enumerable.Empty<ExerciseDTO?>(); }
                return exercises.Select(e => MapToDTO(e));
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while retrieving exercises by teacher ID.", ex);
            }

        }

        public async Task<ExerciseDTO?> UpdateExerciseAsync(Guid id, UpdateExerciseRequest dto)
        {
            try
            {
                var existingExercise = await _repository.GetExerciseByIdAsync(id);
                if (existingExercise == null) { return null; }

                existingExercise.Update(dto.Title, dto.Content);
                var result = await _repository.UpdateExerciseAsync(existingExercise, existingExercise.RowVersion);
                return MapToDTO(result);
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while updating the exercise.", ex);
            }
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
