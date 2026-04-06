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
        public Task<Question> AddQuestionAsync(Question question)
        {
            throw new NotImplementedException();
        }

        public Task<Question> GetQuestionByIdAsync(Guid questionId)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Question>> GetQuestionsByExerciseIdAsync(Guid exerciseId)
        {
            throw new NotImplementedException();
        }

        public Task<Question> UpdateQuestionAsync(Guid questionId)
        {
            throw new NotImplementedException();
        }

        public Task<Question> UpdateQuestionAsync(Question question, byte[] rowVersion)
        {
            throw new NotImplementedException();
        }
    }
}
