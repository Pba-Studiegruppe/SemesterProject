using Exercise_Application.Interfaces.Repositories;
using Exercise_Application.Interfaces.Services;
using Exercise_Application.Projections;

public class ExerciseSnapshotQueryService : IExerciseSnapshotQueryService
{
    private readonly IExerciseRepository _repository;

    public ExerciseSnapshotQueryService(IExerciseRepository repository)
    {
        _repository = repository;
    }

    public async Task<ExerciseSnapshotProjection?> GetForSnapshotAsync(Guid exerciseId)
    {
        var exercise = await _repository.GetForSnapshotAsync(exerciseId);
        if (exercise is null) return null;

        var questions = exercise.Questions
            .Select(q => new QuestionSnapshotProjection(q.Id, q.Title, q.Content))
            .ToList();

        return new ExerciseSnapshotProjection(
            exercise.Id,
            exercise.Title,
            exercise.Content,
            questions);
    }
}