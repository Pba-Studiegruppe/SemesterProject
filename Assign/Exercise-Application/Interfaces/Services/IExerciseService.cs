using Exercise_Application.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exercise_Application.Interfaces.Services
{
    public interface IExerciseService
    {
        Task<ExerciseDTO?> GetExerciseByIdAsync(Guid id);
        Task<IEnumerable<ExerciseDTO?>> GetExercisesByTeacherIdAsync(Guid teacherId);
        Task<IEnumerable<ExerciseDTO?>> GetExerciseByExerciseKeywords(List<Guid> keywordIds);
        Task<ExerciseDTO> CreateExerciseAsync(CreateExerciseRequest dto);
        Task<ExerciseDTO?> UpdateExerciseAsync(UpdateExerciseRequest dto);
    }
}
