using Exercise_Application.Implementations;
using Exercise_Application.Interfaces.Repositories;
using Exercise_Domain.Entities;
using FluentAssertions;
using Moq;

namespace Exercise_Tests.Application
{
    public class ExerciseSnapshotQueryServiceTests
    {
        private static Exercise BuildExercise(
            string title = "Ex Title",
            string content = "Ex Content",
            int questionCount = 2,
            bool withExerciseSolution = true,
            bool withQuestionSolutions = true)
        {
            var exercise = new Exercise(title, content, Guid.NewGuid());
            for (var i = 0; i < questionCount; i++)
            {
                exercise.AddQuestion($"Q{i}", $"Content {i}");
                if (withQuestionSolutions)
                    exercise.Questions.Last().SetSolution($"Q{i} solution");
            }

            if (withExerciseSolution)
                exercise.SetSolution("Exercise solution", "https://video");

            return exercise;
        }

        [Fact]
        public async Task GetForSnapshotAsync_Should_Return_Null_When_Not_Found()
        {
            // Arrange
            var repoMock = new Mock<IExerciseRepository>();
            repoMock.Setup(r => r.GetForSnapshotAsync(It.IsAny<Guid>()))
                .ReturnsAsync((Exercise?)null);

            var service = new ExerciseSnapshotQueryService(repoMock.Object);

            // Act
            var result = await service.GetForSnapshotAsync(Guid.NewGuid());

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public async Task GetForSnapshotAsync_Should_Project_Title_Content_And_Id()
        {
            // Arrange
            var exercise = BuildExercise(title: "Title", content: "Content", questionCount: 0);
            var repoMock = new Mock<IExerciseRepository>();
            repoMock.Setup(r => r.GetForSnapshotAsync(exercise.Id))
                .ReturnsAsync(exercise);

            var service = new ExerciseSnapshotQueryService(repoMock.Object);

            // Act
            var result = await service.GetForSnapshotAsync(exercise.Id);

            // Assert
            result.Should().NotBeNull();
            result!.Id.Should().Be(exercise.Id);
            result.Title.Should().Be("Title");
            result.Content.Should().Be("Content");
            result.Questions.Should().BeEmpty();
        }

        [Fact]
        public async Task GetForSnapshotAsync_Should_Project_All_Questions()
        {
            // Arrange
            var exercise = BuildExercise(questionCount: 3, withQuestionSolutions: false);
            var repoMock = new Mock<IExerciseRepository>();
            repoMock.Setup(r => r.GetForSnapshotAsync(exercise.Id))
                .ReturnsAsync(exercise);

            var service = new ExerciseSnapshotQueryService(repoMock.Object);

            // Act
            var result = await service.GetForSnapshotAsync(exercise.Id);

            // Assert
            result!.Questions.Should().HaveCount(3);
            result.Questions.Select(q => q.Id)
                .Should().Equal(exercise.Questions.Select(q => q.Id));
            result.Questions.Select(q => q.Title)
                .Should().Equal("Q0", "Q1", "Q2");
        }

        [Fact]
        public async Task GetForSnapshotAsync_Projection_Type_Should_Not_Carry_Solutions()
        {
            // This test is a structural sentinel: if anyone adds a Solution
            // property to the snapshot projection, it fails by reflection.
            var props = typeof(Exercise_Application.Projections.ExerciseSnapshotProjection)
                .GetProperties()
                .Select(p => p.Name);

            props.Should().NotContain("Solution");

            var qProps = typeof(Exercise_Application.Projections.QuestionSnapshotProjection)
                .GetProperties()
                .Select(p => p.Name);

            qProps.Should().NotContain("Solution");
        }
    }
}