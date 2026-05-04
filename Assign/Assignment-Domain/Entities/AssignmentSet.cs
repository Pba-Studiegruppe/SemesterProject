using Assignment_Domain.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment_Domain.Entities
{
    public class AssignmentSet: Entity
    {
        public string Title { get; private set; } //Tilføjet, mangler at komme på klasse diagrammet
        public Guid CourseId { get; private set; }
        public DateTime CreatedAt { get; private set; } //Tilføjet, mangler at komme på klasse diagrammet
        public Guid CreatedByTeacherId { get; private set; } //Tilføjet, mangler at komme på klasse diagrammet
       
        private readonly List<Assignment> _assignments = new();
        public IReadOnlyCollection<Assignment> Assignments => _assignments;
        private AssignmentSet() : base(Guid.NewGuid()) { }

        public GradeSheet GradeSheet { get; private set; }

        public AssignmentSet(string title, string description, Guid teacherId) : base(Guid.NewGuid())
        {
            Title = title;
            CreatedByTeacherId = teacherId;
            CreatedAt = DateTime.UtcNow;
        }
        public void AddAssignment(Assignment assignment)
        {
            if (assignment == null)
                throw new ArgumentNullException(nameof(assignment));

            _assignments.Add(assignment);
        }
    }
}
