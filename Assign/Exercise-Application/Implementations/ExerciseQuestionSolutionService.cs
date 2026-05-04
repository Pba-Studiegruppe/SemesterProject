using Exercise_Application.DTO;
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

        public Task<ExerciseDTO> RemoveQuestionSolutionAsync(Guid questionId)
        {
            throw new NotImplementedException();
        }

        public Task<ExerciseDTO> UpdateQuestionSolutionAsync(UpdateQuestionSolutionRequest dto)
        {
            throw new NotImplementedException();
        }
    }
}


