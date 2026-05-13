using Assign.Web.Teacher.Models.Assignment;

namespace Assign.Web.Teacher.Services.MockData
{
    public static class AssignmentMockData
    {
        public static ExerciseCatalogModel SeedCatalog() => new()
        {
            SearchQuery = string.Empty,
            Results = new()
            {
                new() { Id = Guid.NewGuid(), Title = "Algebra Basics",
                        Content = "Solve linear equations step by step.", QuestionCount = 3 },
                new() { Id = Guid.NewGuid(), Title = "Geometry Pop Quiz",
                        Content = "Identify the shape and compute the missing angle.", QuestionCount = 1 },
                new() { Id = Guid.NewGuid(), Title = "Reading Reflection",
                        Content = "Write a paragraph reflecting on the chapter.", QuestionCount = 0 },
                new() { Id = Guid.NewGuid(), Title = "Word Problems",
                        Content = "Translate each story into an equation and solve it.", QuestionCount = 4 },
                new() { Id = Guid.NewGuid(), Title = "Vocabulary Check",
                        Content = "Define each term in your own words.", QuestionCount = 5 },
                new() { Id = Guid.NewGuid(), Title = "Lab Observations",
                        Content = "Record your observations from the experiment.", QuestionCount = 2 },
                new() { Id = Guid.NewGuid(), Title = "Quick Maths",
                        Content = "Short mental-arithmetic exercises.", QuestionCount = 1 },
                new() { Id = Guid.NewGuid(), Title = "Map Skills",
                        Content = "Annotate the provided map. Submit on paper.", QuestionCount = 0 },
            }
        };

        public static AssignmentExerciseModel SnapshotFor(ExerciseCatalogItemModel item) => new()
        {
            Id = Guid.NewGuid(),
            SourceExerciseId = item.Id,
            Title = item.Title,
            Content = item.Content,
            Order = 0,
            SnapshotTakenAt = DateTime.UtcNow,
            Questions = Enumerable.Range(0, item.QuestionCount)
                .Select(i => new AssignmentQuestionModel
                {
                    Id = Guid.NewGuid(),
                    Title = $"Question {i + 1}",
                    Content = $"Sample body for question {i + 1} in '{item.Title}'.",
                    Points = 0,
                    Order = i
                })
                .ToList()
        };

        public static ExercisePreviewModel PreviewFor(ExerciseCatalogItemModel item) => new()
        {
            Id = item.Id,
            Title = item.Title,
            Content = item.Content + " (full content visible in preview)",
            SolutionContent = item.QuestionCount > 0
                ? "This is the overall solution approach for this exercise."
                : null,
            SolutionVideoUrl = null,
            Questions = Enumerable.Range(0, item.QuestionCount)
                .Select(i => new ExercisePreviewQuestionModel
                {
                    Id = Guid.NewGuid(),
                    Title = $"Question {i + 1}",
                    Content = $"Detailed body for question {i + 1}.",
                    SolutionContent = $"Sample answer to question {i + 1}."
                })
                .ToList()
        };
    }
}