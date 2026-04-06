using Exercise_Application.Interfaces.Repositories;
using Exercise_Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exercise_Infrastructure.DataAccess
{
    public class QuestionSolutionRepository : IQuestionSolutionRepository
    {
        public void AddQuestionSolution(QuestionSolution questionSolution)
        {
            throw new NotImplementedException();
        }

        public void GetQuestionSolutionById(Guid solutionId)
        {
            throw new NotImplementedException();
        }

        public void GetQuestionSolutionsByQuestionId(Guid questionId)
        {
            throw new NotImplementedException();
        }

        public void UpdateQuestionSolution(QuestionSolution solution, byte[] rowVersion)
        {
            throw new NotImplementedException();
        }
    }
}
