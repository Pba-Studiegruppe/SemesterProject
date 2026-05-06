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
    public class AssignmentExerciseService: IAssignmentExerciseService    
    {
        private readonly IAssignmentRepository _repository;

        public AssignmentExerciseService(IAssignmentRepository repository)
        {
            _repository = repository;
        }

        public Task<AssignmentExerciseDTO> AddAssignmentExerciseAsync(Guid assignmentId, CreateAssignmentExerciseRequest request)
        {
            throw new NotImplementedException();
        }

        public Task<AssignmentExerciseDTO> RemoveAssignmentExerciseAsync(Guid assignmentId, RemoveAssignmentExerciseRequest request)
        {
            throw new NotImplementedException();
        }
    }
}
