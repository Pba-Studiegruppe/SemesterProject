using Assignment_Application.Interfaces.Repositories;
using Assignment_Application.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment_Application.Implementations
{
    public class AssignmentService: IAssignmentService
    {
        private readonly IAssignmentRepository _repository;

        public AssignmentService(IAssignmentRepository repository)
        {
            _repository = repository;
        }
    }
}
