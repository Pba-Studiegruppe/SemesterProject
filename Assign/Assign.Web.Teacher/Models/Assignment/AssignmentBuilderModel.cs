using System.Text.Json.Serialization;

namespace Assign.Web.Teacher.Models.Assignment
{
    public class AssignmentBuilderModel
    {
        [JsonPropertyName("id")]
        public Guid AssignmentId { get; set; }
        public Guid? AssignmentSetId { get; set; }   // new
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int TotalPoints => Exercises.Sum(e => e.TotalPoints);
        public List<AssignmentExerciseModel> Exercises { get; set; } = [];
    }
}