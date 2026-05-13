namespace Assign.Web.Teacher.Models.Assignment
{
    public record RemoveQuestionEventArgs(Guid AssignmentExerciseId, Guid QuestionId);
    public record SetQuestionPointsEventArgs(Guid AssignmentExerciseId, Guid QuestionId, int Points);
}