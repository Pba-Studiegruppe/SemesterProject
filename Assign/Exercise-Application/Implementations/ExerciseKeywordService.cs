using Exercise_Application.DTO;
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

        public Task<ExerciseDTO> AddExerciseKeywordsAsync(List<ExerciseKeywordDTO> exerciseKeywords)
        {
            throw new NotImplementedException();
        }

        public Task<ExerciseDTO> RemoveExerciseKeywordAsync(RemoveExerciseKeywordRequest dto)
        {
            throw new NotImplementedException();
        }
    }
}


