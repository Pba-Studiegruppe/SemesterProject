using Course_Domain.Entities;
using Course_Infrastructure.DataAccess.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Course_Infrastructure.DataAccess.Repositories
{
    public class CourseRepository : ICourseRepository
    {
        private DbContext _dbContext;

        public CourseRepository(DbContext dbContext)
        {
            _dbContext = dbContext;
        }

        Task<Course> ICourseRepository.AddCourseAsync(Course course)
        {
            throw new NotImplementedException();
        }

        Task ICourseRepository.DeleteCourseAsync(Guid courseId)
        {
            throw new NotImplementedException();
        }

        Task<IEnumerable<Course>> ICourseRepository.GetAllCoursesAsync()
        {
            throw new NotImplementedException();
        }

        Task<Course> ICourseRepository.GetCourseByIdAsync(Guid courseId)
        {
            throw new NotImplementedException();
        }

        Task<Course> ICourseRepository.UpdateCourseAsync(Course course)
        {
            throw new NotImplementedException();
        }
    }
}
