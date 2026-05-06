using System;
using System.Collections.Generic;

namespace Assignment_Api;

public partial class ExerciseFeedback
{
    public Guid Id { get; set; }

    public string? Feedbacktext { get; set; }

    public int? PointsGiven { get; set; }

    public Guid? ErrorType { get; set; }

    public Guid? SubmittedExerciseId { get; set; }

    public byte[]? RowVersion { get; set; }

    public virtual ErrorType? ErrorTypeNavigation { get; set; }

    public virtual SubmittedExercise? SubmittedExercise { get; set; }
}
