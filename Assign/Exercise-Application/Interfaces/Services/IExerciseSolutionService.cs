using Exercise_Application.DTO;

public interface IExerciseSolutionService
{
    Task<ExerciseDTO> SetExerciseSolution(Guid exerciseId, CreateExerciseSolutionRequest solution);
    Task<ExerciseDTO> RemoveExerciseSolutionAsync(Guid exerciseId);
}