using Assignment_Domain.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment_Domain.Entities
{
    public class Assignment : Entity
    {
        public string Title{ get; private set; } //Tilføjet, mangler at komme på klasse diagrammet
        public DateTime CreatedAt { get; private set; } //Tilføjet, mangler at komme på klasse diagrammet

        public IReadOnlyCollection<AssignmentExercise> AssignmentExercises => _assignmentExercises;
        private readonly List<AssignmentExercise> _assignmentExercises = new List<AssignmentExercise>();

        public IReadOnlyCollection<SubmittedAssignment> SubmittedAssignments => _submittedAssignments;
        private readonly List<SubmittedAssignment> _submittedAssignments = new List<SubmittedAssignment>();

        private Assignment() : base(Guid.NewGuid()) { }

        //Skal vi have nogle flere attributter?
        // / Skal vi have en CreatedByTeacherId ligesom i Exercise?
        // Skal vi have en type, eksempelvis med eller uden hjælpemidler?
    }
}
