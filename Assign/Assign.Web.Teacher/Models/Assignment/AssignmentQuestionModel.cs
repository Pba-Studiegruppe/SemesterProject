namespace Assign.Web.Teacher.Models.Assignment
{
    public class AssignmentQuestionModel
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public int Points { get; set; }
        public int Order { get; set; }

        // UI-only: tracks whether the points field is being actively edited
        // so you can show a pending/unsaved indicator if you want one
        public bool HasPendingPointsChange { get; set; } = false;
    }
}

