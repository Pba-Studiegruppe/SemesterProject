using Exercise_Application.Interfaces.Repositories;
using Exercise_Application.Interfaces.Services;
using Exercise_Application.Projections;

public class ExerciseEvaluationQueryService : IExerciseEvaluationQueryService
{
    private readonly IExerciseRepository _repository;

    public ExerciseEvaluationQueryService(IExerciseRepository repository)
    {
        _repository = repository;
    }

    public async Task<ExerciseEvaluationProjection?> GetForEvaluationAsync(
        Guid exerciseId,
        Guid teacherId)
    {
        var exercise = await _repository.GetForEvaluationAsync(exerciseId);
        if (exercise is null) return null;

        if (exercise.CreatedByTeacherId != teacherId)
            throw new UnauthorizedAccessException(
                $"Teacher '{teacherId}' is not authorized to view evaluation data " +
                $"for exercise '{exerciseId}'.");

        var questions = exercise.Questions
            .Select(q => new QuestionEvaluationProjection(
                q.Id,
                q.Title,
                q.Content,
                q.Solution is null
                    ? null
                    : new QuestionSolutionProjection(q.Solution.Content)))
            .ToList();

        var solution = exercise.Solution is null
            ? null
            : new ExerciseSolutionProjection(
                exercise.Solution.Content,
                exercise.Solution.VideoUrl);

        return new ExerciseEvaluationProjection(
            exercise.Id,
            exercise.Title,
            exercise.Content,
            solution,
            questions);
    }
}
