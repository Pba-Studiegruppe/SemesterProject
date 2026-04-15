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
    public class ExerciseKeywordService : IExerciseKeywordService
    {

        private IExerciseKeywordRepository _repository;

        public ExerciseKeywordService(IExerciseKeywordRepository repository)
        {
            _repository = repository;
        }

        public async Task<ExerciseKeyword> AddExerciseKeywordAsync(ExerciseKeywordDTO dto)
        {
            try
            {
                if (dto == null) { throw new ArgumentNullException(nameof(dto), "The ExerciseKeywordDTO cannot be null."); }
                if (dto.ExerciseId == Guid.Empty) { throw new ArgumentException("The ExerciseId cannot be empty.", nameof(dto.ExerciseId)); }
                if (dto.KeywordId == Guid.Empty) { throw new ArgumentException("The KeywordId cannot be empty.", nameof(dto.KeywordId)); }

                var exerciseKeyword = new ExerciseKeyword(dto.ExerciseId, dto.KeywordId);
                return await _repository.AddExerciseKeywordAsync(exerciseKeyword);

            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while adding the exercise keyword.", ex);
            }
        }

        public async Task<IEnumerable<ExerciseKeyword>> GetExerciseKeywordsByExerciseIdAsync(Guid exerciseId)
        {
            try
            {
                if (exerciseId == Guid.Empty) { throw new ArgumentException("The ExerciseId cannot be empty.", nameof(exerciseId)); }
                return await _repository.GetExerciseKeywordsByExerciseIdAsync(exerciseId);
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while retrieving exercise keywords by exercise ID.", ex);
            }

        }
    }
}
