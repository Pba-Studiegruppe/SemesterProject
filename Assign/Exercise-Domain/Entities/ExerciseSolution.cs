using System;
using System.ComponentModel.DataAnnotations;

public class ExerciseSolution
{
    public Guid Id { get; private set; }
    public Guid ExerciseId { get; private set; }
    public string Content { get; private set; }
    public string? VideoUrl { get; private set; }
    [Timestamp] public byte[] RowVersion { get; private set; } = [];

    private ExerciseSolution() { }

    public ExerciseSolution(Guid exerciseId, string content, string? videoUrl)
    {
        Id = Guid.NewGuid();
        ExerciseId = exerciseId;
        Content = content;
        VideoUrl = videoUrl;
    }

    public void Update(string content, string? videoUrl)
    {
        Content = content;
        VideoUrl = videoUrl;
    }
}