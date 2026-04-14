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
        private readonly IAssignmentSetRepository _repository;

        public AssignmentSetService(IAssignmentSetRepository repository)
        {
            _repository = repository;
        }
    }
}
