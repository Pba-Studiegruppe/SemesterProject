using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exercise_Application.Interfaces.Repositories
{
    public interface IQuestionSolutionRepository
    {
        void AddQuestionSolution(QuestionSolution questionSolution);
        void GetQuestionSolutionById(Guid solutionId);
        void GetQuestionSolutionsByQuestionId(Guid questionId);
        void UpdateQuestionSolution(QuestionSolution solution, byte[] rowVersion);
    }
}
