using Moq;
using Exercise_Application.Implementations;
using Exercise_Application.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Exercise_Domain.Entities;

namespace Exercise_Tests.Application
{
    public class QuestionServiceTests
    {
    }

    public class CreateQuestionServiceTests
    {
        [Fact]
        public void CreateQuestion_ShouldCallRepositorySave()
        {
            // Arrange
            var mockRepo = new Mock<IQuestionRepository>();
            var service = new QuestionService(mockRepo.Object);
            // Act
            service.CreateQuestion("Question Content", Guid.NewGuid());
            // Assert
            mockRepo.Verify(
                r => r.AddQuestion(It.IsAny<Question>()),
                Times.Once
            );
        }
        [Fact]
        public void CreateQuestion_ShouldThrowException_WhenRepositoryFails()
        {
            // Arrange
            var mockRepo = new Mock<IQuestionRepository>();
            mockRepo.Setup(r => r.AddQuestion(It.IsAny<Question>()))
                    .Throws(new Exception("Database error"));
            var service = new QuestionService(mockRepo.Object);
            // Act & Assert
            Assert.Throws<Exception>(() => service.CreateQuestion("Question Content", Guid.NewGuid()));
        }
    }

    public class ReadQuestionServiceTests
    {
        [Fact]
        public void GetQuestion_ShouldCallRepositoryToGet()
        {
            // Arrange
            var mockRepo = new Mock<IQuestionRepository>();
            var service = new QuestionService(mockRepo.Object);
            var questionId = Guid.NewGuid();
            // Act
            service.GetQuestion(questionId);
            // Assert
            mockRepo.Verify(
                r => r.GetQuestionById(questionId),
                Times.Once
            );
        }

        [Fact]
        public void GetQuestionsByExerciseId_ShouldCallRepositoryToGet()
        {
            // Arrange
            var mockRepo = new Mock<IQuestionRepository>();
            var service = new QuestionService(mockRepo.Object);
            var exerciseId = Guid.NewGuid();
            // Act
            service.GetQuestionsByExerciseId(exerciseId);
            // Assert
            mockRepo.Verify(
                r => r.GetQuestionsByExerciseId(exerciseId),
                Times.Once
            );
        }

        [Fact]
        public void GetQuestionsByExerciseId_ShouldThrowException_WhenRepositoryFails()
        {
            // Arrange
            var mockRepo = new Mock<IQuestionRepository>();
            var exerciseId = Guid.NewGuid();
            mockRepo.Setup(r => r.GetQuestionsByExerciseId(exerciseId))
                    .Throws(new Exception("Database error"));
            var service = new QuestionService(mockRepo.Object);
            // Act & Assert
            Assert.Throws<Exception>(() => service.GetQuestionsByExerciseId(exerciseId));
        }

        [Fact]
        public void GetQuestion_ShouldThrowException_WhenRepositoryFails()
        {
            // Arrange
            var mockRepo = new Mock<IQuestionRepository>();
            var questionId = Guid.NewGuid();
            mockRepo.Setup(r => r.GetQuestionById(questionId))
                    .Throws(new Exception("Database error"));
            var service = new QuestionService(mockRepo.Object);
            // Act & Assert
            Assert.Throws<Exception>(() => service.GetQuestion(questionId));
        }
    }

    public class UpdateQuestionServiceTests
    {
        [Fact]
        public void UpdateQuestion_ShouldCallRepositoryToUpdate()
        {
            // Arrange
            var mockRepo = new Mock<IQuestionRepository>();
            var service = new QuestionService(mockRepo.Object);
            var question = new Question("Question Content", Guid.NewGuid());
            var rowVersion = new byte[] { 1, 2, 3 };
            // Act
            service.UpdateQuestion(question, rowVersion);
            // Assert
            mockRepo.Verify(
                r => r.UpdateQuestion(question, rowVersion),
                Times.Once
            );
        }

        [Fact]
        public void UpdateQuestion_ShouldThrowException_WhenRepositoryFails()
        {
            // Arrange
            var mockRepo = new Mock<IQuestionRepository>();
            var question = new Question("Question Content", Guid.NewGuid());
            var rowVersion = new byte[] { 1, 2, 3 };
            mockRepo.Setup(r => r.UpdateQuestion(question, rowVersion))
                    .Throws(new Exception("Database error"));
            var service = new QuestionService(mockRepo.Object);
            // Act & Assert
            Assert.Throws<Exception>(() => service.UpdateQuestion(question, rowVersion));
        }
    }

    public class DeleteQuestionServiceTests
    {
        [Fact]
        public void DeleteQuestion_ShouldCallRepositoryToDelete()
        {
            // Arrange
            var mockRepo = new Mock<IQuestionRepository>();
            var service = new QuestionService(mockRepo.Object);
            var questionId = Guid.NewGuid();
            // Act
            service.DeleteQuestion(questionId);
            // Assert
            mockRepo.Verify(
                r => r.DeleteQuestion(questionId),
                Times.Once
            );
        }
        [Fact]
        public void DeleteQuestion_ShouldThrowException_WhenRepositoryFails()
        {
            // Arrange
            var mockRepo = new Mock<IQuestionRepository>();
            var questionId = Guid.NewGuid();
            mockRepo.Setup(r => r.DeleteQuestion(questionId))
                    .Throws(new Exception("Database error"));
            var service = new QuestionService(mockRepo.Object);
            // Act & Assert
            Assert.Throws<Exception>(() => service.DeleteQuestion(questionId));
        }
    }
}

