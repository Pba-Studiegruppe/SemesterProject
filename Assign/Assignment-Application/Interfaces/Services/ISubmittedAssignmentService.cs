using Assignment_Application.DTO;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Assignment_Application.Interfaces.Services
{
    public interface ISubmittedAssignmentService
    {
        /// <summary>
        /// Returns the full submission with all submitted exercises and questions,
        /// including denormalized source fields (titles, content, order) so the
        /// teacher's evaluation UI can render everything in one round-trip.
        /// </summary>
        Task<SubmittedAssignmentDTO> GetSubmittedAssignmentAsync(Guid id);

        /// <summary>
        /// Returns all submissions for the given assignment. Intended for the
        /// teacher's submissions list and, later, for the grade-book export.
        /// </summary>
        Task<IEnumerable<SubmittedAssignmentDTO>> GetSubmittedAssignmentsByAssignmentAsync(Guid assignmentId);

        /// <summary>
        /// Creates a new submission slot for a student on a given assignment.
        /// Snapshots the assignment's exercise/question structure into the submission.
        /// </summary>
        Task<SubmittedAssignmentDTO> CreateSubmittedAssignmentAsync(CreateSubmittedAssignmentRequest request);

        Task<SubmittedQuestionDTO> ScoreQuestionAsync(
            Guid submittedAssignmentId,
            Guid submittedQuestionId,
            ScoreQuestionRequest request);

        Task<SubmittedExerciseDTO> SetExerciseCommentAsync(
            Guid submittedAssignmentId,
            Guid submittedExerciseId,
            SetExerciseCommentRequest request);

        Task<SubmittedAssignmentDTO> MarkEvaluatedAsync(
            Guid submittedAssignmentId,
            MarkEvaluatedRequest request);

        Task<SubmittedAssignmentDTO> ReturnSubmissionAsync(Guid submittedAssignmentId);
    }
}
