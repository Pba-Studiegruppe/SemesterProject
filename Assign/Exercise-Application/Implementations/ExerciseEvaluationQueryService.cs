using Exercise_Application.Interfaces.Services;
using Exercise_Application.Projections;

public class ExerciseEvaluationQueryService : IExerciseEvaluationQueryService
{
    public Task<ExerciseEvaluationProjection?> GetForEvaluationAsync(Guid exerciseId, Guid teacherId)
    {
        throw new NotImplementedException();
    }
}