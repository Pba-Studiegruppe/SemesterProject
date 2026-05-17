using System;

namespace Assignment_Application.DTO
{
    public class SubmittedQuestionDTO
    {
        public Guid Id { get; set; }
        public Guid SubmittedExerciseId { get; set; }
        public Guid AssignmentQuestionId { get; set; }
        public int MaxPoints { get; set; }
        public int? PointsAwarded { get; set; }
        public string? Comment { get; set; }
        public Guid? ErrorTypeId { get; set; }
        public bool IsScored { get; set; }

        // ── denormalized from the source AssignmentQuestion for UI display ──
        public string SourceTitle { get; set; } = string.Empty;
        public string SourceContent { get; set; } = string.Empty;
        public int SourceOrder { get; set; }
    }

    public class ScoreQuestionRequest
    {
        public int Points { get; set; }
        public string? Comment { get; set; }
        public Guid? ErrorTypeId { get; set; }
    }
}
