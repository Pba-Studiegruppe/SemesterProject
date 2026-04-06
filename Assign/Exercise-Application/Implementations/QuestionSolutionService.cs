using Exercise_Application.DTO;
using Exercise_Application.Interfaces.Repositories;
using Exercise_Application.Interfaces.Services;
using Exercise_Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exercise_Application.Implementations
{
    public class QuestionSolutionService : IQuestionSolutionService
    {
        public Task<QuestionSolutionDTO> CreateQuestionSolutionAsync(CreateQuestionSolutionRequest dto)
        {
            throw new NotImplementedException();
        }

        public Task<QuestionSolutionDTO?> GetQuestionSolutionByIdAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<QuestionSolutionDTO?>> GetQuestionSolutionsByQuestionIdAsync(Guid questionId)
        {
            throw new NotImplementedException();
        }
    }
}
