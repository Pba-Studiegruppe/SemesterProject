using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exercise_Application.DTO
{
    public class ExerciseKeywordDTO
    {
        public Guid ExerciseId { get; set; }
        public Guid KeywordId { get; set; }
    }

    public class RemoveExerciseKeywordRequest
    {
        public Guid ExerciseId { get; set; }
        public Guid KeywordId { get; set; }
    }
}
