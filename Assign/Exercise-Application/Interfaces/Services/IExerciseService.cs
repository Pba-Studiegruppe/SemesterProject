using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exercise_Application.Interfaces.Services
{
    public interface IExerciseService
    {
        //skal ændres til at tage en dto
        void CreateExercise(string title, string content, Guid teacherId);
    }
}
