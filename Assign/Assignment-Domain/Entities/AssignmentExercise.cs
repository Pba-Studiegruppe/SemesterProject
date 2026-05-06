using System;
using System.Collections.Generic;

namespace Assignment_Api;

public partial class AssignmentExercise
{
    public Guid AssignmentId { get; private set; }

    public Guid ExerciseId { get; private set; }

    public byte[]? RowVersion { get; set; }

    public virtual Assignment Assignment { get; set; } = null!;

    public AssignmentExercise(Guid assignmentId, Guid exerciseId)
    {
        AssignmentId = assignmentId;
        ExerciseId = exerciseId;
    }
}
