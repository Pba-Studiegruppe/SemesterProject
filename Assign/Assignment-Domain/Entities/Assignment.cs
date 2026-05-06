using System;
using System.Collections.Generic;

namespace Assignment_Api;

public partial class Assignment
{
    public Guid Id { get; set; }
    public int? TotalPoints { get; set; }
    public Guid? AssignmentSetId { get; set; }
    public string? Title { get; set; }
    public string? Description { get; set; }
    public DateOnly? CreatedAt { get; set; }
    public byte[]? RowVersion { get; set; }

    private readonly List<AssignmentExercise> _assignmentExercises = new();
    public IReadOnlyCollection<AssignmentExercise> AssignmentExercises => _assignmentExercises;
    public virtual AssignmentSet? AssignmentSet { get; set; }

    private readonly List<SubmittedAssignment> _submittedAssignments = new();
    public IReadOnlyCollection<SubmittedAssignment> SubmittedAssignments => _submittedAssignments;

    public Assignment(Guid? assignmentSetId, string? title, string? description)
    {
        AssignmentSetId = assignmentSetId;
        Title = title;
        Description = description;
    }

    public void AddExercise(Guid exerciseId)
    {
        var exercise = new AssignmentExercise();
        _assignmentExercises.Add(exercise);
    }

    public void RemoveExercise(Guid exerciseId)
    {
        var exercise = _assignmentExercises.Find(q => q.ExerciseId == exerciseId);
        if (exercise == null) { throw new ArgumentException("exercise not found"); }

        _assignmentExercises.Remove(exercise);
    }

    public void AddSubmittedAssignment(SubmittedAssignment submittedAssignment)
    {
        //validations
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
}
