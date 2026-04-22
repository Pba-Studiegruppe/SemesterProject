using Exercise_Application.DTO;

public interface IExerciseSolutionService
{
    Task<ExerciseDTO> SetExerciseSolution(ExerciseSolutionDTO solution);
    Task<ExerciseDTO> UpdateExerciseSolutionAsync(UpdateExerciseSolutionRequest dto);
    Task<ExerciseDTO> RemoveExerciseSolutionAsync(Guid exerciseId);
}