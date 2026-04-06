using Exercise_Application.Interfaces.Repositories;
using Exercise_Application.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exercise_Application.Implementations
{
    public class QuestionSolutionService : IQuestionSolutionService
    {
        private IQuestionSolutionRepository @object;

        public QuestionSolutionService(IQuestionSolutionRepository @object)
        {
            this.@object = @object;
        }

        public void CreateQuestionSolution(string v, Guid guid1, Guid guid2)
        {
            throw new NotImplementedException();
        }

        public void GetQuestionSolution(Guid solutionId)
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
