using Exercise_Application.DTO;

namespace Exercise_Application.Interfaces.Services
{
    public interface IQuestionService
    {
        Task<QuestionDTO> CreateQuestionAsync(CreateQuestionRequest dto);
        Task<QuestionDTO?> GetQuestionByIdAsync(Guid id);
        Task<IEnumerable<QuestionDTO>> GetQuestionsByExerciseIdAsync(Guid exerciseId);
        Task<QuestionDTO> UpdateQuestionAsync(UpdateQuestionRequest dto);
    }
}