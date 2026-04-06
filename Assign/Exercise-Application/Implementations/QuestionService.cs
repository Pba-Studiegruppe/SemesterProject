using Exercise_Application.DTO;
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
        public Task<QuestionDTO> CreateQuestionAsync(CreateQuestionRequest dto)
        {
            throw new NotImplementedException();
        }

        public Task<QuestionDTO?> GetQuestionByIdAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<QuestionDTO>> GetQuestionsByExerciseIdAsync(Guid exerciseId)
        {
            throw new NotImplementedException();
        }
    }
}
