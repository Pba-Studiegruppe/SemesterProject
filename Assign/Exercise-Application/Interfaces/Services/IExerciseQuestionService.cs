using Exercise_Application.DTO;

public interface IExerciseQuestionService
{
    Task<ExerciseDTO> AddQuestionsAsync(List<QuestionDTO> questions);
    Task<ExerciseDTO> UpdateQuestionAsync(UpdateQuestionRequest dto);
    Task<ExerciseDTO> RemoveQuestionAsync(RemoveQuestionRequest dto);


}
