using Exercise_Application.DTO;

public interface IExerciseKeywordService
{
    Task<ExerciseDTO> AddExerciseKeywordsAsync(Guid ExerciseId, List<CreateExerciseKeywordRequest> exerciseKeywords);
    Task<ExerciseDTO> RemoveExerciseKeywordAsync(Guid ExerciseId, RemoveExerciseKeywordRequest dto);
}