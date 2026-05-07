using System;
using System.Collections.Generic;

namespace Course_Domain.Entities;

public partial class CourseStudent
{
    public Guid StudentId { get; set; }

    public Guid CourseId { get; set; }

    public bool? Inactive { get; set; }

    public byte[]? RowVersion { get; set; }

    public virtual Course Course { get; set; } = null!;
}
