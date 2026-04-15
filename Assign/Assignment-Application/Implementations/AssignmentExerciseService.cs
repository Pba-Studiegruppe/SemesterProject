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
        private readonly IAssignmentExerciseRepository _repository;

        public AssignmentExerciseService(IAssignmentExerciseRepository repository)
        {
            _repository = repository;
        }
    }
}
