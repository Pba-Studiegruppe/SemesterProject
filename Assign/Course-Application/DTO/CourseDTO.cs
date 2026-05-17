using Course_Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Course_Application.DTO
{
    public class CourseDTO
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

    public class CreateCourseRequest
    {
        public Guid? SubjectId { get; set; }
        public bool? Inactive { get; set; }
        public string? Name { get; set; }
        public List<CourseTeacher> Teachers { get; set; }
        public List<CourseStudent> Students { get; set; }
    }

    public class UpdateCourseRequest
    {
        public Guid Id { get; set; }
        public Guid? SubjectId { get; set; }
        public bool? Inactive { get; set; }
        public string? Name { get; set; }
        public byte[]? RowVersion { get; set; }
        public List<CourseTeacher> Teachers { get; set; }
        public List<CourseStudent> Students { get; set; }
    }
}
