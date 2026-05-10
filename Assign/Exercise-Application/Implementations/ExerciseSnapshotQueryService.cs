using Exercise_Application.Interfaces.Services;
using Exercise_Application.Projections;

public class ExerciseSnapshotQueryService : IExerciseSnapshotQueryService
{
    public Task<ExerciseSnapshotProjection?> GetForSnapshotAsync(Guid exerciseId)
    {
        throw new NotImplementedException();
    }
}