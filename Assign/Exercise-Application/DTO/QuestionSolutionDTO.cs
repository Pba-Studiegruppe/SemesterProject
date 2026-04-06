using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exercise_Application.DTO
{
    public class QuestionSolutionDTO
    {
        public Guid Id { get; set; }
        public Guid QuestionId { get; set; }
        public string Content { get; set; } = string.Empty;
    }

    public class CreateQuestionSolutionRequest
    {
        public Guid QuestionId { get; set; }
        public string Content { get; set; } = string.Empty;
    }
}
