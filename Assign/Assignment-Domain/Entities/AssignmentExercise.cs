using Assignment_Api;
using System;
using System.Collections.Generic;

namespace Assignment_Domain.Entities;

public class AssignmentExercise
{
    public Guid Id { get; private set; }
    public Guid AssignmentId { get; private set; }
    public Guid SourceExerciseId { get; private set; }
    public string Title { get; private set; } = string.Empty;
    public string Content { get; private set; } = string.Empty;
    public int Order { get; private set; }
    public DateTime SnapshotTakenAt { get; private set; }
    public byte[] RowVersion { get; private set; } = [];

    private readonly List<AssignmentQuestion> _questions = [];
    public IReadOnlyList<AssignmentQuestion> Questions => _questions;

    public int TotalPoints => _questions.Sum(q => q.Points);

    public virtual Assignment Assignment { get; set; } = null!;

    private AssignmentExercise() { }

    internal AssignmentExercise(
        Guid assignmentId,
        Guid sourceExerciseId,
        string title,
        string content,
        int order,
        DateTime snapshotTakenAt)
    {
        if (string.IsNullOrWhiteSpace(title))
            throw new ArgumentException("Title is required.", nameof(title));

        Id = Guid.NewGuid();
        AssignmentId = assignmentId;
        SourceExerciseId = sourceExerciseId;
        Title = title;
        Content = content ?? string.Empty;
        Order = order;
        SnapshotTakenAt = snapshotTakenAt;
    }

    internal AssignmentQuestion AddQuestionFromSnapshot(
        Guid sourceQuestionId,
        string title,
        string content,
        int order,
        int points = 0)
    {
        var q = new AssignmentQuestion(Id, sourceQuestionId, title, content, order, points);
        _questions.Add(q);
        return q;
    }

    public void RemoveQuestion(Guid questionId)
    {
        var question = _questions.FirstOrDefault(q => q.Id == questionId);
        if (question is null)
            throw new KeyNotFoundException(
                $"Question with id '{questionId}' not found in this exercise.");

        _questions.Remove(question); // hard removal, per the design
    }

    public void SetQuestionPoints(Guid questionId, int points)
    {
        var question = _questions.FirstOrDefault(q => q.Id == questionId);
        if (question is null)
            throw new KeyNotFoundException(
                $"Question with id '{questionId}' not found in this exercise.");

        question.SetPoints(points);
    }
}
