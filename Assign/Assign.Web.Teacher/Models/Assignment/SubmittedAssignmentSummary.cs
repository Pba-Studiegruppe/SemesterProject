using System;

namespace Assign.Web.Teacher.Models.Assignment
{
    /// <summary>
    /// Lightweight projection of SubmittedAssignmentDTO used in the submissions
    /// table on the Assignment detail page. The wire response includes the full
    /// nested SubmittedExercises tree; we just don't deserialize it (System.Text.Json
    /// silently drops fields that aren't on the model).
    /// </summary>
    public class SubmittedAssignmentSummary
    {
        public Guid Id { get; set; }
        public Guid AssignmentId { get; set; }
        public Guid StudentId { get; set; }
        public EvaluationStatus Status { get; set; }
        public Guid? EvaluatorId { get; set; }
        public int TotalScore { get; set; }
        public int MaxPossibleScore { get; set; }
    }

    /// <summary>
    /// Mirrors Assignment_Domain.Shared.EvaluationStatus on the backend.
    /// Values must stay in sync — the API serializes the enum as its integer
    /// value (System.Text.Json default), and we deserialize directly into
    /// this enum on the frontend.
    /// </summary>
    public enum EvaluationStatus
    {
        Pending = 0,
        InProgress = 1,
        Completed = 2,
        Returned = 3,
    }
}
