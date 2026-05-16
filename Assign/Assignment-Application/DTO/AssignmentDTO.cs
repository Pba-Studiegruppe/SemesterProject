using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment_Application.DTO
{
    public class AssignmentDTO
    {
        public Guid Id { get; set; }
        public Guid? AssignmentSetId { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public int? TotalPoints { get; set; }
        public DateOnly? CreatedAt { get; set; }
        public List<AssignmentExerciseDTO> Exercises { get; set; } = [];
    }

    public class CreateAssignmentRequest
    {
        public Guid? AssignmentSetId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
    }

    public class UpdateAssignmentRequest
    {
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
    }
}
