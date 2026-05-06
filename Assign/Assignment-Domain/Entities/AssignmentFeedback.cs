using System;
using System.Collections.Generic;

namespace Assignment_Api;

public partial class AssignmentFeedback
{
    public Guid Id { get; set; }

    public Guid? StudentId { get; set; }

    public bool? SelfEvaluationSubmitted { get; set; }

    public Guid? SubmittedAssignmentId { get; set; }

    public byte[]? RowVersion { get; set; }

    public virtual SubmittedAssignment? SubmittedAssignment { get; set; }
}
