using Exercise_Application.DTO;

namespace Exercise_Application.Interfaces.Services
{
    public interface IExerciseKeywordService
    {
        Task<ExerciseKeyword> AddExerciseKeywordAsync(ExerciseKeywordDTO dto);
        Task<IEnumerable<ExerciseKeyword>> GetExerciseKeywordsByExerciseIdAsync(Guid exerciseId);
    }
}                                                                                                                                                                                                                                         