using System;

public class ExerciseKeyword
{
    public Guid ExerciseId { get; private set; }

    public Guid KeywordId { get; private set; }

    private ExerciseKeyword() { }

    public ExerciseKeyword(Guid exerciseId, Guid keywordId)
    {
        ExerciseId = exerciseId;
        KeywordId = keywordId;
    }
}