using Exercise_Application.DTO;
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
    public class CreateExerciseKeywordServiceTests
    {
        [Fact]
        public async Task AddExerciseKeywordAsync_ShouldReturnExerciseKeyword()
        {
            // Arrange
            var mockRepository = new Mock<IExerciseKeywordRepository>();
            var service = new ExerciseKeywordService(mockRepository.Object);
            var dto = new ExerciseKeywordDTO
            {
                ExerciseId = Guid.NewGuid(),
                KeywordId = Guid.NewGuid()
            };
            var expectedExerciseKeyword = new ExerciseKeyword(dto.ExerciseId, dto.KeywordId);
            mockRepository.Setup(repo => repo.AddExerciseKeywordAsync(It.IsAny<ExerciseKeyword>()))
                          .ReturnsAsync(expectedExerciseKeyword);
            // Act
            var result = await service.AddExerciseKeywordAsync(dto);
            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedExerciseKeyword.ExerciseId, result.ExerciseId);
            Assert.Equal(expectedExerciseKeyword.KeywordId, result.KeywordId);
        }

        [Fact]
        public async Task AddExerciseKeywordAsync_ShouldCallRepositoryMethod()
        {
            // Arrange
            var mockRepository = new Mock<IExerciseKeywordRepository>();
            var service = new ExerciseKeywordService(mockRepository.Object);
            var dto = new ExerciseKeywordDTO
            {
                ExerciseId = Guid.NewGuid(),
                KeywordId = Guid.NewGuid()
            };
            mockRepository.Setup(repo => repo.AddExerciseKeywordAsync(It.IsAny<ExerciseKeyword>()))
                          .ReturnsAsync(new ExerciseKeyword(dto.ExerciseId, dto.KeywordId));
            // Act
            var result = await service.AddExerciseKeywordAsync(dto);
            // Assert
            mockRepository.Verify(repo => repo.AddExerciseKeywordAsync(It.IsAny<ExerciseKeyword>()), Times.Once);
        }

        public class ReadExerciseKeywordServiceTests
        {
            [Fact]
            public async Task GetExerciseKeywordsByExerciseIdAsync_ShouldReturnExerciseKeyword()
            {
                // Arrange
                var mockRepository = new Mock<IExerciseKeywordRepository>();
                var service = new ExerciseKeywordService(mockRepository.Object);
                var exerciseId = Guid.NewGuid();

                var expectedExerciseKeywords = new List<ExerciseKeyword>
                {
                    new ExerciseKeyword(exerciseId, Guid.NewGuid()),
                    new ExerciseKeyword(exerciseId, Guid.NewGuid())
                };

                mockRepository.Setup(repo => repo.GetExerciseKeywordsByExerciseIdAsync(exerciseId))
                              .ReturnsAsync(expectedExerciseKeywords);
                // Act
                var result = await service.GetExerciseKeywordsByExerciseIdAsync(exerciseId);

                // Assert
                Assert.NotNull(result);
                Assert.Equal(expectedExerciseKeywords.Count, result.Count());
                foreach (var expected in expectedExerciseKeywords)
                {
                    Assert.Contains(result, ek => ek.ExerciseId == expected.ExerciseId && ek.KeywordId == expected.KeywordId);
                }



            }

            [Fact]
            public async Task GetExerciseKeywordsByExerciseIdAsync_ShouldCallRepositoryMethod()
            {
                // Arrange
                var mockRepository = new Mock<IExerciseKeywordRepository>();
                var service = new ExerciseKeywordService(mockRepository.Object);
                var exerciseId = Guid.NewGuid();

                var expectedExerciseKeywords = new List<ExerciseKeyword>
                {
                    new ExerciseKeyword(exerciseId, Guid.NewGuid()),
                    new ExerciseKeyword(exerciseId, Guid.NewGuid())
                };

                mockRepository.Setup(repo => repo.GetExerciseKeywordsByExerciseIdAsync(exerciseId))
                              .ReturnsAsync(expectedExerciseKeywords);
                // Act
                var result = await service.GetExerciseKeywordsByExerciseIdAsync(exerciseId);

                // Assert
                mockRepository.Verify(repo => repo.GetExerciseKeywordsByExerciseIdAsync(exerciseId), Times.Once);

            }
        }
    }
}
