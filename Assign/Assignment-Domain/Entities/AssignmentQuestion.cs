using System;

namespace Assignment_Domain.Entities;

public class AssignmentQuestion
{
    public Guid Id { get; private set; }
    public Guid AssignmentExerciseId { get; private set; }
    public Guid SourceQuestionId { get; private set; }
    public string Title { get; private set; } = string.Empty;
    public string Content { get; private set; } = string.Empty;
    public int Points { get; private set; }
    public int Order { get; private set; }
    public byte[] RowVersion { get; private set; } = [];

    private AssignmentQuestion() { } // EF

    internal AssignmentQuestion(
        Guid assignmentExerciseId,
        Guid sourceQuestionId,
        string title,
        string content,
        int order,
        int points = 0)
    {
        if (string.IsNullOrWhiteSpace(title))
            throw new ArgumentException("Title is required.", nameof(title));
        if (points < 0)
            throw new ArgumentOutOfRangeException(nameof(points));

        Id = Guid.NewGuid();
        AssignmentExerciseId = assignmentExerciseId;
        SourceQuestionId = sourceQuestionId;
        Title = title;
        Content = content ?? string.Empty;
        Order = order;
        Points = points;
    }

    internal void SetPoints(int points)
    {
        if (points < 0)
            throw new ArgumentOutOfRangeException(nameof(points));
        Points = points;
    }
}
