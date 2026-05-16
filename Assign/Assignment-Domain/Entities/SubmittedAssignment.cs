using System;
using System.Collections.Generic;

namespace Assignment_Domain.Entities;

public partial class SubmittedAssignment
{
    public Guid Id { get; set; }

    public Guid? StudentId { get; set; }

    public bool? SelfEvaluationSubmitted { get; set; }

    public Guid? AssignmentId { get; set; }

    public byte[]? RowVersion { get; set; }

    public virtual Assignment? Assignment { get; set; }

    public virtual ICollection<AssignmentFeedback> AssignmentFeedbacks { get; set; } = new List<AssignmentFeedback>();

    public virtual ICollection<SubmittedExercise> SubmittedExercises { get; set; } = new List<SubmittedExercise>();
}
