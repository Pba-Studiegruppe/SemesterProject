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
        public async Task<QuestionSolution> AddQuestionSolutionAsync(QuestionSolution questionSolution)
        {
            throw new NotImplementedException();
        }

        public async Task<QuestionSolution> GetQuestionSolutionByIdAsync(Guid solutionId)
        {
            throw new NotImplementedException();
        }

        public async Task<QuestionSolution> GetQuestionSolutionByQuestionIdAsync(Guid questionId)
        {
            throw new NotImplementedException();
        }

        public async Task<QuestionSolution> UpdateQuestionSolutionAsync(QuestionSolution solution, byte[] rowVersion)
        {
            throw new NotImplementedException();
        }
    }
}
