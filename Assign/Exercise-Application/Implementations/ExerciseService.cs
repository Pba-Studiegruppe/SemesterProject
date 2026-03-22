using Exercise_Application.Interfaces.Repositories;
using Exercise_Application.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exercise_Application.Implementations
{
    public class ExerciseService: IExerciseService
    {
        private readonly IExerciseRepository _repository;

        public ExerciseService(IExerciseRepository repository)
        {
            _repository = repository;
        }

        //skal ændres til at tage en dto
        public void CreateExercise(string title, string content, Guid teacherId)
        {
            var exercise = new Exercise(title, content, teacherId);

            _repository.AddExercise(exercise);
        }

    }
}
