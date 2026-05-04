using System;
using System.Collections.Generic;

namespace Course_Api;

public partial class CourseTeacher
{
    public Guid TeacherId { get; set; }

    public Guid CourseId { get; set; }

    public bool? Inactive { get; set; }

    public byte[]? RowVersion { get; set; }

    public virtual Course Course { get; set; } = null!;
}
