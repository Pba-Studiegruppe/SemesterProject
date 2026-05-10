using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exercise_Application.Projections
{
    public sealed record ExerciseEvaluationProjection(
        Guid Id,
        string Title,
        string Content,
        ExerciseSolutionProjection? Solution,
        IReadOnlyList<QuestionEvaluationProjection> Questions);

    public sealed record QuestionEvaluationProjection(
        Guid Id,
        string Title,
        string Content,
        QuestionSolutionProjection? Solution);

    public sealed record ExerciseSolutionProjection(string Content, string? VideoUrl);

    public sealed record QuestionSolutionProjection(string Content);
}
