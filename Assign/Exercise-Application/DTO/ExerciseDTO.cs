using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exercise_Application.DTO
{
    public class ExerciseDTO
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public Guid CreatedByTeacherId { get; set; }
        public List<QuestionDTO> Questions { get; set; } = new();
        public List<ExerciseKeywordDTO> ExerciseKeywords { get; set; } = new();
        public ExerciseSolutionDTO? Solution { get; set; }
        public byte[]? RowVersion { get; set; }
    }

    public class CreateExerciseRequest
    {
        public string Title { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public Guid CreatedByTeacherId { get; set; }
        public List<QuestionDTO> Questions { get; set; } = new();
        public List<ExerciseKeywordDTO> ExerciseKeywords { get; set; } = new();
        public ExerciseSolutionDTO? Solution { get; set; }
    }

    public class UpdateExerciseRequest
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public List<QuestionDTO> Questions { get; set; } = new();
        public List<ExerciseKeywordDTO> ExerciseKeywords { get; set; } = new();
        public ExerciseSolutionDTO? Solution { get; set; }
        public byte[]? RowVersion { get; set; }
    }
}
