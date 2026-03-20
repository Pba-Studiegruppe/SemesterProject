using System.Collections.Generic;
using System;
using Exercise_Domain.Entities;

public class Exercise
{
    public Guid Id { get; private set; }

    public string Title { get; private set; }

    public string Content { get; private set; }

    public DateTime CreatedAt { get; private set; }

    public Guid CreatedByTeacherId { get; private set; }

    private readonly List<Question> _questions = new();
    public IReadOnlyCollection<Question> Questions => _questions;

    private readonly List<ExerciseKeyword> _exerciseKeywords = new();
    public IReadOnlyCollection<ExerciseKeyword> ExerciseKeywords => _exerciseKeywords;

    public ExerciseSolution? Solution { get; private set; }

    private Exercise() { } // Required by EF Core

    public Exercise(string title, string content, Guid teacherId)
    {
        Id = Guid.NewGuid();
        Title = title;
        Content = content;
        CreatedByTeacherId = teacherId;
        CreatedAt = DateTime.UtcNow;
    }

    public void AddQuestion(string title, string content)
    {
        throw new NotImplementedException();
    }
}