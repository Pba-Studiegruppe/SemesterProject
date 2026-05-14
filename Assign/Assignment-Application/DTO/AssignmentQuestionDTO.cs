namespace Assignment_Application.DTO
{
    public class AssignmentQuestionDTO
    {
        public Guid Id { get; set; }
        public Guid SourceQuestionId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public int Points { get; set; }
        public int Order { get; set; }
    }

    public class SetQuestionPointsRequest
    {
        public int Points { get; set; }
    }
}