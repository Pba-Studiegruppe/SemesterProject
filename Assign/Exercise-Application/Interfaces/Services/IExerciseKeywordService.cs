using Exercise_Application.DTO;

public interface IExerciseKeywordService
{
    Task<ExerciseDTO> AddExerciseKeywordsAsync(List<ExerciseKeywordDTO> exerciseKeywords);
    Task<ExerciseDTO> RemoveExerciseKeywordAsync(RemoveExerciseKeywordRequest dto);
}