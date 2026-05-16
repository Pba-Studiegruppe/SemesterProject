using System;
using System.Collections.Generic;

namespace Assignment_Domain.Entities;

public partial class SubmittedExercise
{
    public Guid Id { get; set; }
    public Guid? FeedbackId { get; set; }
    public Guid? SubmittedAssignmentId { get; set; }
    public byte[]? RowVersion { get; set; }
    public virtual ICollection<ExerciseFeedback> ExerciseFeedbacks { get; set; } = new List<ExerciseFeedback>();
    public virtual ICollection<SelfEvaluation> SelfEvaluations { get; set; } = new List<SelfEvaluation>();
    public virtual SubmittedAssignment? SubmittedAssignment { get; set; }
}
