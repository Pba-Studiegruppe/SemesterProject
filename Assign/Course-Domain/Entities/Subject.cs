using System;
using System.Collections.Generic;

namespace Course_Domain.Entities;

public partial class Subject
{
    public Guid Id { get; set; }

    public string? Title { get; set; }

    public string? Level { get; set; }

    public bool? Inactive { get; set; }

    public byte[]? RowVersion { get; set; }

    public virtual ICollection<Course> Courses { get; set; } = new List<Course>();
}
