using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exercise_Tests
{
    public class ExerciseSolutionTests
    {
        [Theory]
        [InlineData("Some content", null)]
        [InlineData("Some content", "https://video.com")]
        [InlineData("", null)]
        [InlineData(" ", "https://video.com")]
        public void Constructor_Should_Set_Properties_Correctly(string content, string? videoUrl)
        {
            // Arrange
            var exerciseId = Guid.NewGuid();

            // Act
            var solution = new ExerciseSolution(exerciseId, content, videoUrl);

            // Assert
            Assert.NotEqual(Guid.Empty, solution.Id);
            Assert.Equal(exerciseId, solution.ExerciseId);
            Assert.Equal(content, solution.Content);
            Assert.Equal(videoUrl, solution.VideoUrl);
        }
    }
}
