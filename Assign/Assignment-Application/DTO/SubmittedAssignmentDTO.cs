using Assignment_Domain.Entities;
using Assignment_Domain.Shared;
using System;
using System.Collections.Generic;

namespace Assignment_Application.DTO
{
    public class SubmittedAssignmentDTO
    {
        public Guid Id { get; set; }
        public Guid AssignmentId { get; set; }
        public Guid StudentId { get; set; }
        public EvaluationStatus Status { get; set; }
        public Guid? EvaluatorId { get; set; }
        public int TotalScore { get; set; }

        /// <summary>
        /// Sum of all questions' MaxPoints across the source assignment at the
        /// time of submission. Useful for "score / max" display in the UI.
        /// </summary>
        public int MaxPossibleScore { get; set; }

        public List<SubmittedExerciseDTO> SubmittedExercises { get; set; } = [];
    }

    /// <summary>
    /// Registers a student's submission for an assignment. The service is
    /// expected to load the Assignment, call SubmittedAssignment.CreateFromAssignment,
    /// and persist — the snapshot of exercises/questions is taken at that point.
    /// </summary>
    public class CreateSubmittedAssignmentRequest
    {
        public Guid AssignmentId { get; set; }
        public Guid StudentId { get; set; }
    }

    public class MarkEvaluatedRequest
    {
        // Eventually this would come from the authenticated principal,
        // but for now it's passed explicitly to match the existing layer style.
        public Guid EvaluatorId { get; set; }
    }
}
