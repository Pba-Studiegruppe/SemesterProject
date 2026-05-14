using Assignment_Domain.Entities;
using Assignment_Domain.SnapShots;
using System;
using System.Collections.Generic;

namespace Assignment_Domain.Entities;

public partial class Assignment
{
    public Guid Id { get; set; }
    public int TotalPoints => _assignmentExercises.Sum(ae => ae.TotalPoints);
    public Guid? AssignmentSetId { get; set; }
    public string? Title { get; private set; }
    public string? Description { get; private set; }
    public DateOnly? CreatedAt { get; private set; }
    public byte[]? RowVersion { get; set; }

    private readonly List<AssignmentExercise> _assignmentExercises = new();
    public IReadOnlyCollection<AssignmentExercise> AssignmentExercises => _assignmentExercises;
    public virtual AssignmentSet? AssignmentSet { get; set; }

    private readonly List<SubmittedAssignment> _submittedAssignments = new();
    public IReadOnlyCollection<SubmittedAssignment> SubmittedAssignments => _submittedAssignments;

    public Assignment(string? title, string? description)
    {
        Id = Guid.NewGuid();
        Title = title;
        Description = description;
    }

    public AssignmentExercise AddExerciseFromSnapshot(ExerciseSnapshotInput snapshot)
    {
        if (snapshot is null)
            throw new ArgumentNullException(nameof(snapshot));

        if (_assignmentExercises.Any(ae => ae.SourceExerciseId == snapshot.SourceExerciseId))
            throw new InvalidOperationException(
                $"Exercise '{snapshot.SourceExerciseId}' is already in this assignment.");

        var nextOrder = _assignmentExercises.Count == 0
            ? 0
            : _assignmentExercises.Max(ae => ae.Order) + 1;

        var ae = new AssignmentExercise(
            Id,
            snapshot.SourceExerciseId,
            snapshot.Title,
            snapshot.Content,
            nextOrder,
            DateTime.UtcNow);

        var qOrder = 0;
        foreach (var q in snapshot.Questions)
        {
            ae.AddQuestionFromSnapshot(q.SourceQuestionId, q.Title, q.Content, qOrder++);
        }

        _assignmentExercises.Add(ae);
        return ae;
    }

    public void RemoveExercise(Guid assignmentExerciseId)
    {
        var ae = _assignmentExercises.FirstOrDefault(e => e.Id == assignmentExerciseId);
        if (ae is null)
            throw new KeyNotFoundException(
                $"AssignmentExercise '{assignmentExerciseId}' not found.");

        _assignmentExercises.Remove(ae);
    }

    public void AddSubmittedAssignment(SubmittedAssignment submittedAssignment)
    {
        //Todo validations of submitted assignment
        // Has to filled out correctly, no duplicates, etc
        _submittedAssignments.Add(submittedAssignment);
    }

    public void UpdateTitle(string title)
    {
        Title = title;
    }

    public void UpdateDescription(string description)
    {
        Description = description;
    }

    internal void SetAssignmentSet(Guid id)
    {
        AssignmentSetId = id;
    }
}
