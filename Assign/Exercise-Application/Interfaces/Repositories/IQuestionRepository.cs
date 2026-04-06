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
        void AddQuestion(Question question);
        void DeleteQuestion(Guid questionId);
        void GetQuestionById(Guid questionId);
        void GetQuestionsByExerciseId(Guid exerciseId);
        void UpdateQuestion(Question question, byte[] rowVersion);
    }
}
