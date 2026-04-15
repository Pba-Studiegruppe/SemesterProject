using Exercise_Application.DTO;

namespace Exercise_Application.Interfaces.Services
{
    public interface IExerciseSolutionService
    {
        Task<ExerciseSolutionDTO> CreateExerciseSolutionAsync(CreateExerciseSolutionRequest dto);
        Task<IEnumerable<ExerciseSolutionDTO?>> GetExerciseSolutionsByExerciseIdAsync(Guid exerciseId);
        Task<ExerciseSolutionDTO?> GetExerciseSolutionByIdAsync(Guid id);
        Task<ExerciseSolutionDTO> UpdateExerciseSolutionAsync(Guid id, UpdateExerciseSolutionRequest dto, byte[] rowVersion);
    }
}