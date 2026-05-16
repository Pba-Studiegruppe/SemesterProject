using System;
using System.Collections.Generic;

namespace Assignment_Application.DTO
{
    public class AssignmentExerciseDTO
    {
        public Guid Id { get; set; }
        public Guid AssignmentId { get; set; }
        public Guid SourceExerciseId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public int Order { get; set; }
        public DateTime SnapshotTakenAt { get; set; }
        public int TotalPoints { get; set; }
        public List<AssignmentQuestionDTO> Questions { get; set; } = [];
    }

    public class CreateAssignmentExerciseRequest
    {
        public Guid ExerciseId { get; set; } // source exercise id
    }

    public class RemoveAssignmentExerciseRequest
    {
        public Guid AssignmentExerciseId { get; set; }
    }
}