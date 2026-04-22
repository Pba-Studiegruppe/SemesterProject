using Exercise_Application.DTO;
using Exercise_Application.Interfaces.Repositories;

namespace Exercise_Application.Implementations
{
    public class ExerciseQuesitonService : IExerciseQuestionService
    {
        private readonly IExerciseRepository _repository;

        public ExerciseQuesitonService(IExerciseRepository repository)
        {
            _repository = repository;
        }

        public Task<ExerciseDTO> AddQuestionsAsync(List<QuestionDTO> questions)
        {
            throw new NotImplementedException();
        }

        public Task<ExerciseDTO> RemoveQuestionAsync(RemoveQuestionRequest dto)
        {
            throw new NotImplementedException();
        }

        public Task<ExerciseDTO> UpdateQuestionAsync(UpdateQuestionRequest dto)
        {
            throw new NotImplementedException();
        }
    }
}


