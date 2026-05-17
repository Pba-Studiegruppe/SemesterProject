using System;
using System.Collections.Generic;
using System.Linq;

namespace Assignment_Domain.Entities;

public partial class SubmittedExercise
{
    public Guid Id { get; private set; }
    public Guid SubmittedAssignmentId { get; private set; }
    public Guid AssignmentExerciseId { get; private set; }
    public string? OverallComment { get; private set; }
    public byte[]? RowVersion { get; set; }

    private readonly List<SubmittedQuestion> _submittedQuestions = new();
    public IReadOnlyCollection<SubmittedQuestion> SubmittedQuestions => _submittedQuestions;

    public int TotalScore => _submittedQuestions.Sum(q => q.PointsAwarded ?? 0);

    public virtual SubmittedAssignment? SubmittedAssignment { get; set; }

    // EF
    private SubmittedExercise() { }

    internal SubmittedExercise(Guid submittedAssignmentId, Guid assignmentExerciseId)
    {
        Id = Guid.NewGuid();
        SubmittedAssignmentId = submittedAssignmentId;
        AssignmentExerciseId = assignmentExerciseId;
    }

    internal SubmittedQuestion AddSubmittedQuestion(Guid assignmentQuestionId, int maxPoints)
    {
        var sq = new SubmittedQuestion(Id, assignmentQuestionId, maxPoints);
        _submittedQuestions.Add(sq);
        return sq;
    }

    internal SubmittedQuestion? FindQuestion(Guid submittedQuestionId)
        => _submittedQuestions.FirstOrDefault(q => q.Id == submittedQuestionId);

    internal void SetOverallComment(string? comment)
    {
        OverallComment = comment;
    }
}