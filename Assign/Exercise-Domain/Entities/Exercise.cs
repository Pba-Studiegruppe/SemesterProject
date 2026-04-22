using Exercise_Domain.Entities;
using Exercise_Domain.shared;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
public class Exercise : Entity
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
    }

    public void SetSolution(string content, string? videoUrl)
    {
        Solution = new ExerciseSolution(this.Id, content, videoUrl);
    }

    public void RemoveSolution()
    {
        Solution = null;
    }

    public void UpdateTitle(string title)
    {
        Title = title;
    }

    public void UpdateContent(string content)
    {
        Content = content;
    }

    public void AddQuestion(string title, string content, string solutionTitle)
    {
        var question = new Question(this.Id, title, content);
        question.SetSolution(solutionTitle);
        _questions.Add(question);
    }

    public void RemoveQuestion(Guid questionId)
    {
        var question = _questions.Find(q => q.Id == questionId);
        if (question == null) throw new ArgumentException("Question not found");

        _questions.Remove(question);
    }

    public void UpdateQuestion(Guid questionId, string title, string content)
    {
        var question = _questions.Find(q => q.Id == questionId);
        if (question == null) throw new ArgumentException("Question not found");

        question.Update(title, content);
    }

    public void AddKeyword(Guid keywordId)
    {
        var exerciseKeyword = new ExerciseKeyword(this.Id, keywordId);
        if (_exerciseKeywords.Exists(ek => ek.KeywordId == keywordId))
        {
            return;
        }

        _exerciseKeywords.Add(exerciseKeyword);
    }

    public void RemoveKeyword(Guid keywordId)
    {
        var exerciseKeyword = _exerciseKeywords.Find(ek => ek.KeywordId == keywordId);
        if (exerciseKeyword != null)
        {
            _exerciseKeywords.Remove(exerciseKeyword);
        }
    }

    public void SetCreatedAt(DateTime createdAt)
    {
        CreatedAt = createdAt;
    }




}