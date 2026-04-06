using Exercise_Application.Implementations;
using Exercise_Application.Interfaces.Repositories;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exercise_Tests.Application
{
    public class ExerciseServiceTests
    {


        //[Fact]
        //public void UpdateExercise_ShouldCallRepositoryToUpdate()
        //{
        //    // Arrange
        //    var mockRepo = new Mock<IExerciseRepository>();
        //    var service = new ExerciseService(mockRepo.Object);
        //    var exercise = new Exercise("Title", "Content", Guid.NewGuid());
        //    var rowVersion = new byte[] { 1, 2, 3 };

        //    // Act
        //    service.UpdateExercise(exercise, rowVersion);

        //    // Assert
        //    mockRepo.Verify(
        //        r => r.UpdateExercise(exercise, rowVersion),
        //        Times.Once
        //    );
        //}
    }

    public class CreateExerciseServiceTests
    {
        [Fact]
        public void CreateExercise_ShouldCallRepositorySave()
        {
            // Arrange
            var mockRepo = new Mock<IExerciseRepository>();
            var service = new ExerciseService(mockRepo.Object);

            // Act
            service.CreateExercise("Title", "Content", Guid.NewGuid());

            // Assert
            mockRepo.Verify(
                r => r.AddExercise(It.IsAny<Exercise>()),
                Times.Once
            );
        }

        [Fact]
        public void CreateExercise_ShouldThrowException_WhenRepositoryFails()
        {
            // Arrange
            var mockRepo = new Mock<IExerciseRepository>();
            mockRepo.Setup(r => r.AddExercise(It.IsAny<Exercise>()))
                    .Throws(new Exception("Database error"));
            var service = new ExerciseService(mockRepo.Object);
            // Act & Assert
            Assert.Throws<Exception>(() => service.CreateExercise("Title", "Content", Guid.NewGuid()));
        }
    }

}

public class ReadExerciseServiceTests
{
    [Fact]
    public void GetExerciseById_ShouldReturnExercise_WhenFound()
    {
        // Arrange
        var mockRepo = new Mock<IExerciseRepository>();
        var exerciseId = Guid.NewGuid();
        var expectedExercise = new Exercise("Title", "Content", exerciseId);
        mockRepo.Setup(r => r.GetExerciseById(exerciseId)).Returns(expectedExercise);
        var service = new ExerciseService(mockRepo.Object);
        // Act
        var result = service.GetExerciseById(exerciseId);
        // Assert
        Assert.Equal(expectedExercise, result);
    }

    //Exercise has a Guid CreatedByTeacherId
    //Exercise has a list of ExerciseKeywords

    [Fact]
    public void GetExerciseByExerciseKeywords_ShouldReturnExercises_WhenFound()
    {

    }

    [Fact]
    public void GetExerciseByCreatedByTeacherId_ShouldReturnExercises_WhenFound()
    {

    }

    [Fact]
    public void GetExerciseById_ShouldReturnNull_WhenNotFound()
    {
        // Arrange
        var mockRepo = new Mock<IExerciseRepository>();
        var exerciseId = Guid.NewGuid();
        mockRepo.Setup(r => r.GetExerciseById(exerciseId)).Returns((Exercise)null);
        var service = new ExerciseService(mockRepo.Object);
        // Act
        var result = service.GetExerciseById(exerciseId);
        // Assert
        Assert.Null(result);
    }
}
