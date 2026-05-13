using System;
using System.Collections.Generic;

namespace Assign.Web.Teacher.Models.Exercise;

// ─────────────────────────────────────────────────────────────────────────────
// These mirror the shapes in Exercise-Application/DTO for serialization.
// They are intentionally local to the frontend to avoid pulling in EF Core
// and other backend dependencies. If a shared Contracts project is added
// later, replace these with references to it.
// ─────────────────────────────────────────────────────────────────────────────

public sealed class CreateExerciseRequest
{
    public string Title { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public Guid CreatedByTeacherId { get; set; }
    public List<CreateQuestionRequest?> Questions { get; set; } = new();
    public List<CreateExerciseKeywordRequest?> ExerciseKeywords { get; set; } = new();
    public CreateExerciseSolutionRequest? Solution { get; set; }
}

public sealed class CreateQuestionRequest
{
    public string Title { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public CreateQuestionSolutionRequest? Solution { get; set; }
}

public sealed class CreateQuestionSolutionRequest
{
    public Guid QuestionId { get; set; }
    public string Content { get; set; } = string.Empty;
}

public sealed class CreateExerciseKeywordRequest
{
    public Guid KeywordId { get; set; }
}

public sealed class CreateExerciseSolutionRequest
{
    public string Content { get; set; } = string.Empty;
    public string? VideoUrl { get; set; }
}

// ── Responses ────────────────────────────────────────────────────────────────

public sealed class ExerciseResponse
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
}

public sealed class KeywordDto
{
    public Guid Id { get; set; }
    public string KeywordName { get; set; } = string.Empty;
    public string KeywordType { get; set; } = string.Empty;
}
