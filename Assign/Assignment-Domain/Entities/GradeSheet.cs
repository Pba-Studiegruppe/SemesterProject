using System;
using System.Collections.Generic;

namespace Assignment_Api;

public partial class GradeSheet
{
    public Guid Id { get; set; }

    public Guid? AssignmentSetId { get; set; }

    public byte[]? RowVersion { get; set; }

    public virtual AssignmentSet? AssignmentSet { get; set; }
}
