using Course_Application.DTO;
using Course_Application.Services.Interfaces;
using Course_Domain.Entities;
using Course_Infrastructure.DataAccess.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Course_Application.Services
{
    public class CourseService : ICourseService
    {
        private readonly ICourseRepository _repository;

        public CourseService(ICourseRepository repository)
        {
            _repository = repository;
        }

        Task<CourseDTO> ICourseService.CreateCourseAsync(CreateCourseRequest dto)
        {
            throw new NotImplementedException();
        }

        Task<CourseDTO?> ICourseService.GetCourseByIdAsync(Guid Id)
        {
            throw new NotImplementedException();
        }

        Task<List<CourseDTO>> ICourseService.GetCoursesAsync()
        {
            throw new NotImplementedException();
        }

        Task<CourseDTO?> ICourseService.UpdateCourseAsync(Guid Id, UpdateCourseRequest dto)
        {
            throw new NotImplementedException();
        }
    }
}
