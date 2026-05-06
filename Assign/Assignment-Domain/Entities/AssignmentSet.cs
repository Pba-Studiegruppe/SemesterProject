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

    }

    public void RemoveAssignment(Assignment assignment) 
    { 

    }


    public bool Publish()
    {
        if (Assignments == null) { return false; }
        if (CourseId == null) { return false; }

        IsPublihsed = true;
        return true;
    }

    public void AddGradeSheet()
    {
        if (Assignments == null) { return; }

    }

    /// <summary>
    /// Publishes Grades, Gradesheet must be made, All Assignments must have SubmittedAssignments
    /// </summary>
    public void PublishGrades()
    {
        if (GradeSheets == null) { return; }
        if (Assignments == null) { return; }

       

    }
}
