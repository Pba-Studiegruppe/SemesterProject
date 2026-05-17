using Assignment_Application.DTO;
using Assignment_Application.Interfaces.Repositories;
using Assignment_Application.Interfaces.Services;
using Assignment_Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Assignment_Application.Implementations
{
    public class SubmittedAssignmentService : ISubmittedAssignmentService
    {
        private readonly ISubmittedAssignmentRepository _submissionRepository;
        private readonly IAssignmentRepository _assignmentRepository;

        public SubmittedAssignmentService(
            ISubmittedAssignmentRepository submissionRepository,
            IAssignmentRepository assignmentRepository)
        {
            _submissionRepository = submissionRepository;
            _assignmentRepository = assignmentRepository;
        }

        public async Task<SubmittedAssignmentDTO> GetSubmittedAssignmentAsync(Guid id)
        {
            var submission = await _submissionRepository.GetByIdAsync(id);
            if (submission is null)
                throw new KeyNotFoundException(
                    $"SubmittedAssignment with id '{id}' not found.");

            return ToDto(submission, submission.Assignment);
        }

        public async Task<IEnumerable<SubmittedAssignmentDTO>> GetSubmittedAssignmentsByAssignmentAsync(
            Guid assignmentId)
        {
            var assignment = await _assignmentRepository.GetByIdAsync(assignmentId);
            if (assignment is null)
                throw new KeyNotFoundException(
                    $"Assignment with id '{assignmentId}' not found.");

            var submissions = await _submissionRepository.GetByAssignmentIdAsync(assignmentId);
            return submissions.Select(s => ToDto(s, assignment)).ToList();
        }

        public async Task<SubmittedAssignmentDTO> CreateSubmittedAssignmentAsync(
            CreateSubmittedAssignmentRequest request)
        {
            if (request is null) throw new ArgumentNullException(nameof(request));

            var assignment = await _assignmentRepository.GetByIdAsync(request.AssignmentId);
            if (assignment is null)
                throw new KeyNotFoundException(
                    $"Assignment with id '{request.AssignmentId}' not found.");

            // Domain validates studentId != Guid.Empty.
            var submission = SubmittedAssignment.CreateFromAssignment(
                assignment, request.StudentId);

            await _submissionRepository.CreateAsync(submission);
            await _submissionRepository.SaveChangesAsync();

            return ToDto(submission, assignment);
        }

        public async Task<SubmittedQuestionDTO> ScoreQuestionAsync(
            Guid submittedAssignmentId,
            Guid submittedQuestionId,
            ScoreQuestionRequest request)
        {
            if (request is null) throw new ArgumentNullException(nameof(request));

            var submission = await _submissionRepository.GetByIdAsync(submittedAssignmentId);
            if (submission is null)
                throw new KeyNotFoundException(
                    $"SubmittedAssignment with id '{submittedAssignmentId}' not found.");

            // Domain throws KeyNotFoundException if the question isn't part of this
            // submission, ArgumentOutOfRangeException for invalid points, and
            // InvalidOperationException if the submission is returned.
            submission.ScoreQuestion(
                submittedQuestionId, request.Points, request.Comment, request.ErrorTypeId);

            await _submissionRepository.SaveChangesAsync();

            var sq = submission.SubmittedExercises
                .SelectMany(se => se.SubmittedQuestions)
                .Single(q => q.Id == submittedQuestionId);
            var sourceExercise = FindSourceExerciseForQuestion(submission, submittedQuestionId);

            return ToDto(sq, sourceExercise);
        }

        public async Task<SubmittedExerciseDTO> SetExerciseCommentAsync(
            Guid submittedAssignmentId,
            Guid submittedExerciseId,
            SetExerciseCommentRequest request)
        {
            if (request is null) throw new ArgumentNullException(nameof(request));

            var submission = await _submissionRepository.GetByIdAsync(submittedAssignmentId);
            if (submission is null)
                throw new KeyNotFoundException(
                    $"SubmittedAssignment with id '{submittedAssignmentId}' not found.");

            submission.SetExerciseComment(submittedExerciseId, request.Comment);
            await _submissionRepository.SaveChangesAsync();

            var se = submission.SubmittedExercises
                .Single(x => x.Id == submittedExerciseId);
            var sourceExercise = submission.Assignment?.AssignmentExercises
                .FirstOrDefault(ae => ae.Id == se.AssignmentExerciseId);

            return ToDto(se, sourceExercise);
        }

        public async Task<SubmittedAssignmentDTO> MarkEvaluatedAsync(
            Guid submittedAssignmentId,
            MarkEvaluatedRequest request)
        {
            if (request is null) throw new ArgumentNullException(nameof(request));

            var submission = await _submissionRepository.GetByIdAsync(submittedAssignmentId);
            if (submission is null)
                throw new KeyNotFoundException(
                    $"SubmittedAssignment with id '{submittedAssignmentId}' not found.");

            submission.MarkEvaluated(request.EvaluatorId);
            await _submissionRepository.SaveChangesAsync();

            return ToDto(submission, submission.Assignment);
        }

        public async Task<SubmittedAssignmentDTO> ReturnSubmissionAsync(Guid submittedAssignmentId)
        {
            var submission = await _submissionRepository.GetByIdAsync(submittedAssignmentId);
            if (submission is null)
                throw new KeyNotFoundException(
                    $"SubmittedAssignment with id '{submittedAssignmentId}' not found.");

            submission.Return();
            await _submissionRepository.SaveChangesAsync();

            return ToDto(submission, submission.Assignment);
        }

        // ── mapping helpers ───────────────────────────────────────────────────

        private static AssignmentExercise? FindSourceExerciseForQuestion(
            SubmittedAssignment submission, Guid submittedQuestionId)
        {
            var se = submission.SubmittedExercises
                .FirstOrDefault(x => x.SubmittedQuestions.Any(q => q.Id == submittedQuestionId));
            if (se is null) return null;

            return submission.Assignment?.AssignmentExercises
                .FirstOrDefault(ae => ae.Id == se.AssignmentExerciseId);
        }

        private static SubmittedAssignmentDTO ToDto(SubmittedAssignment submission, Assignment? source)
        {
            var maxPossible = submission.SubmittedExercises
                .SelectMany(se => se.SubmittedQuestions)
                .Sum(q => q.MaxPoints);

            return new SubmittedAssignmentDTO
            {
                Id = submission.Id,
                AssignmentId = submission.AssignmentId,
                StudentId = submission.StudentId,
                Status = submission.Status,
                EvaluatorId = submission.EvaluatorId,
                TotalScore = submission.TotalScore,
                MaxPossibleScore = maxPossible,
                SubmittedExercises = submission.SubmittedExercises
                    .Select(se => ToDto(se, source?.AssignmentExercises
                        .FirstOrDefault(ae => ae.Id == se.AssignmentExerciseId)))
                    .ToList()
            };
        }

        private static SubmittedExerciseDTO ToDto(SubmittedExercise se, AssignmentExercise? sourceExercise)
        {
            return new SubmittedExerciseDTO
            {
                Id = se.Id,
                SubmittedAssignmentId = se.SubmittedAssignmentId,
                AssignmentExerciseId = se.AssignmentExerciseId,
                OverallComment = se.OverallComment,
                TotalScore = se.TotalScore,
                SourceTitle = sourceExercise?.Title ?? string.Empty,
                SourceContent = sourceExercise?.Content ?? string.Empty,
                SourceOrder = sourceExercise?.Order ?? 0,
                SubmittedQuestions = se.SubmittedQuestions
                    .Select(sq => ToDto(sq, sourceExercise))
                    .ToList()
            };
        }

        private static SubmittedQuestionDTO ToDto(SubmittedQuestion sq, AssignmentExercise? sourceExercise)
        {
            var sourceQuestion = sourceExercise?.Questions
                .FirstOrDefault(q => q.Id == sq.AssignmentQuestionId);

            return new SubmittedQuestionDTO
            {
                Id = sq.Id,
                SubmittedExerciseId = sq.SubmittedExerciseId,
                AssignmentQuestionId = sq.AssignmentQuestionId,
                MaxPoints = sq.MaxPoints,
                PointsAwarded = sq.PointsAwarded,
                Comment = sq.Comment,
                ErrorTypeId = sq.ErrorTypeId,
                IsScored = sq.IsScored,
                SourceTitle = sourceQuestion?.Title ?? string.Empty,
                SourceContent = sourceQuestion?.Content ?? string.Empty,
                SourceOrder = sourceQuestion?.Order ?? 0
            };
        }
    }
}
