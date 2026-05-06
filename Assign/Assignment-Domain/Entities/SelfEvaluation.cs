using System;
using System.Collections.Generic;

namespace Assignment_Api;

public partial class SelfEvaluation
{
    public Guid Id { get; set; }

    public int? Points { get; set; }

    public Guid? SubmittedExerciseId { get; set; }

    public byte[]? RowVersion { get; set; }

    public virtual SubmittedExercise? SubmittedExercise { get; set; }
}
