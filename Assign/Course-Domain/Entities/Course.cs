using System;
using System.Collections.Generic;

namespace Course_Domain.Entities;

public partial class Course
{
    public Guid Id { get; set; }

    public Guid? SubjectId { get; set; }

    public bool? Inactive { get; set; }

    public byte[]? RowVersion { get; set; }

    public string? Name { get; set; }

    public virtual ICollection<CourseStudent> CourseStudents { get; set; } = new List<CourseStudent>();

    public virtual ICollection<CourseTeacher> CourseTeachers { get; set; } = new List<CourseTeacher>();

    public virtual Subject? Subject { get; set; }
}
