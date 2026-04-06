using Exercise_Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exercise_Application.Interfaces.Repositories
{
    public interface IQuestionRepository
    {
        Task<Question> AddQuestionAsync(Question question);
        Task<Question> UpdateQuestionAsync(Guid questionId);
        Task<Question> GetQuestionByIdAsync(Guid questionId);
        Task<IEnumerable<Question>> GetQuestionsByExerciseIdAsync(Guid exerciseId);
        Task<Question> UpdateQuestionAsync(Question question, byte[] rowVersion);
    }
}
