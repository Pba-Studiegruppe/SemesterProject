using System;
using System.Collections.Generic;

namespace Assignment_Application.DTO
{
    public class SubmittedExerciseDTO
    {
        public Guid Id { get; set; }
        public Guid SubmittedAssignmentId { get; set; }
        public Guid AssignmentExerciseId { get; set; }
        public string? OverallComment { get; set; }
        public int TotalScore { get; set; }

        // ── denormalized from the source AssignmentExercise for UI display ──
        public string SourceTitle { get; set; } = string.Empty;
        public string SourceContent { get; set; } = string.Empty;
        public int SourceOrder { get; set; }

        public List<SubmittedQuestionDTO> SubmittedQuestions { get; set; } = [];
    }

    public class SetExerciseCommentRequest
    {
        public string? Comment { get; set; }
    }
}
