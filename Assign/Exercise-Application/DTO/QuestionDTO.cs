using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exercise_Application.DTO
{
    public class QuestionDTO
    {
        public Guid Id { get; set; }
        public Guid ExerciseId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public QuestionSolutionDTO? Solution { get; set; }
        public byte[]? RowVersion { get; set; }
    }
    public class CreateQuestionRequest
    {
        public Guid ExerciseId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
    }

    public class UpdateQuestionRequest
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public byte[]? RowVersion { get; set; }
    }
}
