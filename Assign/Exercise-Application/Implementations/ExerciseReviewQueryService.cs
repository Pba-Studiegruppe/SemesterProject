using Exercise_Application.Interfaces.Repositories;
using Exercise_Application.Interfaces.Services;
using Exercise_Application.Projections;

namespace Exercise_Application.Implementations
{
    public class ExerciseReviewQueryService : IExerciseReviewQueryService
    {
        private readonly IExerciseRepository _repository;

        public ExerciseReviewQueryService(IExerciseRepository repository)
        {
            _repository = repository;
        }

        public async Task<ExerciseReviewProjection?> GetForReviewAsync(Guid exerciseId)
        {
            var exercise = await _repository.GetForReviewAsync(exerciseId);
            if (exercise is null) return null;

            var questions = exercise.Questions
                .Select(q => new QuestionReviewProjection(
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

            return new ExerciseReviewProjection(
                exercise.Id,
                exercise.Title,
                exercise.Content,
                solution,
                questions);
        }
    }
}