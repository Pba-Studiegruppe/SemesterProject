using System;

public class ExerciseSolution
{
    public Guid Id { get; private set; }

    public Guid ExerciseId { get; private set; }

    public string Content { get; private set; }

    public string? VideoUrl { get; private set; }

    private ExerciseSolution() { }

    public ExerciseSolution(Guid exerciseId, string content, string? videoUrl)
    {
        Id = Guid.NewGuid();
        ExerciseId = exerciseId;
        Content = content;
        VideoUrl = videoUrl;
    }
}