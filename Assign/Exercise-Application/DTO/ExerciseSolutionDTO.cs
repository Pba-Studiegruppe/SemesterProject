using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exercise_Application.DTO
{
    public class ExerciseSolutionDTO
    {
        public Guid Id { get; set; }
        public Guid ExerciseId { get; set; }
        public string Content { get; set; } = string.Empty;
        public string? VideoUrl { get; set; }
    }

    public class CreateExerciseSolutionRequest
    {
        public Guid ExerciseId { get; set; }
        public string Content { get; set; } = string.Empty;
        public string? VideoUrl { get; set; }
    }
}
