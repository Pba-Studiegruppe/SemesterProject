using System;
using System.Collections.Generic;

namespace Assignment_Api;

public partial class AssignmentSet
{
    public Guid Id { get; set; }

    public Guid? CourseId { get; set; }

    public Guid? AssignmentId { get; set; }

    public Guid? GradesheetId { get; set; }

    public bool? IsPublihsed { get; set; }

    public bool? GradingPublished { get; set; }

    public bool? Inactive { get; set; }

    public string? Title { get; set; }

    public string? Description { get; set; }

    public DateOnly? CreatedAt { get; set; }

    public byte[]? RowVersion { get; set; }

    public virtual ICollection<Assignment> Assignments { get; set; } = new List<Assignment>();

    public virtual ICollection<GradeSheet> GradeSheets { get; set; } = new List<GradeSheet>();
}
