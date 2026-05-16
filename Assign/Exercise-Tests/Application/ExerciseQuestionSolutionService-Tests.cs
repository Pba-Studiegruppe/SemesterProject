using Exercise_Application.DTO;
using Exercise_Application.Implementations;
using Exercise_Application.Interfaces.Repositories;
using Exercise_Domain.Entities;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exercise_Tests.Application
{
    public class AddQuestionSolution_Tests
    {
        [Fact]
        public async Task AddQuestionSolution_Should_Set_Solution_For_Question()
        {
            // Arrange
            var mockRepo = new Mock<IExerciseRepository>();
            var expectedExercise = new Exercise("title", "content", Guid.NewGuid());
            expectedExercise.AddQuestion("qTitle", "qContent");
            var questionSolutionRequest = new CreateQuestionSolutionRequest()
            {
                QuestionId = expectedExercise.Questions.First().Id,
                Content = "Solution",
            };

            mockRepo.Setup(r => r.GetByIdAsync(expectedExercise.Id))
                .ReturnsAsync(expectedExercise);
            mockRepo.Setup(r => r.UpdateAsync(expectedExercise, expectedExercise.RowVersion))
                .ReturnsAsync(expectedExercise);

            var service = new ExerciseQuestionSolutionService(mockRepo.Object);

            // Act
            var result = await service.SetQuestionSolutionAsync(expectedExercise.Id, questionSolutionRequest);

            // Assert
            var question = result.Questions.First(q => q.Id == questionSolutionRequest.QuestionId);

            Assert.NotNull(question.Solution);
            Assert.Equal(questionSolutionRequest.Content, question.Solution.Content);
            Assert.Equal(questionSolutionRequest.QuestionId, question.Solution.QuestionId);

            mockRepo.Verify(r => r.UpdateAsync(expectedExercise, expectedExercise.RowVersion), Times.Once);
        }
        [Fact]
        public async Task AddQuestionSolution_Should_Throw_When_Solution_Already_Exists()
        {
            // Arrange
            var mockRepo = new Mock<IExerciseRepository>();
            var expectedExercise = new Exercise("title", "content", Guid.NewGuid());
            expectedExercise.AddQuestion("qTitle", "qContent");
            expectedExercise.Questions.First().SetSolution("Solution");

            var questionSolutionRequest = new CreateQuestionSolutionRequest()
            {
                QuestionId = expectedExercise.Questions.First().Id,
                Content = "New Solution",
            };

            mockRepo.Setup(r => r.GetByIdAsync(expectedExercise.Id))
                .ReturnsAsync(expectedExercise);

            var service = new ExerciseQuestionSolutionService(mockRepo.Object);

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() =>
                service.SetQuestionSolutionAsync(expectedExercise.Id, questionSolutionRequest));

            mockRepo.Verify(r => r.UpdateAsync(It.IsAny<Exercise>(), It.IsAny<byte[]>()), Times.Never);
        }

        [Fact]
        public async Task AddQuestionSolution_Should_Throw_When_Setting_Solution_For_Nonexistent_Question()
        {
            // Arrange
            var mockRepo = new Mock<IExerciseRepository>();
            var expectedExercise = new Exercise("title", "content", Guid.NewGuid());
            var questionSolutionRequest = new CreateQuestionSolutionRequest()
            {
                QuestionId = Guid.NewGuid(),
                Content = "New Solution",
            };

            mockRepo.Setup(r => r.GetByIdAsync(expectedExercise.Id))
                .ReturnsAsync(expectedExercise);
            mockRepo.Setup(r => r.UpdateAsync(expectedExercise, expectedExercise.RowVersion))
                .ReturnsAsync(expectedExercise);

            var service = new ExerciseQuestionSolutionService(mockRepo.Object);

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() =>
                service.SetQuestionSolutionAsync(expectedExercise.Id, questionSolutionRequest));

            mockRepo.Verify(r => r.UpdateAsync(It.IsAny<Exercise>(), It.IsAny<byte[]>()), Times.Never);
        }
    }

    public class UpdateQuestionSolution_Tests
    {
        [Fact]
        public async Task UpdateQuestionSolution_Should_Update_Solution_For_Question()
        {
            // Arrange
            var mockRepo = new Mock<IExerciseRepository>();
            var expectedExercise = new Exercise("title", "content", Guid.NewGuid());
            expectedExercise.AddQuestion("qTitle", "qContent");
            expectedExercise.Questions.First().SetSolution("Solution");

            var questionSolutionRequest = new UpdateQuestionSolutionRequest()
            {
                QuestionId = expectedExercise.Questions.First().Id,
                Content = "New solution",
                RowVersion = expectedExercise.RowVersion
            };

            mockRepo.Setup(r => r.GetByIdAsync(expectedExercise.Id))
                .ReturnsAsync(expectedExercise);
            mockRepo.Setup(r => r.UpdateAsync(expectedExercise, expectedExercise.RowVersion))
                .ReturnsAsync(expectedExercise);

            var service = new ExerciseQuestionSolutionService(mockRepo.Object);

            // Act
            var result = await service.UpdateQuestionSolutionAsync(expectedExercise.Id, questionSolutionRequest);

            // Assert
            var question = result.Questions.First(q => q.Id == questionSolutionRequest.QuestionId);

            Assert.NotNull(question.Solution);
            Assert.Equal("New solution", question.Solution.Content);
            Assert.Equal(questionSolutionRequest.QuestionId, question.Solution.QuestionId);

            mockRepo.Verify(r => r.UpdateAsync(expectedExercise, expectedExercise.RowVersion), Times.Once);
        }

        [Fact]
        public async Task UpdateQuestionSolution_Should_Throw_When_Updating_Solution_For_Nonexistent_Question()
        {
            // Arrange
            var mockRepo = new Mock<IExerciseRepository>();
            var exercise = new Exercise("title", "content", Guid.NewGuid());

            var request = new UpdateQuestionSolutionRequest()
            {
                QuestionId = Guid.NewGuid(),
                Content = "New solution",
                RowVersion = exercise.RowVersion
            };

            mockRepo.Setup(r => r.GetByIdAsync(exercise.Id))
                .ReturnsAsync(exercise);

            var service = new ExerciseQuestionSolutionService(mockRepo.Object);

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() =>
                service.UpdateQuestionSolutionAsync(exercise.Id, request));

            mockRepo.Verify(r => r.UpdateAsync(It.IsAny<Exercise>(), It.IsAny<byte[]>()), Times.Never);
        }
    }


    public class RemoveQuestionSolution_Tests
    {
        [Fact]
        public async Task RemoveQuestionSolution_Should_Remove_Solution_From_Question()
        {
            // Arrange
            var mockRepo = new Mock<IExerciseRepository>();
            var expectedExercise = new Exercise("title", "content", Guid.NewGuid());
            expectedExercise.AddQuestion("qTitle", "qContent");
            expectedExercise.Questions.First().SetSolution("Solution");

            var questionSolutionRequest = new RemoveQuestionSolutionRequest()
            {
                QuestionId = expectedExercise.Questions.First().Id,
                RowVersion = expectedExercise.RowVersion
            };

            mockRepo.Setup(r => r.GetByIdAsync(expectedExercise.Id))
                .ReturnsAsync(expectedExercise);
            mockRepo.Setup(r => r.UpdateAsync(expectedExercise, expectedExercise.RowVersion))
                .ReturnsAsync(expectedExercise);

            var service = new ExerciseQuestionSolutionService(mockRepo.Object);

            // Act
            var result = await service.RemoveQuestionSolutionAsync(expectedExercise.Id, questionSolutionRequest);

            // Assert
            var question = result.Questions.First(q => q.Id == questionSolutionRequest.QuestionId);

            Assert.Null(question.Solution);

            mockRepo.Verify(r => r.UpdateAsync(expectedExercise, expectedExercise.RowVersion), Times.Once);

        }
        [Fact]
        public async Task RemoveQuestionSolution_Should_Throw_When_Removing_Solution_For_Nonexistent_Question()
        {
            // Arrange
            var mockRepo = new Mock<IExerciseRepository>();
            var exercise = new Exercise("title", "content", Guid.NewGuid());

            var request = new RemoveQuestionSolutionRequest()
            {
                QuestionId = Guid.NewGuid(),
                RowVersion = exercise.RowVersion
            };

            mockRepo.Setup(r => r.GetByIdAsync(exercise.Id))
                .ReturnsAsync(exercise);

            var service = new ExerciseQuestionSolutionService(mockRepo.Object);

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() =>
                service.RemoveQuestionSolutionAsync(exercise.Id, request));

            mockRepo.Verify(r => r.UpdateAsync(It.IsAny<Exercise>(), It.IsAny<byte[]>()), Times.Never);
        }
    }
}
