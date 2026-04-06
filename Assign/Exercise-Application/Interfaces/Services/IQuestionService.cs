using Exercise_Domain.Entities;

namespace Exercise_Application.Interfaces.Services
{
    public interface IQuestionService
    {
        void UpdateQuestion(Question question, byte[] rowVersion);
    }
}