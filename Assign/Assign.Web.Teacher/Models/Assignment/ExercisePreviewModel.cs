namespace Assign.Web.Teacher.Models.Assignment
{
    public class ExercisePreviewModel
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public string? SolutionContent { get; set; }
        public string? SolutionVideoUrl { get; set; }
        public List<ExercisePreviewQuestionModel> Questions { get; set; } = [];
    }

    public class ExercisePreviewQuestionModel
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public string? SolutionContent { get; set; }
    }
}
