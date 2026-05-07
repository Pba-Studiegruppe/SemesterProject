using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Course_Domain.Entities;

namespace Course_Infrastructure.DataAccess.Interfaces
{
    public interface ICourseRepository
    {
        public Task<Course> GetCourseByIdAsync(Guid courseId);
        public Task<IEnumerable<Course>> GetAllCoursesAsync();
        public Task<Course> AddCourseAsync(Course course);
        public Task<Course> UpdateCourseAsync(Course course);
        public Task DeleteCourseAsync(Guid courseId);
    }
}
