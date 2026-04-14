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
    public class CreateQuestionSolutionServiceTests
    {
        [Fact]
        public async Task CreateQuestionSolution_Should_Create_Solution_For_Existing_Question()
        {
            // Arrange
            var question = new Question(Guid.NewGuid(), "Title", "Content");
            var createdQuestionSolution = new QuestionSolution(question.Id, "Content");
            var solutionContent = "Content";
            var questionRepositoryMock = new Mock<IQuestionRepository>();
            var questionSolutionRepositoryMock = new Mock<IQuestionSolutionRepository>();

            questionRepositoryMock.Setup(repo => repo.GetQuestionByIdAsync(question.Id)).ReturnsAsync(question);
            questionSolutionRepositoryMock.Setup(repo => repo.AddQuestionSolutionAsync(It.IsAny<QuestionSolution>())).ReturnsAsync(createdQuestionSolution);

            var service = new QuestionSolutionService(questionSolutionRepositoryMock.Object);

            // Act
            var result = await service.CreateQuestionSolutionAsync(new CreateQuestionSolutionRequest
            {
                QuestionId = question.Id,
                Content = solutionContent
            });
            // Assert
            Assert.NotNull(result);
            Assert.Equal(createdQuestionSolution.Id, result.Id);
            Assert.Equal(createdQuestionSolution.QuestionId, result.QuestionId);
            Assert.Equal(createdQuestionSolution.Content, result.Content);
        }

        [Fact]
        public async Task CreateQuestionSolution_Should_Throw_Exception_For_NonExisting_Question()
        {
            // Arrange
            var questionId = Guid.NewGuid();
            var solutionContent = "Content";
            var questionRepositoryMock = new Mock<IQuestionRepository>();
            var questionSolutionRepositoryMock = new Mock<IQuestionSolutionRepository>();
            questionRepositoryMock.Setup(repo => repo.GetQuestionByIdAsync(questionId)).ReturnsAsync((Question?)null);
            var service = new QuestionSolutionService(questionSolutionRepositoryMock.Object);

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() =>
                service.CreateQuestionSolutionAsync(new CreateQuestionSolutionRequest
                {
                    QuestionId = questionId,
                    Content = solutionContent
                })
            );
        }
    }

    public class ReadQuestionSolutionServiceTests
    {
        [Fact]
        public async Task GetQuestionSolutionById_Should_Return_Solution_For_Existing_Id()
        {
            // Arrange
            var questionSolutionId = Guid.NewGuid();
            var questionSolution = new QuestionSolution(Guid.NewGuid(), "Content");
            var questionSolutionRepositoryMock = new Mock<IQuestionSolutionRepository>();

            questionSolutionRepositoryMock.Setup(repo => repo.GetQuestionSolutionByIdAsync(questionSolutionId)).ReturnsAsync(questionSolution);
            var service = new QuestionSolutionService(questionSolutionRepositoryMock.Object);

            // Act
            var result = await service.GetQuestionSolutionByIdAsync(questionSolutionId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(questionSolution.Id, result.Id);
            Assert.Equal(questionSolution.QuestionId, result.QuestionId);
            Assert.Equal(questionSolution.Content, result.Content);
        }

        [Fact]
        public async Task GetQuestionSolutionByQuestionId_Should_Return_Solution_For_Existing_QuestionId()
        {
            // Arrange
            var questionId = Guid.NewGuid();
            var questionSolution = new QuestionSolution(questionId, "Content");
            var questionSolutionRepositoryMock = new Mock<IQuestionSolutionRepository>();
            questionSolutionRepositoryMock.Setup(repo => repo.GetQuestionSolutionByQuestionIdAsync(questionId)).ReturnsAsync(questionSolution);
            var service = new QuestionSolutionService(questionSolutionRepositoryMock.Object);

            // Act
            var result = await service.GetQuestionSolutionByQuestionIdAsync(questionId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(questionSolution.Id, result.Id);
            Assert.Equal(questionSolution.QuestionId, result.QuestionId);
            Assert.Equal(questionSolution.Content, result.Content);
        }

        public class UpdateQuestionSolutionServiceTests
        {
            [Fact]
            public async Task UpdateQuestionSolution_Should_Update_Content_For_Existing_Solution()
            {
                // Arrange
                var questionId = Guid.NewGuid();
                var existingSolution = new QuestionSolution(questionId, "Old Content");
                var updatedContent = "Updated Content";
                var questionSolutionUpdateRequest = new UpdateQuestionSolutionRequest
                {
                    Id = questionId,
                    QuestionId = questionId,
                    Content = updatedContent
                };
                var questionSolutionRepositoryMock = new Mock<IQuestionSolutionRepository>();

                questionSolutionRepositoryMock.Setup(repo => repo.GetQuestionSolutionByIdAsync(existingSolution.Id)).ReturnsAsync(existingSolution);
                questionSolutionRepositoryMock.Setup(repo => repo.UpdateQuestionSolutionAsync(It.IsAny<QuestionSolution>(),It.IsAny<byte[]>())).ReturnsAsync(existingSolution);

                var service = new QuestionSolutionService(questionSolutionRepositoryMock.Object);

                // Act
                var result = await service.UpdateQuestionSolutionAsync(questionSolutionUpdateRequest);

                // Assert
                Assert.NotNull(result);
                Assert.Equal(existingSolution.Id, result.Id);
                Assert.Equal(existingSolution.QuestionId, result.QuestionId);
                Assert.Equal(updatedContent, result.Content);

            }
        }
    }
}


