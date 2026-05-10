using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment_Application.DTO
{
    public class AssignmentExerciseDTO
    {
        public Guid AssignmentId { get; set; }
        public Guid ExerciseId { get; set; }
    }

    public class CreateAssignmentExerciseRequest
    {
        public Guid ExerciseId { get; set; }
    }

    public class RemoveAssignmentExerciseRequest
    {
        public Guid ExerciseId { get; set; }
    }
}
