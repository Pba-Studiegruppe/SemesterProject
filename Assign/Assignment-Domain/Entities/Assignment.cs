using System;
using System.Collections.Generic;

namespace Assignment_Api;

public partial class Assignment
{
    public Guid Id { get; set; }

    public int? TotalPoints { get; set; }

    public Guid? AssignmentSetId { get; set; }

    public string? Title { get; set; }

    public string? Description { get; set; }

    public DateOnly? CreatedAt { get; set; }

    public byte[]? RowVersion { get; set; }

    public virtual ICollection<AssignmentExercise> AssignmentExercises { get; set; } = new List<AssignmentExercise>();

    public virtual AssignmentSet? AssignmentSet { get; set; }

    public virtual ICollection<SubmittedAssignment> SubmittedAssignments { get; set; } = new List<SubmittedAssignment>();
}
