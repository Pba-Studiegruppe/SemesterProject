using System;
using System.Collections.Generic;

namespace Assignment_Api;

public partial class Assignment
{
    public Guid Id { get; set; }
    public int? TotalPoints { get; private set; }
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
        Title = title;
        Description = description;
    }

    public void AddExercise(Guid exerciseId)
    {
        if (_assignmentExercises.Any(e => e.ExerciseId == exerciseId))
        {
            throw new InvalidOperationException($"Exercise with id '{exerciseId}' already exists in assignment.");
        }

        var exercise = new AssignmentExercise(Id, exerciseId);
        _assignmentExercises.Add(exercise);
    }

    public void RemoveExercise(Guid exerciseId)
    {
        var exercise = _assignmentExercises.FirstOrDefault(e => e.ExerciseId == exerciseId);

        if (exercise == null)
        {
            throw new KeyNotFoundException($"Exercise with id '{exerciseId}' was not found in assignment.");
        }

        _assignmentExercises.Remove(exercise);
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
