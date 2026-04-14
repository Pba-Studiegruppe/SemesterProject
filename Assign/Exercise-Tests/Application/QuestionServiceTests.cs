using Moq;
using Exercise_Application.Implementations;
using Exercise_Application.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Exercise_Domain.Entities;
using Exercise_Application.DTO;

namespace Exercise_Tests.Application
{
    public class CreateQuestionServiceTests
    {
        public static IEnumerable<object[]> QuestionTestData()
        {
            yield return new object[] { new CreateQuestionRequest { ExerciseId = Guid.NewGuid(), Title = "Title1", Content = "Content1" } };
            yield return new object[] { new CreateQuestionRequest { ExerciseId = Guid.NewGuid(), Title = "Title2", Content = "Content2" } };
        }

        [Theory]
        [MemberData(nameof(QuestionTestData))]
        public async Task AddQuestionAsync_Should_Return_Created_Question(CreateQuestionRequest request)
        {
            // Arrange
            var mockRepository = new Mock<IQuestionRepository>();
            var createdQuestion = new Question(request.ExerciseId, request.Title, request.Content);

            mockRepository.Setup(repo => repo.AddQuestionAsync(It.IsAny<Question>())).ReturnsAsync(createdQuestion);
            var service = new QuestionService(mockRepository.Object);

            // Act
            var result = await service.CreateQuestionAsync(request);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(createdQuestion.Id, result.Id);
            Assert.Equal(request.ExerciseId, result.ExerciseId);
            Assert.Equal(request.Content, result.Content);
        }
    }

    public class ReadQuestionServiceTests
    {

        [Fact]
        public async Task GetQuestionByIdAsync_Should_Return_Question()
        {
            // Arrange
            var mockRepository = new Mock<IQuestionRepository>();
            var questionId = Guid.NewGuid();
            var expectedQuestion = new Question(Guid.NewGuid(), "Title", "Content");

            mockRepository.Setup(repo => repo.GetQuestionByIdAsync(questionId)).ReturnsAsync(expectedQuestion);

            var service = new QuestionService(mockRepository.Object);

            // Act
            var result = await service.GetQuestionByIdAsync(questionId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedQuestion.Id, result.Id);
            Assert.Equal(expectedQuestion.ExerciseId, result.ExerciseId);
            Assert.Equal(expectedQuestion.Content, result.Content);

        }

        [Fact]
        public async Task GetQuestionsByExerciseIdAsync_Should_Return_List_Of_Questions()
        {
            // Arrange
            var mockRepository = new Mock<IQuestionRepository>();
            var exerciseId = Guid.NewGuid();
            var expectedQuestions = new List<Question>
            {
                new Question(exerciseId, "Title1", "Content1"),
                new Question(exerciseId, "Title2", "Content2")
            };

            mockRepository.Setup(repo => repo.GetQuestionsByExerciseIdAsync(exerciseId)).ReturnsAsync(expectedQuestions);

            var service = new QuestionService(mockRepository.Object);

            // Act
            var result = await service.GetQuestionsByExerciseIdAsync(exerciseId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedQuestions.Count, result.Count());
            Assert.All(result, q => Assert.Equal(exerciseId, q.ExerciseId));
        }
    }

        public class UpdateQuestionServiceTests
        {
            [Fact]
            public async Task UpdateQuestionAsync_Should_Return_Updated_Question()
            {
                // Arrange
                var mockRepository = new Mock<IQuestionRepository>();

                var existingQuestion = new Question(Guid.NewGuid(), "Old Title", "Old Content");
                var updateRequest = new UpdateQuestionRequest {Id = existingQuestion.Id, Title = "New Title", Content = "New Content", RowVersion = existingQuestion.RowVersion };

                var updatedQuestion = new Question(existingQuestion.ExerciseId, updateRequest.Title, updateRequest.Content);

                mockRepository.Setup(repo => repo.GetQuestionByIdAsync(existingQuestion.Id)).ReturnsAsync(existingQuestion);
                mockRepository.Setup(repo => repo.UpdateQuestionAsync(existingQuestion, existingQuestion.RowVersion)).ReturnsAsync(updatedQuestion);

                var service = new QuestionService(mockRepository.Object);

                // Act
                var result = await service.UpdateQuestionAsync(updateRequest);

                // Assert
                Assert.NotNull(result);
                Assert.Equal(updatedQuestion.Id, result.Id);
                Assert.Equal(existingQuestion.ExerciseId, result.ExerciseId);
                Assert.Equal(updateRequest.Content, result.Content);
            }
        }
    }



