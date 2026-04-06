using Exercise_Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exercise_Application.Interfaces.Repositories
{
    public interface IQuestionSolutionRepository
    {
        Task<QuestionSolution> AddQuestionSolutionAsync(QuestionSolution questionSolution);
        Task<QuestionSolution> GetQuestionSolutionByIdAsync(Guid solutionId);
        Task<QuestionSolution> GetQuestionSolutionsByQuestionIdAsync(Guid questionId);
        Task<QuestionSolution> UpdateQuestionSolutionAsync(QuestionSolution solution, byte[] rowVersion);
    }
}
