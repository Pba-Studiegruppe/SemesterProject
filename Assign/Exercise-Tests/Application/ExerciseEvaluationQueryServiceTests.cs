using Exercise_Application.Implementations;
using Exercise_Application.Interfaces.Repositories;
using Exercise_Domain.Entities;
using FluentAssertions;
using Moq;

namespace Exercise_Tests.Application
{
    public class ExerciseEvaluationQueryServiceTests
    {
        private static Exercise BuildExercise(
            Guid teacherId,
            int questionCount = 2,
            bool withExerciseSolution = true,
            bool withQuestionSolutions = true)
        {
            var exercise = new Exercise("Title", "Content", teacherId);
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
        public async Task GetForEvaluationAsync_Should_Return_Null_When_Not_Found()
        {
            // Arrange
            var repoMock = new Mock<IExerciseRepository>();
            repoMock.Setup(r => r.GetForEvaluationAsync(It.IsAny<Guid>()))
                .ReturnsAsync((Exercise?)null);

            var service = new ExerciseEvaluationQueryService(repoMock.Object);

            // Act
            var result = await service.GetForEvaluationAsync(
                Guid.NewGuid(),
                Guid.NewGuid());

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public async Task GetForEvaluationAsync_Should_Throw_When_Teacher_Does_Not_Own()
        {
            // Arrange
            var ownerId = Guid.NewGuid();
            var otherTeacher = Guid.NewGuid();
            var exercise = BuildExercise(ownerId);

            var repoMock = new Mock<IExerciseRepository>();
            repoMock.Setup(r => r.GetForEvaluationAsync(exercise.Id))
                .ReturnsAsync(exercise);

            var service = new ExerciseEvaluationQueryService(repoMock.Object);

            // Act
            Func<Task> act = () =>
                service.GetForEvaluationAsync(exercise.Id, otherTeacher);

            // Assert
            await act.Should()
                .ThrowAsync<UnauthorizedAccessException>()
                .WithMessage($"*{otherTeacher}*{exercise.Id}*");
        }

        [Fact]
        public async Task GetForEvaluationAsync_Should_Return_Projection_With_Solutions_When_Authorized()
        {
            // Arrange
            var teacherId = Guid.NewGuid();
            var exercise = BuildExercise(teacherId, questionCount: 2);

            var repoMock = new Mock<IExerciseRepository>();
            repoMock.Setup(r => r.GetForEvaluationAsync(exercise.Id))
                .ReturnsAsync(exercise);

            var service = new ExerciseEvaluationQueryService(repoMock.Object);

            // Act
            var result = await service.GetForEvaluationAsync(exercise.Id, teacherId);

            // Assert
            result.Should().NotBeNull();
            result!.Id.Should().Be(exercise.Id);
            result.Solution.Should().NotBeNull();
            result.Solution!.Content.Should().Be("Exercise solution");
            result.Solution.VideoUrl.Should().Be("https://video");

            result.Questions.Should().HaveCount(2);
            result.Questions.Should().AllSatisfy(q =>
            {
                q.Solution.Should().NotBeNull();
                q.Solution!.Content.Should().StartWith("Q");
            });
        }

        [Fact]
        public async Task GetForEvaluationAsync_Should_Tolerate_Missing_Exercise_Solution()
        {
            // Arrange
            var teacherId = Guid.NewGuid();
            var exercise = BuildExercise(teacherId, withExerciseSolution: false);

            var repoMock = new Mock<IExerciseRepository>();
            repoMock.Setup(r => r.GetForEvaluationAsync(exercise.Id))
                .ReturnsAsync(exercise);

            var service = new ExerciseEvaluationQueryService(repoMock.Object);

            // Act
            var result = await service.GetForEvaluationAsync(exercise.Id, teacherId);

            // Assert
            result.Should().NotBeNull();
            result!.Solution.Should().BeNull();
        }

        [Fact]
        public async Task GetForEvaluationAsync_Should_Tolerate_Question_Without_Solution()
        {
            // Arrange
            var teacherId = Guid.NewGuid();
            var exercise = BuildExercise(teacherId, questionCount: 1, withQuestionSolutions: false);

            var repoMock = new Mock<IExerciseRepository>();
            repoMock.Setup(r => r.GetForEvaluationAsync(exercise.Id))
                .ReturnsAsync(exercise);

            var service = new ExerciseEvaluationQueryService(repoMock.Object);

            // Act
            var result = await service.GetForEvaluationAsync(exercise.Id, teacherId);

            // Assert
            result!.Questions.Should().ContainSingle()
                .Which.Solution.Should().BeNull();
        }
    }
}