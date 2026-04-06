using Exercise_Application.DTO;

namespace Exercise_Application.Interfaces.Services
{
    public interface IQuestionSolutionService
    {
        Task<QuestionSolutionDTO> CreateQuestionSolutionAsync(CreateQuestionSolutionRequest dto);
        Task<IEnumerable<QuestionSolutionDTO?>> GetQuestionSolutionsByQuestionIdAsync(Guid questionId);
        Task<QuestionSolutionDTO?> GetQuestionSolutionByIdAsync(Guid id);
    }
}