using System;

namespace Assignment_Domain.Entities;

public class SubmittedQuestion
{
    public Guid Id { get; private set; }
    public Guid SubmittedExerciseId { get; private set; }
    public Guid AssignmentQuestionId { get; private set; }
    public int MaxPoints { get; private set; }
    public int? PointsAwarded { get; private set; }
    public string? Comment { get; private set; }
    public Guid? ErrorTypeId { get; private set; }
    public byte[] RowVersion { get; private set; } = [];

    public bool IsScored => PointsAwarded.HasValue;

    // EF
    private SubmittedQuestion() { }

    internal SubmittedQuestion(Guid submittedExerciseId, Guid assignmentQuestionId, int maxPoints)
    {
        if (maxPoints < 0)
            throw new ArgumentOutOfRangeException(nameof(maxPoints),
                "MaxPoints cannot be negative.");

        Id = Guid.NewGuid();
        SubmittedExerciseId = submittedExerciseId;
        AssignmentQuestionId = assignmentQuestionId;
        MaxPoints = maxPoints;
    }

    internal void Score(int points, string? comment, Guid? errorTypeId)
    {
        if (points < 0)
            throw new ArgumentOutOfRangeException(nameof(points),
                "Points cannot be negative.");
        if (points > MaxPoints)
            throw new ArgumentOutOfRangeException(nameof(points),
                $"Points cannot exceed MaxPoints ({MaxPoints}).");

        PointsAwarded = points;
        Comment = comment;
        ErrorTypeId = errorTypeId;
    }
}