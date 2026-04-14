using Exercise_Domain.Entities;
using Exercise_Domain.shared;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

public class Exercise: Entity
{
    public string Title { get; private set; }
    public string Content { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public Guid CreatedByTeacherId { get; private set; }
    private readonly List<Question> _questions = new();
    public IReadOnlyCollection<Question> Questions => _questions;
    private readonly List<ExerciseKeyword> _exerciseKeywords = new();
    public IReadOnlyCollection<ExerciseKeyword> ExerciseKeywords => _exerciseKeywords;
    public ExerciseSolution? Solution { get; private set; }


    private Exercise() : base(Guid.NewGuid()) { } // Required by EF Core

    /// <summary>
    /// Represents an exercise created by a teacher, containing a title, content, creation timestamp, and associations with questions and keywords. Each exercise is uniquely identified and linked to the teacher who created it through the CreatedByTeacherId property. The constructor initializes the exercise with the provided title, content, and teacher ID, while also setting the creation timestamp to the current UTC time.
    /// </summary>
    /// <param name="title"></param>
    /// <param name="content"></param>
    /// <param name="teacherId"></param>
    public Exercise(string title, string content, Guid teacherId) : base(Guid.NewGuid())
    {
        Title = title;
        Content = content;
        CreatedByTeacherId = teacherId;
        CreatedAt = DateTime.UtcNow;
    }

    public void AddQuestion(string title, string content)
    {
        var question = new Question(this.Id, title, content);
        _questions.Add(question);
    }

    public void AddKeyword(Guid keywordId)
    {
        var exerciseKeyword = new ExerciseKeyword(this.Id, keywordId);
        _exerciseKeywords.Add(exerciseKeyword);
    }

    public void SetSolution(string content, string? videoUrl)
    {
        Solution = new ExerciseSolution(this.Id, content, videoUrl);
    }

    public void Update(string title, string content)
    {
        Title = title;
        Content = content;
    }


}