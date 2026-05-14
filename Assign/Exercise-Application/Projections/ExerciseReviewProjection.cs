using System;
using System.Collections.Generic;

namespace Exercise_Application.Projections
{
    public sealed record ExerciseReviewProjection(
        Guid Id,
        string Title,
        string Content,
        ExerciseSolutionProjection? Solution,
        IReadOnlyList<QuestionReviewProjection> Questions);

    public sealed record QuestionReviewProjection(
        Guid Id,
        string Title,
        string Content,
        QuestionSolutionProjection? Solution);

    public sealed record ExerciseSolutionProjection(string Content, string? VideoUrl);

    public sealed record QuestionSolutionProjection(string Content);
}