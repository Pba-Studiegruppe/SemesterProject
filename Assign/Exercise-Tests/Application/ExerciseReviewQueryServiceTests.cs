using Exercise_Application.Implementations;
using Exercise_Application.Interfaces.Repositories;
using Exercise_Domain.Entities;
using FluentAssertions;
using Moq;

namespace Exercise_Tests.Application
{
    public class ExerciseReviewQueryServiceTests
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
        public async Task GetForReviewAsync_Should_Return_Null_When_Not_Found()
        {
            // Arrange
            var repoMock = new Mock<IExerciseRepository>();
            repoMock.Setup(r => r.GetForReviewAsync(It.IsAny<Guid>()))
                .ReturnsAsync((Exercise?)null);

            var service = new ExerciseReviewQueryService(repoMock.Object);

            // Act
            var result = await service.GetForReviewAsync(
                Guid.NewGuid());

            // Assert
            result.Should().BeNull();
        }


        [Fact]
        public async Task GetForReviewAsync_Should_Tolerate_Missing_Exercise_Solution()
        {
            // Arrange
            var teacherId = Guid.NewGuid();
            var exercise = BuildExercise(teacherId, withExerciseSolution: false);

            var repoMock = new Mock<IExerciseRepository>();
            repoMock.Setup(r => r.GetForReviewAsync(exercise.Id))
                .ReturnsAsync(exercise);

            var service = new ExerciseReviewQueryService(repoMock.Object);

            // Act
            var result = await service.GetForReviewAsync(exercise.Id);

            // Assert
            result.Should().NotBeNull();
            result!.Solution.Should().BeNull();
        }

        [Fact]
        public async Task GetForReviewAsync_Should_Tolerate_Question_Without_Solution()
        {
            // Arrange
            var teacherId = Guid.NewGuid();
            var exercise = BuildExercise(teacherId, questionCount: 1, withQuestionSolutions: false);

            var repoMock = new Mock<IExerciseRepository>();
            repoMock.Setup(r => r.GetForReviewAsync(exercise.Id))
                .ReturnsAsync(exercise);

            var service = new ExerciseReviewQueryService(repoMock.Object);

            // Act
            var result = await service.GetForReviewAsync(exercise.Id);

            // Assert
            result!.Questions.Should().ContainSingle()
                .Which.Solution.Should().BeNull();
        }
    }
}