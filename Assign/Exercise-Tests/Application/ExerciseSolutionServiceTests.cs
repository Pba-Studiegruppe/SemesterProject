using Moq;
using Exercise_Application.Implementations;
using Exercise_Application.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Exercise_Application.Interfaces.Services;

namespace Exercise_Tests.Application
{
    public class ExerciseSolutionServiceTests
    {


    }

    public class CreateExerciseSolutionServiceTests
    {
        [Fact]
        public void CreateExerciseSolution_ShouldCallRepositorySave()
        {
            // Arrange
            var mockRepo = new Mock<IExerciseSolutionRepository>();
            var service = new ExerciseSolutionService(mockRepo.Object);
            // Act
            service.CreateExerciseSolution(Guid.NewGuid(), "Solution Content", Guid.NewGuid());
            // Assert
            mockRepo.Verify(
                r => r.AddExerciseSolution(It.IsAny<ExerciseSolution>()),
                Times.Once
            );
        }

        [Fact]
        public void CreateExerciseSolution_ShouldThrowException_WhenRepositoryFails()
        {
            // Arrange
            var mockRepo = new Mock<IExerciseSolutionRepository>();
            mockRepo.Setup(r => r.AddExerciseSolution(It.IsAny<ExerciseSolution>()))
                    .Throws(new Exception("Database error"));
            var service = new ExerciseSolutionService(mockRepo.Object);
            // Act & Assert
            Assert.Throws<Exception>(() => service.CreateExerciseSolution(Guid.NewGuid(), "Solution Content", Guid.NewGuid()));

        }
    }
    public class ReadExerciseSolutionServiceTests
    {
        [Fact]
        public void GetExerciseSolution_ShouldCallRepositoryToGet()
        {
            // Arrange
            var mockRepo = new Mock<IExerciseSolutionRepository>();
            var service = new ExerciseSolutionService(mockRepo.Object);
            var solutionId = Guid.NewGuid();
            // Act
            service.GetExerciseSolution(solutionId);
            // Assert
            mockRepo.Verify(
                r => r.GetExerciseSolution(solutionId),
                Times.Once
            );
        }
        [Fact]
        public void GetExerciseSolution_ShouldThrowException_WhenRepositoryFails()
        {
            // Arrange
            var mockRepo = new Mock<IExerciseSolutionRepository>();
            var solutionId = Guid.NewGuid();
            mockRepo.Setup(r => r.GetExerciseSolution(solutionId))
                    .Throws(new Exception("Database error"));
            var service = new ExerciseSolutionService(mockRepo.Object);
            // Act & Assert
            Assert.Throws<Exception>(() => service.GetExerciseSolution(solutionId));
        }
    }
    public class UpdateExerciseSolutionServiceTests
    {
        [Fact]
        public void UpdateExerciseSolution_ShouldCallRepositoryToUpdate()
        {
            // Arrange
            var mockRepo = new Mock<IExerciseSolutionRepository>();
            var service = new ExerciseSolutionService(mockRepo.Object);
            var solution = new ExerciseSolution(Guid.NewGuid(), "Updated Content", Guid.NewGuid());
            var rowVersion = new byte[] { 1, 2, 3 };
            // Act
            service.UpdateExerciseSolution(solution, rowVersion);
            // Assert
            mockRepo.Verify(
                r => r.UpdateExerciseSolution(solution, rowVersion),
                Times.Once
            );
        }

        [Fact]
        public void UpdateExerciseSolution_ShouldThrowException_WhenRepositoryFails()
        {
            // Arrange
            var mockRepo = new Mock<IExerciseSolutionRepository>();
            var solution = new ExerciseSolution(Guid.NewGuid(), "Updated Content", Guid.NewGuid());
            var rowVersion = new byte[] { 1, 2, 3 };
            mockRepo.Setup(r => r.UpdateExerciseSolution(solution, rowVersion))
                    .Throws(new Exception("Database error"));
            var service = new ExerciseSolutionService(mockRepo.Object);
            // Act & Assert
            Assert.Throws<Exception>(() => service.UpdateExerciseSolution(solution, rowVersion));
        }
    }
    public class DeleteExerciseSolutionServiceTests
    {
        [Fact]
        public void DeleteExerciseSolution_ShouldCallRepositoryToDelete()
        {
            // Arrange
            var mockRepo = new Mock<IExerciseSolutionRepository>();
            var service = new ExerciseSolutionService(mockRepo.Object);
            var solutionId = Guid.NewGuid();
            var rowVersion = new byte[] { 1, 2, 3 };
            // Act
            service.DeleteExerciseSolution(solutionId, rowVersion);
            // Assert
            mockRepo.Verify(
                r => r.DeleteExerciseSolution(solutionId, rowVersion),
                Times.Once
            );
        }
        [Fact]
        public void DeleteExerciseSolution_ShouldThrowException_WhenRepositoryFails()
        {
            // Arrange
            var mockRepo = new Mock<IExerciseSolutionRepository>();
            var solutionId = Guid.NewGuid();
            var rowVersion = new byte[] { 1, 2, 3 };
            mockRepo.Setup(r => r.DeleteExerciseSolution(solutionId, rowVersion))
                    .Throws(new Exception("Database error"));
            var service = new ExerciseSolutionService(mockRepo.Object);
            // Act & Assert
            Assert.Throws<Exception>(() => service.DeleteExerciseSolution(solutionId, rowVersion));
        }
    }

}
