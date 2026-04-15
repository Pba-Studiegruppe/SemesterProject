using Exercise_Application.Interfaces.Repositories;
using Exercise_Domain.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exercise_Infrastructure.DataAccess
{
    public class QuestionSolutionRepository : IQuestionSolutionRepository
    {
        private DbContext dbContext;
        public QuestionSolutionRepository(DbContext dbContext)
        {
            this.dbContext = dbContext;
        }
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
