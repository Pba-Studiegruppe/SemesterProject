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
    public class AddQuestionTests
    {
        [Fact]
        public async Task AddQuestionAsync_Should_Call_ExerciseRepository_GetByIdAsync()
        {
            // Arrange
            var mockRepo = new Mock<IExerciseRepository>();
            var expectedExercise = new Exercise("title", "content", Guid.NewGuid());
            var expectedQuestion = new Question("QuestionTitle", "QuestionContent");
            var addQuestionRequests = new List<CreateQuestionRequest>
            {
                new CreateQuestionRequest {Title = expectedQuestion.Title, Content = expectedQuestion.Content}
            };

            mockRepo.Setup(r => r.GetByIdAsync(expectedExercise.Id))
                .ReturnsAsync(expectedExercise);
            mockRepo.Setup(r => r.UpdateAsync(expectedExercise, expectedExercise.RowVersion))
                .ReturnsAsync(expectedExercise);

            var service = new ExerciseQuestionService(mockRepo.Object);

            //Act
            var result = await service.AddQuestionsAsync(expectedExercise.Id, addQuestionRequests);

            // Assert
            mockRepo.Verify(r => r.GetByIdAsync(It.IsAny<Guid>()), Times.Once);
        }

        [Fact]
        public async Task AddQuestionAsync_Should_Call_ExerciseRepository_UpdateAsync()
        {
            // Arrange
            var mockRepo = new Mock<IExerciseRepository>();
            var expectedExercise = new Exercise("title", "content", Guid.NewGuid());
            var expectedQuestion = new Question("QuestionTitle", "QuestionContent");
            var addQuestionRequests = new List<CreateQuestionRequest>
            {
                new CreateQuestionRequest {Title = expectedQuestion.Title, Content = expectedQuestion.Content}
            };

            mockRepo.Setup(r => r.GetByIdAsync(expectedExercise.Id))
                .ReturnsAsync(expectedExercise);
            mockRepo.Setup(r => r.UpdateAsync(expectedExercise, expectedExercise.RowVersion))
                .ReturnsAsync(expectedExercise);

            var service = new ExerciseQuestionService(mockRepo.Object);

            //Act
            var result = await service.AddQuestionsAsync(expectedExercise.Id, addQuestionRequests);

            // Assert
            mockRepo.Verify(r => r.UpdateAsync(It.IsAny<Exercise>(), expectedExercise.RowVersion), Times.Once);
        }

        [Fact]
        public async Task AddQuestionAsync_Should_Add_Question_To_Exercise()
        {
            // Arrange
            var mockRepo = new Mock<IExerciseRepository>();
            var expectedExercise = new Exercise("title", "content", Guid.NewGuid());
            var expectedQuestion = new Question("QuestionTitle", "QuestionContent");
            var addQuestionRequests = new List<CreateQuestionRequest>
            {
                new CreateQuestionRequest {Title = expectedQuestion.Title, Content = expectedQuestion.Content}
            };

            mockRepo.Setup(r => r.GetByIdAsync(expectedExercise.Id))
                .ReturnsAsync(expectedExercise);
            mockRepo.Setup(r => r.UpdateAsync(expectedExercise, expectedExercise.RowVersion))
                .ReturnsAsync(expectedExercise);

            var service = new ExerciseQuestionService(mockRepo.Object);

            //Act
            var result = await service.AddQuestionsAsync(expectedExercise.Id, addQuestionRequests);

            // Assert
            Assert.NotEmpty(result.Questions);
            Assert.True(result.Questions.Any());
            Assert.Single(result.Questions);
            Assert.Equal(result.Questions.First().Title, expectedQuestion.Title);
            Assert.Equal(result.Questions.First().Content, expectedQuestion.Content);
        }
        [Fact]
        public async Task AddQuestionAsync_Should_Add_Multiple_Questions_To_Exercise()
        {
            // Arrange
            var mockRepo = new Mock<IExerciseRepository>();
            var expectedExercise = new Exercise("title", "content", Guid.NewGuid());
            var expectedQuestion1 = new Question("Question1Title", "Question1Content");
            var expectedQuestion2 = new Question("Question2Title", "Question2Content");
            var addQuestionRequests = new List<CreateQuestionRequest>
            {
                new CreateQuestionRequest {Title = expectedQuestion1.Title, Content = expectedQuestion1.Content},
                new CreateQuestionRequest {Title = expectedQuestion2.Title, Content = expectedQuestion2.Content}
            };

            mockRepo.Setup(r => r.GetByIdAsync(expectedExercise.Id))
                .ReturnsAsync(expectedExercise);

            mockRepo.Setup(r => r.UpdateAsync(expectedExercise, expectedExercise.RowVersion))
                .ReturnsAsync(expectedExercise);

            var service = new ExerciseQuestionService(mockRepo.Object);

            // Act
            var result = await service.AddQuestionsAsync(expectedExercise.Id, addQuestionRequests);

            // Assert
            Assert.NotEmpty(result.Questions);
            Assert.True(result.Questions.Any());
            Assert.True(result.Questions.Count() > 0);
        }
        [Fact]
        public async Task AddQuestionAsync_Should_Not_Add_Question_With_Empty_Title()
        {
            // Arrange
            var mockRepo = new Mock<IExerciseRepository>();
            var expectedExercise = new Exercise("title", "content", Guid.NewGuid());
            var expectedQuestion = new Question(null, "QuestionContent");
            var addQuestionRequests = new List<CreateQuestionRequest>
            {
                new CreateQuestionRequest {Title = expectedQuestion.Title, Content = expectedQuestion.Content}
            };

            mockRepo.Setup(r => r.GetByIdAsync(expectedExercise.Id))
                .ReturnsAsync(expectedExercise);
            mockRepo.Setup(r => r.UpdateAsync(expectedExercise, expectedExercise.RowVersion))
                .ReturnsAsync(expectedExercise);

            var service = new ExerciseQuestionService(mockRepo.Object);

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() =>
                service.AddQuestionsAsync(expectedExercise.Id, addQuestionRequests));

            mockRepo.Verify(r => r.UpdateAsync(It.IsAny<Exercise>(), It.IsAny<byte[]>()), Times.Never);
        }
        [Fact]
        public async Task AddQuestionAsync_Should_Allow_Add_Question_With_Empty_Content()
        {
            // Arrange
            var mockRepo = new Mock<IExerciseRepository>();
            var expectedExercise = new Exercise("title", "content", Guid.NewGuid());
            var expectedQuestion = new Question("QuestionTitle", null);
            var addQuestionRequests = new List<CreateQuestionRequest>
            {
                new CreateQuestionRequest {Title = expectedQuestion.Title, Content = expectedQuestion.Content}
            };

            mockRepo.Setup(r => r.GetByIdAsync(expectedExercise.Id))
                .ReturnsAsync(expectedExercise);
            mockRepo.Setup(r => r.UpdateAsync(expectedExercise, expectedExercise.RowVersion))
                .ReturnsAsync(expectedExercise);

            var service = new ExerciseQuestionService(mockRepo.Object);

            //Act
            var result = await service.AddQuestionsAsync(expectedExercise.Id, addQuestionRequests);

            // Assert
            Assert.NotEmpty(result.Questions);
            Assert.True(result.Questions.Any());
            Assert.Single(result.Questions);
            Assert.Null(result.Questions.First().Content);
        }

        [Fact]
        public async Task AddQuestionAsync_Should_Add_Question_with_QuestionSolution()
        {
            // Arrange
            var mockRepo = new Mock<IExerciseRepository>();
            var expectedExercise = new Exercise("title", "content", Guid.NewGuid());
            var expectedQuestion = new Question("QuestionTitle", null);
            var expectedQuestionSolution = new QuestionSolution(expectedQuestion.Id, "QuestionSolution");
            var addQuestionRequests = new List<CreateQuestionRequest>
            {
                new CreateQuestionRequest {Title = expectedQuestion.Title, Content = expectedQuestion.Content,
                    Solution = new CreateQuestionSolutionRequest(){QuestionId = expectedQuestion.Id, Content = expectedQuestionSolution.Content} }
            };

            mockRepo.Setup(r => r.GetByIdAsync(expectedExercise.Id))
                .ReturnsAsync(expectedExercise);

            mockRepo.Setup(r => r.UpdateAsync(expectedExercise, expectedExercise.RowVersion))
                .ReturnsAsync(expectedExercise);

            var service = new ExerciseQuestionService(mockRepo.Object);

            //Act
            var result = await service.AddQuestionsAsync(expectedExercise.Id, addQuestionRequests);

            // Assert
            Assert.NotEmpty(result.Questions);
            Assert.True(result.Questions.Any());
            Assert.Single(result.Questions);
            Assert.NotNull(result.Questions.First().Solution);
            Assert.Equal(result.Questions.First().Solution.Content, expectedQuestionSolution.Content);
            Assert.Equal(result.Questions.First().Solution.QuestionId, result.Questions.First().Id);
        }
    }

    public class UpdateQuestionTests
    {
        [Fact]
        public async Task UpdateQuestionAsync_Should_Call_ExerciseRepository_GetByIdAsync()
        {
            // Arrange
            var mockRepo = new Mock<IExerciseRepository>();
            var expectedExercise = new Exercise("title", "content", Guid.NewGuid());
            expectedExercise.AddQuestion("title", "content");
            var updateRequest = new UpdateQuestionRequest
            {
                Id = expectedExercise.Questions.First().Id,
                Title = "new title",
                Content = "new content",
                RowVersion = expectedExercise.Questions.First().RowVersion,
            };

            mockRepo.Setup(r => r.GetByIdAsync(expectedExercise.Id))
                .ReturnsAsync(expectedExercise);

            mockRepo.Setup(r => r.UpdateAsync(expectedExercise, expectedExercise.RowVersion))
                .ReturnsAsync(expectedExercise);

            var service = new ExerciseQuestionService(mockRepo.Object);
            // Act

            var result = await service.UpdateQuestionAsync(expectedExercise.Id, updateRequest);

            // Assert
            mockRepo.Verify(r => r.GetByIdAsync(It.IsAny<Guid>()), Times.Once);
        }
        [Fact]
        public async Task UpdateQuestionAsync_Should_Call_ExerciseRepository_UpdateAsync()
        {
            // Arrange
            var mockRepo = new Mock<IExerciseRepository>();
            var expectedExercise = new Exercise("title", "content", Guid.NewGuid());
            expectedExercise.AddQuestion("title", "content");
            var updateRequest = new UpdateQuestionRequest
            {
                Id = expectedExercise.Questions.First().Id,
                Title = "new title",
                Content = "new content",
                RowVersion = expectedExercise.Questions.First().RowVersion,
            };

            mockRepo.Setup(r => r.GetByIdAsync(expectedExercise.Id))
                .ReturnsAsync(expectedExercise);
            mockRepo.Setup(r => r.UpdateAsync(expectedExercise, expectedExercise.RowVersion))
                .ReturnsAsync(expectedExercise);
            var service = new ExerciseQuestionService(mockRepo.Object);

            // Act
            var result = await service.UpdateQuestionAsync(expectedExercise.Id, updateRequest);

            // Assert
            mockRepo.Verify(r => r.UpdateAsync(It.IsAny<Exercise>(), expectedExercise.RowVersion), Times.Once);
        }

        [Fact]
        public async Task UpdateQuestionAsync_Should_Update_Question_Properties()
        {
            // Arrange
            var mockRepo = new Mock<IExerciseRepository>();
            var expectedExercise = new Exercise("title", "content", Guid.NewGuid());
            expectedExercise.AddQuestion("title", "content");
            var updateRequest = new UpdateQuestionRequest
            {
                Id = expectedExercise.Questions.First().Id,
                Title = "new title",
                Content = "new content",
                RowVersion = expectedExercise.Questions.First().RowVersion,
            };

            mockRepo.Setup(r => r.GetByIdAsync(expectedExercise.Id))
                .ReturnsAsync(expectedExercise);
            mockRepo.Setup(r => r.UpdateAsync(expectedExercise, expectedExercise.RowVersion))
                .ReturnsAsync(expectedExercise);
            var service = new ExerciseQuestionService(mockRepo.Object);

            // Act
            var result = await service.UpdateQuestionAsync(expectedExercise.Id, updateRequest);

            // Assert
            Assert.Equal(expectedExercise.Questions.First().Id, result.Questions.First().Id);
            Assert.Equal(updateRequest.Title, result.Questions.First().Title);
            Assert.Equal(updateRequest.Content, result.Questions.First().Content);
        }
        [Fact]
        public async Task UpdateQuestionAsync_Should_Throw_When_Updating_Nonexistent_Question()
        {
            // Arrange
            var mockRepo = new Mock<IExerciseRepository>();
            var expectedExercise = new Exercise("title", "content", Guid.NewGuid());
            expectedExercise.AddQuestion("title", "content");
            var updateRequest = new UpdateQuestionRequest
            {
                Id = expectedExercise.Questions.First().Id,
                Title = "new title",
                Content = "new content",
                RowVersion = expectedExercise.Questions.First().RowVersion,
            };

            mockRepo.Setup(r => r.UpdateAsync(expectedExercise, expectedExercise.RowVersion))
                .ReturnsAsync(expectedExercise);
            var service = new ExerciseQuestionService(mockRepo.Object);

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() =>
                service.UpdateQuestionAsync(expectedExercise.Id, updateRequest));

            mockRepo.Verify(r => r.UpdateAsync(It.IsAny<Exercise>(), It.IsAny<byte[]>()), Times.Never);
        }
    }

    public class RemoveQuestionsTests
    {
        [Fact]
        public async Task RemoveQuestionAsync_Should_Call_ExerciseRepository_GetByIdAsync()
        {
            // Arrange
            var mockRepo = new Mock<IExerciseRepository>();
            var expectedExercise = new Exercise("title", "content", Guid.NewGuid());
            expectedExercise.AddQuestion("title", "content");
            var removeRequest = new RemoveQuestionRequest
            {
                Id = expectedExercise.Questions.First().Id,
                RowVersion = expectedExercise.Questions.First().RowVersion,
            };

            mockRepo.Setup(r => r.GetByIdAsync(expectedExercise.Id))
                .ReturnsAsync(expectedExercise);

            mockRepo.Setup(r => r.UpdateAsync(expectedExercise, expectedExercise.RowVersion))
                .ReturnsAsync(expectedExercise);

            var service = new ExerciseQuestionService(mockRepo.Object);
            // Act

            var result = await service.RemoveQuestionAsync(expectedExercise.Id, removeRequest);

            // Assert
            mockRepo.Verify(r => r.GetByIdAsync(It.IsAny<Guid>()), Times.Once);
        }
        
        [Fact]
        public async Task RemoveQuestionAsync_Should_Call_ExerciseRepository_UpdateAsync()
        {
            // Arrange
            var mockRepo = new Mock<IExerciseRepository>();
            var expectedExercise = new Exercise("title", "content", Guid.NewGuid());
            expectedExercise.AddQuestion("title", "content");
            var removeRequest = new RemoveQuestionRequest
            {
                Id = expectedExercise.Questions.First().Id,
                RowVersion = expectedExercise.Questions.First().RowVersion,
            };

            mockRepo.Setup(r => r.GetByIdAsync(expectedExercise.Id))
                .ReturnsAsync(expectedExercise);

            mockRepo.Setup(r => r.UpdateAsync(expectedExercise, expectedExercise.RowVersion))
                .ReturnsAsync(expectedExercise);

            var service = new ExerciseQuestionService(mockRepo.Object);
            // Act

            var result = await service.RemoveQuestionAsync(expectedExercise.Id, removeRequest);

            // Assert
            mockRepo.Verify(r => r.UpdateAsync(It.IsAny<Exercise>(), expectedExercise.RowVersion), Times.Once);
        }

        [Fact]
        public async Task RemoveQuestionAsync_Should_Remove_Question_From_Exercise()
        {
            // Arrange
            var mockRepo = new Mock<IExerciseRepository>();
            var expectedExercise = new Exercise("title", "content", Guid.NewGuid());
            expectedExercise.AddQuestion("title", "content");
            var removeRequest = new RemoveQuestionRequest
            {
                Id = expectedExercise.Questions.First().Id,
                RowVersion = expectedExercise.Questions.First().RowVersion,
            };

            mockRepo.Setup(r => r.GetByIdAsync(expectedExercise.Id))
                .ReturnsAsync(expectedExercise);

            mockRepo.Setup(r => r.UpdateAsync(expectedExercise, expectedExercise.RowVersion))
                .ReturnsAsync(expectedExercise);

            var service = new ExerciseQuestionService(mockRepo.Object);
            // Act

            var result = await service.RemoveQuestionAsync(expectedExercise.Id, removeRequest);

            // Assert
            Assert.DoesNotContain(result.Questions, q => q.Id == removeRequest.Id);
        }
        [Fact]
        public async Task RemoveQuestionAsync_Should_Throw_When_Removing_Nonexistent_Question()
        {
            // Arrange
            var mockRepo = new Mock<IExerciseRepository>();
            var exercise = new Exercise("title", "content", Guid.NewGuid());
            exercise.AddQuestion("title", "content");

            var removeRequest = new RemoveQuestionRequest
            {
                Id = Guid.NewGuid(), // ID that does NOT exist
                RowVersion = exercise.Questions.First().RowVersion,
            };

            mockRepo.Setup(r => r.GetByIdAsync(exercise.Id))
                .ReturnsAsync(exercise);

            var service = new ExerciseQuestionService(mockRepo.Object);

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() =>
                service.RemoveQuestionAsync(exercise.Id, removeRequest));

            mockRepo.Verify(r => r.UpdateAsync(It.IsAny<Exercise>(), It.IsAny<byte[]>()), Times.Never);
        }
        [Fact]
        public async Task RemoveQuestionAsync_Should_Remove_Only_Targeted_Question()
        {
            // Arrange
            var mockRepo = new Mock<IExerciseRepository>();
            var exercise = new Exercise("title", "content", Guid.NewGuid());

            exercise.AddQuestion("q1", "c1");
            exercise.AddQuestion("q2", "c2");

            var questionToRemove = exercise.Questions.First();
            var remainingQuestion = exercise.Questions.Last();

            var removeRequest = new RemoveQuestionRequest
            {
                Id = questionToRemove.Id,
                RowVersion = questionToRemove.RowVersion,
            };

            mockRepo.Setup(r => r.GetByIdAsync(exercise.Id))
                .ReturnsAsync(exercise);

            mockRepo.Setup(r => r.UpdateAsync(exercise, exercise.RowVersion))
                .ReturnsAsync(exercise);

            var service = new ExerciseQuestionService(mockRepo.Object);

            // Act
            var result = await service.RemoveQuestionAsync(exercise.Id, removeRequest);

            // Assert
            Assert.DoesNotContain(result.Questions, q => q.Id == questionToRemove.Id);
            Assert.Contains(result.Questions, q => q.Id == remainingQuestion.Id);
            Assert.Single(result.Questions);

            mockRepo.Verify(r => r.UpdateAsync(exercise, exercise.RowVersion), Times.Once);
        }


    }
}

