using Exercise_Application.DTO;

namespace Exercise_Application.Interfaces.Services
{
    public interface IQuestionSolutionService
    {
        Task<QuestionSolutionDTO> CreateQuestionSolutionAsync(CreateQuestionSolutionRequest dto);
        Task<QuestionSolutionDTO?> GetQuestionSolutionByQuestionIdAsync(Guid questionId);
        Task<QuestionSolutionDTO?> GetQuestionSolutionByIdAsync(Guid id);
        Task<QuestionSolutionDTO> UpdateQuestionSolutionAsync(Guid id, UpdateQuestionSolutionRequest dto, byte[] rowVersio);
    }
}