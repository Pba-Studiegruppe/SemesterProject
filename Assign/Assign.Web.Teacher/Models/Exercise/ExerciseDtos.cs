using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

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

public class CreateExerciseQuestionRequest
{
    [Required]
    public string Text { get; set; } = string.Empty;

    /// <summary>Position of the question in the exercise (1-based).</summary>
    public int Order { get; set; }

    /// <summary>Optional reference solution shown to teachers only.</summary>
    public string? Solution { get; set; }
}

public class UpdateExerciseRequest
{
    public Guid Id { get; set; }

    [Required]
    [StringLength(200, MinimumLength = 1)]
    public string Title { get; set; } = string.Empty;

    public string? Description { get; set; }

    public List<Guid> KeywordIds { get; set; } = new();
}

public class CreateKeywordRequest
{
    [Required]
    [StringLength(100, MinimumLength = 1)]
    public string Name { get; set; } = string.Empty;
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

public class ExerciseReviewResponse
{
    public Guid Id { get; set; }
    public Guid TeacherId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }

    public List<KeywordDto> Keywords { get; set; } = new();
    public List<ExerciseQuestionReviewDto> Questions { get; set; } = new();
}

public class ExerciseQuestionReviewDto
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public int Order { get; set; }
    public string? Solution { get; set; }
}

public class ExerciseSnapshotResponse
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public List<KeywordDto> Keywords { get; set; } = new();
    public List<ExerciseQuestionSnapshotDto> Questions { get; set; } = new();
}

public class ExerciseQuestionSnapshotDto
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public int Order { get; set; }
}