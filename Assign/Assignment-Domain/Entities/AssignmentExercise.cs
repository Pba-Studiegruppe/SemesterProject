using System;
using System.Collections.Generic;

namespace Assignment_Api;

public partial class AssignmentExercise
{
    public Guid AssignmentId { get; set; }

    public Guid ExerciseId { get; set; }

    public byte[]? RowVersion { get; set; }

    public virtual Assignment Assignment { get; set; } = null!;
}
