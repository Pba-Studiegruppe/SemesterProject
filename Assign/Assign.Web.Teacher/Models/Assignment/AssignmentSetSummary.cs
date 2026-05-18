using System;
using System.Collections.Generic;

namespace Assign.Web.Teacher.Models.AssignmentSet
{
    /// <summary>
    /// Mirrors AssignmentSetDTO from Assignment-Application. Drives both the
    /// list page (where only the top-level fields and Assignments.Count are
    /// used) and the detail page (where the nested assignment data is shown).
    /// </summary>
    public class AssignmentSetSummary
    {
        public Guid Id { get; set; }
        public Guid? CourseId { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public bool IsPublished { get; set; }
        public bool GradingPublished { get; set; }
        public bool Inactive { get; set; }
        public DateOnly? CreatedAt { get; set; }
        public List<AssignmentRef> Assignments { get; set; } = new();

        public int AssignmentCount => Assignments.Count;
    }

    /// <summary>
    /// Projection of AssignmentDTO with just the fields the detail page needs.
    /// Extra fields on the wire (CreatedAt, AssignmentSetId, etc.) are ignored.
    /// </summary>
    public class AssignmentRef
    {
        public Guid Id { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public int? TotalPoints { get; set; }
        public List<ExerciseRef> Exercises { get; set; } = new();

        public int ExerciseCount => Exercises.Count;
    }

    /// <summary>
    /// We only need a count of exercises per assignment for the detail page,
    /// so this is intentionally minimal — every other field on
    /// AssignmentExerciseDTO is dropped during deserialization.
    /// </summary>
    public class ExerciseRef
    {
        public Guid Id { get; set; }
    }
}