using Assignment_Domain.Shared;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Assignment_Domain.Entities;

public partial class SubmittedAssignment
{
    public Guid Id { get; private set; }
    public Guid AssignmentId { get; private set; }
    public Guid StudentId { get; private set; }
    public EvaluationStatus Status { get; private set; }
    public Guid? EvaluatorId { get; private set; }
    public byte[]? RowVersion { get; set; }

    private readonly List<SubmittedExercise> _submittedExercises = new();
    public IReadOnlyCollection<SubmittedExercise> SubmittedExercises => _submittedExercises;

    public int TotalScore => _submittedExercises.Sum(se => se.TotalScore);

    public virtual Assignment? Assignment { get; set; }

    // EF
    private SubmittedAssignment() { }

    private SubmittedAssignment(Guid assignmentId, Guid studentId)
    {
        Id = Guid.NewGuid();
        AssignmentId = assignmentId;
        StudentId = studentId;
        Status = EvaluationStatus.Pending;
    }

    /// <summary>
    /// Creates a SubmittedAssignment by snapshotting the structure of the given assignment.
    /// One SubmittedExercise is created per AssignmentExercise, and one SubmittedQuestion
    /// per AssignmentQuestion, carrying the question's current point value as MaxPoints.
    /// </summary>
    public static SubmittedAssignment CreateFromAssignment(Assignment assignment, Guid studentId)
    {
        if (assignment is null)
            throw new ArgumentNullException(nameof(assignment));
        if (studentId == Guid.Empty)
            throw new ArgumentException("StudentId is required.", nameof(studentId));

        var sa = new SubmittedAssignment(assignment.Id, studentId);

        foreach (var ae in assignment.AssignmentExercises)
        {
            var se = new SubmittedExercise(sa.Id, ae.Id);
            foreach (var q in ae.Questions)
            {
                se.AddSubmittedQuestion(q.Id, q.Points);
            }
            sa._submittedExercises.Add(se);
        }

        return sa;
    }

    public void ScoreQuestion(
        Guid submittedQuestionId,
        int points,
        string? comment,
        Guid? errorTypeId)
    {
        EnsureCanEdit();

        var target = FindQuestion(submittedQuestionId)
            ?? throw new KeyNotFoundException(
                $"SubmittedQuestion '{submittedQuestionId}' not found in this submission.");

        target.Score(points, comment, errorTypeId);
        AdvanceFromPendingIfNeeded();
    }

    public void SetExerciseComment(Guid submittedExerciseId, string? comment)
    {
        EnsureCanEdit();

        var se = _submittedExercises.FirstOrDefault(x => x.Id == submittedExerciseId)
            ?? throw new KeyNotFoundException(
                $"SubmittedExercise '{submittedExerciseId}' not found in this submission.");

        se.SetOverallComment(comment);
        AdvanceFromPendingIfNeeded();
    }

    public void MarkEvaluated(Guid evaluatorId)
    {
        if (evaluatorId == Guid.Empty)
            throw new ArgumentException("EvaluatorId is required.", nameof(evaluatorId));

        if (Status is not (EvaluationStatus.Pending or EvaluationStatus.InProgress))
            throw new InvalidOperationException(
                $"Cannot mark evaluated from status '{Status}'.");

        if (_submittedExercises.SelectMany(se => se.SubmittedQuestions).Any(q => !q.IsScored))
            throw new InvalidOperationException(
                "All questions must be scored before the submission can be marked evaluated.");

        Status = EvaluationStatus.Completed;
        EvaluatorId = evaluatorId;
    }

    public void Return()
    {
        if (Status != EvaluationStatus.Completed)
            throw new InvalidOperationException(
                "Only completed evaluations can be returned.");

        Status = EvaluationStatus.Returned;
    }

    // ── helpers ──────────────────────────────────────────────────────────────

    private SubmittedQuestion? FindQuestion(Guid submittedQuestionId)
    {
        foreach (var se in _submittedExercises)
        {
            var q = se.FindQuestion(submittedQuestionId);
            if (q is not null) return q;
        }
        return null;
    }

    private void EnsureCanEdit()
    {
        if (Status == EvaluationStatus.Returned)
            throw new InvalidOperationException(
                "Cannot modify a submission that has been returned to the student.");
    }

    private void AdvanceFromPendingIfNeeded()
    {
        if (Status == EvaluationStatus.Pending)
            Status = EvaluationStatus.InProgress;
    }
}