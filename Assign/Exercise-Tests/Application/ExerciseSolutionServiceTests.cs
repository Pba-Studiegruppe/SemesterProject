using Moq;
using Exercise_Application.Implementations;
using Exercise_Application.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Exercise_Application.Interfaces.Services;
using Exercise_Application.DTO;

namespace Exercise_Tests.Application
{
    public class ReadExerciseSolutionServiceTests
    {
        [Fact]
        public async Task GetExerciseSolutionById_ReturnsExerciseSolution_WhenExerciseSolutionExists()
        {
            // Arrange
            var exerciseId = Guid.NewGuid();
            var expectedSolution = new ExerciseSolution(exerciseId, "Solution Content", "https://video.com");
            var mockRepository = new Mock<IExerciseSolutionRepository>();
            mockRepository.Setup(repo => repo.GetExerciseSolutionByIdAsync(exerciseId))
                          .ReturnsAsync(expectedSolution);
            var service = new ExerciseSolutionService(mockRepository.Object);
            // Act
            var result = await service.GetExerciseSolutionByIdAsync(exerciseId);
            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedSolution.Id, result.Id);
            Assert.Equal(expectedSolution.ExerciseId, result.ExerciseId);
            Assert.Equal(expectedSolution.Content, result.Content);
            Assert.Equal(expectedSolution.VideoUrl, result.VideoUrl);
        }

        [Fact]
        public async Task GetExerciseSolutionsByExerciseIdAsync_ReturnsExerciseSolution_WhenExerciseSolutionExists()
        {
            // Arrange
            var exerciseId = Guid.NewGuid();
            var expectedSolutions = new List<ExerciseSolution>
            {
                new ExerciseSolution(exerciseId, "Solution Content 1", "https://video1.com"),
                new ExerciseSolution(exerciseId, "Solution Content 2", "https://video2.com")
            };
            var mockRepository = new Mock<IExerciseSolutionRepository>();
            mockRepository.Setup(repo => repo.GetExerciseSolutionsByExerciseIdAsync(exerciseId))
                          .ReturnsAsync(expectedSolutions);
            var service = new ExerciseSolutionService(mockRepository.Object);
            // Act
            var result = await service.GetExerciseSolutionsByExerciseIdAsync(exerciseId);
            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedSolutions.Count, result.Count());
        }
    }

    public class CreateExerciseSolutionServiceTests
    {
        [Fact]
        public async Task CreateExerciseSolutionAsync_Should_Return_Created_ExerciseSolution()
        {
            // Arrange
            var exerciseId = Guid.NewGuid();
            var createRequest = new CreateExerciseSolutionRequest
            {
                ExerciseId = exerciseId,
                Content = "New Solution Content",
                VideoUrl = "https://newvideo.com"
            };

            var expectedSolution = new ExerciseSolution(exerciseId, createRequest.Content, createRequest.VideoUrl);
            var mockRepository = new Mock<IExerciseSolutionRepository>();
            mockRepository.Setup(repo => repo.AddExerciseSolutionAsync(It.IsAny<ExerciseSolution>()))
                          .ReturnsAsync(expectedSolution);
            var service = new ExerciseSolutionService(mockRepository.Object);

            // Act
            var result = await service.CreateExerciseSolutionAsync(createRequest);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedSolution.Id, result.Id);
            Assert.Equal(expectedSolution.ExerciseId, result.ExerciseId);
            Assert.Equal(expectedSolution.Content, result.Content);
            Assert.Equal(expectedSolution.VideoUrl, result.VideoUrl);




        }




    }

    public class UpdateExerciseSolutionServiceTests
    {
        [Fact]
        public async Task UpdateExerciseSolutionAsync_Should_Return_Updated_ExerciseSolution()
        {

        }



    }
}

