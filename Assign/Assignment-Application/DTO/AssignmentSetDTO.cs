using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment_Application.DTO
{
    public class AssignmentSetDTO
    {
        public Guid Id { get; set; }
        public Guid? CourseId { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public bool IsPublished { get; set; }
        public bool GradingPublished { get; set; }
        public bool Inactive { get; set; }
        public DateOnly? CreatedAt { get; set; }
        public List<AssignmentDTO> Assignments { get; set; } = [];
    }

    public class CreateAssignmentSetRequest
    {
        public Guid CourseId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
    }

    public class UpdateAssignmentSetRequest
    {
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public bool Inactive { get; set; }
    }
}
