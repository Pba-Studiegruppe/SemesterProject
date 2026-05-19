using System;
using System.Collections.Generic;

namespace Assign.Web.Teacher.Models.Assignment
{
    /// <summary>
    /// Full SubmittedAssignmentDTO projection — used by the evaluation page
    /// where we need the nested exercises, questions, and their denormalized
    /// source fields (titles, content) for display.
    /// </summary>
    public class SubmittedAssignmentDetail
    {
        public Guid Id { get; set; }
        public Guid AssignmentId { get; set; }
        public Guid StudentId { get; set; }
        public EvaluationStatus Status { get; set; }
        public Guid? EvaluatorId { get; set; }
        public int TotalScore { get; set; }
        public int MaxPossibleScore { get; set; }
        public List<SubmittedExerciseDetail> SubmittedExercises { get; set; } = new();
    }

    public class SubmittedExerciseDetail
    {
        public Guid Id { get; set; }
        public Guid SubmittedAssignmentId { get; set; }
        public Guid AssignmentExerciseId { get; set; }
        public string? OverallComment { get; set; }
        public int TotalScore { get; set; }
        public string SourceTitle { get; set; } = string.Empty;
        public string SourceContent { get; set; } = string.Empty;
        public int SourceOrder { get; set; }
        public List<SubmittedQuestionDetail> SubmittedQuestions { get; set; } = new();
    }

    public class SubmittedQuestionDetail
    {
        public Guid Id { get; set; }
        public Guid SubmittedExerciseId { get; set; }
        public Guid AssignmentQuestionId { get; set; }
        public int MaxPoints { get; set; }
        public int? PointsAwarded { get; set; }
        public string? Comment { get; set; }
        public Guid? ErrorTypeId { get; set; }
        public bool IsScored { get; set; }
        public string SourceTitle { get; set; } = string.Empty;
        public string SourceContent { get; set; } = string.Empty;
        public int SourceOrder { get; set; }
    }

    // ── Request DTOs (match backend names exactly for JSON serialization) ──

    public class ScoreQuestionRequest
    {
        public int Points { get; set; }
        public string? Comment { get; set; }
        public Guid? ErrorTypeId { get; set; }
    }

    public class SetExerciseCommentRequest
    {
        public string? Comment { get; set; }
    }

    public class MarkEvaluatedRequest
    {
        public Guid EvaluatorId { get; set; }
    }
}
