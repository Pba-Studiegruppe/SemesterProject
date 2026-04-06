using Moq;
using Exercise_Application.Implementations;
using Exercise_Application.Interfaces.Repositories;
using Exercise_Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Exercise_Tests.Application
{
    public class QuestionSolutionServiceTests
    {
    }

    public class CreateQuestionSolutionServiceTests
    {
        [Fact]
        public void CreateQuestionSolution_ShouldCallRepositorySave()
        {
            // Arrange
            var mockRepo = new Mock<IQuestionSolutionRepository>();
            var service = new QuestionSolutionService(mockRepo.Object);
            // Act
            service.CreateQuestionSolution("Solution Text", Guid.NewGuid(), Guid.NewGuid());
            // Assert
            mockRepo.Verify(
                r => r.AddQuestionSolution(It.IsAny<QuestionSolution>()),
                Times.Once
            );
        }

        [Fact]
        public void CreateQuestionSolution_ShouldThrowException_WhenRepositoryFails()
        {
            // Arrange
            var mockRepo = new Mock<IQuestionSolutionRepository>();
            mockRepo.Setup(r => r.AddQuestionSolution(It.IsAny<QuestionSolution>()))
                    .Throws(new Exception("Database error"));
            var service = new QuestionSolutionService(mockRepo.Object);
            // Act & Assert
            Assert.Throws<Exception>(() => service.CreateQuestionSolution("Solution Text", Guid.NewGuid(), Guid.NewGuid()));
        }
    }

    public class ReadQuestionSolutionServiceTests
    {
        [Fact]
        public void GetQuestionSolution_ShouldCallRepositoryToGet()
        {
            // Arrange
            var mockRepo = new Mock<IQuestionSolutionRepository>();
            var service = new QuestionSolutionService(mockRepo.Object);
            var solutionId = Guid.NewGuid();
            // Act
            service.GetQuestionSolution(solutionId);
            // Assert
            mockRepo.Verify(
                r => r.GetQuestionSolutionById(solutionId),
                Times.Once
            );
        }

        [Fact]
        public void GetQuestionSolutionsByQuestionId_ShouldCallRepositoryToGet()
        {
            // Arrange
            var mockRepo = new Mock<IQuestionSolutionRepository>();
            var service = new QuestionSolutionService(mockRepo.Object);
            var questionId = Guid.NewGuid();
            // Act
            service.GetQuestionSolutionsByQuestionId(questionId);
            // Assert
            mockRepo.Verify(
                r => r.GetQuestionSolutionsByQuestionId(questionId),
                Times.Once
            );
        }

        [Fact]
        public void GetQuestionSolutionsByQuestionId_ShouldThrowException_WhenRepositoryFails()
        {
            // Arrange
            var mockRepo = new Mock<IQuestionSolutionRepository>();
            var questionId = Guid.NewGuid();
            mockRepo.Setup(r => r.GetQuestionSolutionsByQuestionId(questionId))
                    .Throws(new Exception("Database error"));
            var service = new QuestionSolutionService(mockRepo.Object);
            // Act & Assert
            Assert.Throws<Exception>(() => service.GetQuestionSolutionsByQuestionId(questionId));
        }
    }

    public class UpdateQuestionSolutionServiceTests
    {
        [Fact]
        public void UpdateQuestionSolution_ShouldCallRepositoryToUpdate()
        {
            // Arrange
            var mockRepo = new Mock<IQuestionSolutionRepository>();
            var service = new QuestionSolutionService(mockRepo.Object);
            var solution = new QuestionSolution("Updated Solution Text", Guid.NewGuid(), Guid.NewGuid());
            var rowVersion = new byte[] { 1, 2, 3 };
            // Act
            service.UpdateQuestionSolution(solution, rowVersion);
            // Assert
            mockRepo.Verify(
                r => r.UpdateQuestionSolution(solution, rowVersion),
                Times.Once
            );
        }
    }


}

