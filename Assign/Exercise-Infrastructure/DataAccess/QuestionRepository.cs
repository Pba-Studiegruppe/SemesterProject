using Exercise_Application.Interfaces.Repositories;
using Exercise_Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exercise_Infrastructure.DataAccess
{
    public class QuestionRepository : IQuestionRepository
    {
        public async Task<Question> AddQuestionAsync(Question question)
        {
            throw new NotImplementedException();
        }

        public async Task<Question> GetQuestionByIdAsync(Guid questionId)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Question>> GetQuestionsByExerciseIdAsync(Guid exerciseId)
        {
            throw new NotImplementedException();
        }

        public async Task<Question> UpdateQuestionAsync(Question question, byte[] rowVersion)
        {
            throw new NotImplementedException();
        }
    }
}
