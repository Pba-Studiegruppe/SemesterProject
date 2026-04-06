using Exercise_Application.Interfaces.Services;
using Exercise_Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exercise_Application.Implementations
{
    public class QuestionService : IQuestionService
    {
        public void CreateQuestion(string v, Guid guid)
        {
            throw new NotImplementedException();
        }

        public void DeleteQuestion(Guid questionId)
        {
            throw new NotImplementedException();
        }

        public void GetQuestion(Guid questionId)
        {
            throw new NotImplementedException();
        }

        public void GetQuestionsByExerciseId(Guid exerciseId)
        {
            throw new NotImplementedException();
        }

        public void UpdateQuestion(Question question, byte[] rowVersion)
        {
            throw new NotImplementedException();
        }
    }
}
