using Assignment_Domain.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment_Domain.Entities
{
    public class AssignmentExercise 
    {
        
        public Guid ExerciseId { get; private set; }
        private AssignmentExercise() { } // Required by EF Core

    }
}
