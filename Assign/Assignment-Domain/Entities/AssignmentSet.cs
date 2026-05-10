using System;
using System.Collections.Generic;

namespace Assignment_Api;

public partial class AssignmentSet
{
    public Guid Id { get; set; }
    public Guid? CourseId { get; set; }
    public Guid? AssignmentId { get; set; }
    public Guid? GradesheetId { get; set; }
    public bool? IsPublihsed { get; set; }
    public bool? GradingPublished { get; set; }
    public bool? Inactive { get; set; }
    public string? Title { get; set; }
    public string? Description { get; set; }
    public DateOnly? CreatedAt { get; set; } //Todo
    public byte[]? RowVersion { get; set; }
    private readonly List<Assignment> _assignments = new();
    public virtual ICollection<Assignment> Assignments => _assignments;

    private readonly List<GradeSheet> _gradeSheets = new();
    public virtual ICollection<GradeSheet> GradeSheets => _gradeSheets;

    public AssignmentSet(Guid? courseId, string? title, string? description)
    {
        Id = Guid.NewGuid();
        CourseId = courseId;
        IsPublihsed = false;
        GradingPublished = false;
        Inactive = false;
        Title = title;
        Description = description;
    }

    public void UpdateTitle(string newTitle)
    {
        Title = newTitle;
    }

    public void UpdateDescription(string  newDescription) 
    { 
        Description = newDescription; 
    }
    public void AddAssignment(Assignment assignment)
    {
        if (assignment == null)
            throw new ArgumentNullException(nameof(assignment));

        if (_assignments.Contains(assignment))
            throw new InvalidOperationException("Assignment already exists in set");

        if (IsPublihsed == true)
            throw new InvalidOperationException("Cannot add assignment to published set");

        assignment.SetAssignmentSet(Id);
        _assignments.Add(assignment);
    }

    public void RemoveAssignment(Assignment assignment)
    {
        if (!_assignments.Contains(assignment))
            throw new KeyNotFoundException("Assignment not found in set");

        _assignments.Remove(assignment);
    }


    public void Publish()
    {
        if (CourseId == null)
            throw new InvalidOperationException("Cannot publish without course");

        if (!_assignments.Any())
            throw new InvalidOperationException("Cannot publish without assignments");

        if (_assignments.Any(a => !a.AssignmentExercises.Any()))
            throw new InvalidOperationException("All assignments must be valid");

        IsPublihsed = true;
    }

    public void AddGradeSheet()
    {
        if (!_assignments.Any())
            throw new InvalidOperationException("Cannot create gradesheet without assignments");

        _gradeSheets.Add(new GradeSheet());
    }

    /// <summary>
    /// Publishes Grades, Gradesheet must be made, All Assignments must have SubmittedAssignments
    /// </summary>
    public void PublishGrades()
    {
        if (!_gradeSheets.Any())
            throw new InvalidOperationException("Cannot publish grades without gradesheets");

        if (!_assignments.Any())
            throw new InvalidOperationException("Cannot publish grades without assignments");

        if (_assignments.Any(a => !a.SubmittedAssignments.Any()))
            throw new InvalidOperationException("All assignments must have submissions");

        GradingPublished = true;
    }
}
