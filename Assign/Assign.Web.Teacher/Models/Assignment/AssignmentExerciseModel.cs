namespace Assign.Web.Teacher.Models.Assignment
{
    public class AssignmentExerciseModel
    {
        public Guid Id { get; set; }                  // AssignmentExercise.Id
        public Guid SourceExerciseId { get; set; }    // traceability back to Exercise module
        public string Title { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public int Order { get; set; }
        public DateTime SnapshotTakenAt { get; set; }
        public int TotalPoints => Questions.Sum(q => q.Points);
        public bool IsExpanded { get; set; } = false;
        public List<AssignmentQuestionModel> Questions { get; set; } = [];
    }
}
