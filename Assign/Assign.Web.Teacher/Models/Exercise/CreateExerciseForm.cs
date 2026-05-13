using System;
using System.Collections.Generic;

namespace Assign.Web.Teacher.Models.Exercise;

/// <summary>
/// The state the Create Exercise page binds to. Mapped to
/// <see cref="CreateExerciseRequest"/> at submit time.
/// </summary>
public sealed class CreateExerciseForm
{
    public string Title { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;

    public List<QuestionFormItem> Questions { get; set; } = new();

    public ExerciseSolutionFormItem Solution { get; set; } = new();

    // Keywords are split per category so each KeywordSelector owns its own list.
    public List<Guid> SchoolSubjectKeywordIds { get; set; } = new();
    public List<Guid> ExerciseTypeKeywordIds { get; set; } = new();
    public List<Guid> SubjectAreaKeywordIds { get; set; } = new();

    // ── Settings ─────────────────────────────────────────────────────────────
    // TODO: These properties are NOT yet reflected on the Exercise domain
    // entity / DTOs and are therefore not sent to the API. Wire them up once
    // the backend exposes them.
    public string Difficulty { get; set; } = "";
    public int? EstimatedTimeMinutes { get; set; }
    public string Visibility { get; set; } = "Private";
}

public sealed class QuestionFormItem
{
    /// <summary>Client-side id used purely for UI tracking (keys, deletes, etc.).</summary>
    public Guid LocalId { get; set; } = Guid.NewGuid();

    public string Title { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;

    public string SolutionContent { get; set; } = string.Empty;
    public bool SolutionExpanded { get; set; }

    public bool Expanded { get; set; } = true;
}

public sealed class ExerciseSolutionFormItem
{
    public string Content { get; set; } = string.Empty;
    public string VideoUrl { get; set; } = string.Empty;
}
