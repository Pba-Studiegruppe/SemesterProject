using Assignment_Application.DTO;
using Assignment_Application.Interfaces.Repositories;
using Assignment_Application.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment_Application.Implementations
{
    public class AssignmentSetService: IAssignmentSetService
    {
        private readonly IAssignmentRepository _repository;

        public AssignmentSetService(IAssignmentRepository repository)
        {
            _repository = repository;
        }

        public Task<AssignmentSetDTO> CreateAssignmentSetAsync(CreateAssignmentExerciseRequest request)
        {
            throw new NotImplementedException();
        }

        public Task<AssignmentSetDTO> GetAssignmentSetsAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<AssignmentSetDTO>> GetAssignmentSetsByCourseIdAsync(Guid courseId)
        {
            throw new NotImplementedException();
        }

        public Task<AssignmentSetDTO> UpdateAssignmentSetAsync(UpdateAssignmentSetRequest request)
        {
            throw new NotImplementedException();
        }
    }
}
