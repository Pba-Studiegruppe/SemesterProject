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
        public void AddQuestion(Question question)
        {
            throw new NotImplementedException();
        }

        public void DeleteQuestion(Guid questionId)
        {
            throw new NotImplementedException();
        }

        public void GetQuestionById(Guid questionId)
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
