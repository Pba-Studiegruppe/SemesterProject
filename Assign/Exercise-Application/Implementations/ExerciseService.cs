using Exercise_Application.DTO;
using Exercise_Application.Interfaces.Repositories;
using Exercise_Application.Interfaces.Services;
using Exercise_Application.Helper;
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
            try
            {
                var exercise = new Exercise(dto.Title, dto.Content, dto.CreatedByTeacherId);
                
                foreach (var q in dto.Questions.Where(q => q != null))
                {
                    exercise.AddQuestion(q!.Title, q.Content);

                    if (q.Solution != null)
                    {
                        exercise.Questions.Last().SetSolution(q.Solution.Content);
                    }
                }

                foreach (var k in dto.ExerciseKeywords.Where(k => k != null))
                {
                    exercise.AddKeyword(k!.KeywordId);
                }

                if (dto.Solution != null)
                {
                    exercise.SetSolution(dto.Solution.Content, dto.Solution.VideoUrl);
                }

                var result = await _repository.AddAsync(exercise);

                if (result == null) { throw new Exception("Failed to create the exercise."); }

                return Mapper.MapToDTO(result);
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
                var exercises = await _repository.GetByKeywordsAsync(keywordIds);
                if (exercises == null) { return Enumerable.Empty<ExerciseDTO?>(); }

                return exercises.Select(e => Mapper.MapToDTO(e));
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
                var exercise = await _repository.GetByIdAsync(id);
                if (exercise == null) { return null; }
                return Mapper.MapToDTO(exercise);
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
                var exercises = await _repository.GetByTeacherIdAsync(teacherId);
                if (exercises == null) { return Enumerable.Empty<ExerciseDTO?>(); }
                return exercises.Select(e => Mapper.MapToDTO(e));
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while retrieving exercises by teacher ID.", ex);
            }
        }
        public async Task<ExerciseDTO?> UpdateExerciseAsync(UpdateExerciseRequest dto)
        {
            try
            {
                var existingExercise = await _repository.GetByIdAsync(dto.Id);
                if (existingExercise == null) { return null; }

                existingExercise.UpdateTitle(dto.Title);
                existingExercise.UpdateContent(dto.Content);

                var result = await _repository.UpdateAsync(existingExercise, existingExercise.RowVersion);
                return Mapper.MapToDTO(result);
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while updating the exercise.", ex);
            }
        }

        public async Task<IEnumerable<ExerciseDTO>> GetAllExercisesAsync(string? search = null)
        {
            try
            {
                var exercises = await _repository.GetAllAsync(search);
                return exercises.Select(Mapper.MapToDTO);
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while retrieving exercises.", ex);
            }
        }
    }
}
