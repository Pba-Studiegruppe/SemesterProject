namespace Assign.Web.Teacher.Models.Assignment
{
    public class ExerciseCatalogModel
    {
        public string SearchQuery { get; set; } = string.Empty;
        public List<ExerciseCatalogItemModel> Results { get; set; } = [];
        public bool IsLoading { get; set; } = false;
    }

    public class ExerciseCatalogItemModel
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public int QuestionCount { get; set; }
        public bool IsAlreadyInAssignment { get; set; } = false;
    }
}
