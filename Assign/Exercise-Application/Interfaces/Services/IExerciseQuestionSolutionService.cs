using Exercise_Application.DTO;

public interface IExerciseQuestionSolutionService
{
    Task<ExerciseDTO> SetQuestionSolutionAsync(Guid exerciseId, CreateQuestionSolutionRequest dto);
    Task<ExerciseDTO> UpdateQuestionSolutionAsync(Guid exerciseId, UpdateQuestionSolutionRequest dto);
    Task<ExerciseDTO> RemoveQuestionSolutionAsync(Guid exerciseId, RemoveQuestionSolutionRequest dto);
}