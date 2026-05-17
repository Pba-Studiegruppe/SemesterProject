using Course_Application.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Course_Application.Services.Interfaces
{
    public interface ICourseService
    {
        Task<CourseDTO?> GetCourseByIdAsync(Guid Id);
        Task<CourseDTO> CreateCourseAsync(CreateCourseRequest dto);
        Task<CourseDTO?> UpdateCourseAsync(Guid Id, UpdateCourseRequest dto);
        Task<List<CourseDTO>> GetCoursesAsync();
    }
}
