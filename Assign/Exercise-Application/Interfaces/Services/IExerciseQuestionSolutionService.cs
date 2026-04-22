using Exercise_Application.DTO;

public interface IExerciseQuestionSolutionService
{
    Task<ExerciseDTO> UpdateQuestionSolutionAsync(UpdateQuestionSolutionRequest dto);
    Task<ExerciseDTO> RemoveQuestionSolutionAsync(Guid questionId);
}