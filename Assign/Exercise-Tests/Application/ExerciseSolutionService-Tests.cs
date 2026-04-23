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
    public class SetExerciseSolution_Tests
    {
        [Fact]
        public async Task SetExerciseSolution_Should_Set_Solution_For_Exercise()
        {
            // Arrange
            var exercise = new Exercise("title", "content", Guid.NewGuid());
            var request = new CreateExerciseSolutionRequest
            {
                Content = "content",
                VideoUrl = "video url"
            };

            var mockRepo = new Mock<IExerciseRepository>();
            mockRepo.Setup(r => r.GetByIdAsync(exercise.Id))
                    .ReturnsAsync(exercise);

            exercise.SetSolution(request.Content, request.VideoUrl);
            mockRepo.Setup(r => r.UpdateAsync(exercise, exercise.RowVersion)).ReturnsAsync(exercise);

            var service = new ExerciseSolutionService(mockRepo.Object);

            // Act

            var result = await service.SetExerciseSolution(exercise.Id, request);

            //Assert
            Assert.NotNull(result);
            Assert.Equal(exercise.Solution.Content, result.Solution.Content);
            Assert.Equal(exercise.Solution.VideoUrl, result.Solution.VideoUrl);
        }

        [Fact]
        public async Task SetExerciseSolution_Should_Overwrite_Existing_Solution()
        {
            // Arrange
            var exercise = new Exercise("title", "content", Guid.NewGuid());
            exercise.SetSolution("content", "video url");
            var request = new CreateExerciseSolutionRequest
            {
                Content = "new content",
                VideoUrl = "new video url"
            };

            var mockRepo = new Mock<IExerciseRepository>();
            mockRepo.Setup(r => r.GetByIdAsync(exercise.Id))
                    .ReturnsAsync(exercise);

            exercise.SetSolution(request.Content, request.VideoUrl);
            mockRepo.Setup(r => r.UpdateAsync(exercise, exercise.RowVersion)).ReturnsAsync(exercise);

            var service = new ExerciseSolutionService(mockRepo.Object);

            // Act

            var result = await service.SetExerciseSolution(exercise.Id, request);

            //Assert
            Assert.NotNull(result);
            Assert.Equal(exercise.Solution.Content, result.Solution.Content);
            Assert.Equal(exercise.Solution.VideoUrl, result.Solution.VideoUrl);
        }
    }

    public class RemoveExerciseSolution_Tests
    {
        [Fact]
        public async Task RemoveExerciseSolution_Should_Remove_Solution_From_Exercise()
        {
            // Arrange
            var exercise = new Exercise("title", "content", Guid.NewGuid());
            exercise.SetSolution("content", "video url");

            var mockRepo = new Mock<IExerciseRepository>();
            mockRepo.Setup(r => r.GetByIdAsync(exercise.Id))
                    .ReturnsAsync(exercise);

            exercise.RemoveSolution();
            mockRepo.Setup(r => r.UpdateAsync(exercise, exercise.RowVersion)).ReturnsAsync(exercise);

            var service = new ExerciseSolutionService(mockRepo.Object);

            // Act

            var result = await service.RemoveExerciseSolutionAsync(exercise.Id);

            //Assert
            Assert.NotNull(result);
            Assert.Null(result.Solution);
        }
    }
}
