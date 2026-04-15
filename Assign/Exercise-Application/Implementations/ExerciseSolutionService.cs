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
    public class ExerciseSolutionService : IExerciseSolutionService
    {
        private readonly IExerciseSolutionRepository _repository;

        public ExerciseSolutionService(IExerciseSolutionRepository repository)
        {
            _repository = repository;
        }

        public async Task<ExerciseSolutionDTO> CreateExerciseSolutionAsync(CreateExerciseSolutionRequest dto)
        {
            try
            {
                if (dto == null) { throw new ArgumentNullException(nameof(dto), "The CreateExerciseSolutionRequest cannot be null."); }
                if (dto.ExerciseId == Guid.Empty) { throw new ArgumentException("The ExerciseId cannot be empty.", nameof(dto.ExerciseId)); }
                if (string.IsNullOrWhiteSpace(dto.Content)) { throw new ArgumentException("The Content cannot be null or whitespace.", nameof(dto.Content)); }
                var solution = new ExerciseSolution(dto.ExerciseId, dto.Content, dto.VideoUrl);
                var createdSolution = await _repository.AddExerciseSolutionAsync(solution);
                return new ExerciseSolutionDTO
                {
                    Id = createdSolution.Id,
                    ExerciseId = createdSolution.ExerciseId,
                    Content = createdSolution.Content,
                    VideoUrl = createdSolution.VideoUrl
                };
            }
            catch (Exception ex)
            {
                throw new ApplicationException("An error occurred while creating the exercise solution.", ex);
            }
        }

        public async Task<ExerciseSolutionDTO?> GetExerciseSolutionByIdAsync(Guid id)
        {
            try
            {
                var solution = await _repository.GetExerciseSolutionByIdAsync(id);
                if (solution == null)
                    return null;
                return new ExerciseSolutionDTO
                {
                    Id = solution.Id,
                    ExerciseId = solution.ExerciseId,
                    Content = solution.Content,
                    VideoUrl = solution.VideoUrl
                };
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"An error occurred while retrieving the exercise solution with ID {id}.", ex);
            }
        }

        public async Task<IEnumerable<ExerciseSolutionDTO?>> GetExerciseSolutionsByExerciseIdAsync(Guid exerciseId)
        {
            try
            {
                var solutions = await _repository.GetExerciseSolutionsByExerciseIdAsync(exerciseId);
                return solutions.Select(s => new ExerciseSolutionDTO
                {
                    Id = s.Id,
                    ExerciseId = s.ExerciseId,
                    Content = s.Content,
                    VideoUrl = s.VideoUrl
                });
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"An error occurred while retrieving exercise solutions for exercise ID {exerciseId}.", ex);
            }
        }


        public async Task<ExerciseSolutionDTO> UpdateExerciseSolutionAsync(Guid id, UpdateExerciseSolutionRequest dto, byte[] rowVersion)
        {
            throw new NotImplementedException();
        }
    }
}
