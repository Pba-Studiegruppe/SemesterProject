using Exercise_Application.DTO;

public interface IExerciseQuestionService
{
    Task<ExerciseDTO> AddQuestionsAsync(Guid exerciseId, List<CreateQuestionRequest> questions);
    Task<ExerciseDTO> UpdateQuestionAsync(Guid exerciseId, UpdateQuestionRequest dto);
    Task<ExerciseDTO> RemoveQuestionAsync(Guid exerciseId, RemoveQuestionRequest dto);


}
