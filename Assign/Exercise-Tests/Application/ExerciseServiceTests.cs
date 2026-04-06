using Exercise_Application.DTO;
using Exercise_Application.Implementations;
using Exercise_Application.Interfaces.Repositories;
using Moq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exercise_Tests.Application
{
    public class ExerciseServiceTests
    {

    }

    public class CreateExerciseServiceTests
    {
        public static IEnumerable<object[]> ExerciseTestData()
        {
            yield return new object[]
            {
            new CreateExerciseRequest
            {
                Id = Guid.NewGuid(),
                Title = "Title1",
                Content = "Content1",
                CreatedAt = DateTime.UtcNow,
                CreatedByTeacherId = Guid.NewGuid(),
            }
            };

            yield return new object[]
            {
            new CreateExerciseRequest
            {
                Id = Guid.NewGuid(),
                Title = "Title2",
                Content = "Content2",
                CreatedAt = DateTime.UtcNow,
                CreatedByTeacherId = Guid.NewGuid(),
            }
            };
        }

        [Theory]
        [MemberData(nameof(ExerciseTestData))]
        public async Task CreateExercise_ShouldCallRepositorySave(CreateExerciseRequest exerciseDto)
        {
            // Arrange
            var mockRepo = new Mock<IExerciseRepository>();
            var service = new ExerciseService(mockRepo.Object);

            // Act
            await service.CreateExerciseAsync(exerciseDto);

            // Assert
            mockRepo.Verify(
                r => r.AddExerciseAsync(It.Is<Exercise>(e =>
                    e.Title == exerciseDto.Title &&
                    e.Content == exerciseDto.Content &&
                    e.CreatedByTeacherId == exerciseDto.CreatedByTeacherId
                )), Times.Once);
        }

        [Fact]
        public async Task CreateExercise_ShouldThrowException_WhenRepositoryFails()
        {
            // Arrange
            var mockRepo = new Mock<IExerciseRepository>();
            var service = new ExerciseService(mockRepo.Object);
            var exerciseDto = new CreateExerciseRequest
            {
                Id = Guid.NewGuid(),
                Title = "Title",
                Content = "Content",
                CreatedAt = DateTime.UtcNow,
                CreatedByTeacherId = Guid.NewGuid(),
            };
            mockRepo.Setup(r => r.AddExerciseAsync(It.IsAny<Exercise>()))
                    .ThrowsAsync(new Exception("Database error"));

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => service.CreateExerciseAsync(exerciseDto));
        }
    }

    public class ReadExerciseServiceTests
    {
        public static IEnumerable<object[]> KeywordSearchTestData()
        {
            yield return new object[]
            {
                new List<Guid> { Guid.NewGuid(), Guid.NewGuid() }, // input keyword IDs
                new List<Exercise>
                {
                    new Exercise("Title1", "Content1", Guid.NewGuid()),
                    new Exercise("Title2", "Content2", Guid.NewGuid())
                }
            };

            yield return new object[]
            {
                new List<Guid>(), //empty keyword list
                new List<Exercise>()// no exercises
            };
        }

        [Theory]
        [MemberData(nameof(KeywordSearchTestData))]
        public async Task GetExerciseByExerciseKeywords_ShouldReturnExpected(List<Guid> keywordIds, List<Exercise> repoResult)
        {
            // Arrange
            var mockRepo = new Mock<IExerciseRepository>();
            mockRepo.Setup(r => r.GetExercisesByKeywordsAsync(keywordIds)).ReturnsAsync(repoResult);

            var service = new ExerciseService(mockRepo.Object);

            // Act
            var result = await service.GetExerciseByExerciseKeywords(keywordIds);

            // Assert
            Assert.Equal(repoResult.Count, result.Count());
            foreach (var exercise in repoResult)
            {
                Assert.Contains(result, r => r.Id == exercise.Id && r.Title == exercise.Title);
            }
        }



        public static IEnumerable<object[]> TeacherExerciseTestData()
        {
            yield return new object[]
            {
                Guid.NewGuid(), //Teacher id
                new List<Exercise>
                {
                    new Exercise("Title1", "Content1", Guid.NewGuid()),
                    new Exercise("Title2", "Content2", Guid.NewGuid())
                }
            };
            yield return new object[]
            {
                Guid.NewGuid(), // teacher with no exercises
                new List<Exercise>()
            };
        }

        [Theory]
        [MemberData(nameof(TeacherExerciseTestData))]
        public async Task GetExercisesByTeacherIdAsync_ShouldReturnExpected(Guid teacherId, List<Exercise> repoResult)
        {
            // Arrange
            var mockRepo = new Mock<IExerciseRepository>();
            mockRepo.Setup(r => r.GetExercisesByTeacherIdAsync(teacherId))
                    .ReturnsAsync(repoResult);

            var service = new ExerciseService(mockRepo.Object);

            // Act
            var result = await service.GetExercisesByTeacherIdAsync(teacherId);

            // Assert
            Assert.Equal(repoResult.Count, result.Count());
            foreach (var exercise in repoResult)
            {
                Assert.Contains(result, r => r.Id == exercise.Id && r.Title == exercise.Title);
            }
        }

        public static IEnumerable<object[]> ExerciseByIdTestData()
        {
            yield return new object[]
            {
                Guid.NewGuid(), // Exercise id
                new Exercise("Title1", "Content1", Guid.NewGuid())
            };
            yield return new object[]
            {
                Guid.NewGuid(), // non existing exercise id
                null
            };
        }

        [Theory]
        [MemberData(nameof(ExerciseByIdTestData))]
        public async Task GetExerciseByIdAsync_ShouldReturnExpected(Guid exerciseId, Exercise? repoResult)
        {
            // Arrange
            var mockRepo = new Mock<IExerciseRepository>();
            mockRepo.Setup(r => r.GetExerciseByIdAsync(exerciseId))
                    .ReturnsAsync(repoResult);
            var service = new ExerciseService(mockRepo.Object);
            // Act
            var result = await service.GetExerciseByIdAsync(exerciseId);
            // Assert
            if (repoResult == null)
            {
                Assert.Null(result);
            }
            else
            {
                Assert.NotNull(result);
                Assert.Equal(repoResult.Id, result.Id);
                Assert.Equal(repoResult.Title, result.Title);
                Assert.Equal(repoResult.Content, result.Content);
                Assert.Equal(repoResult.CreatedByTeacherId, result.CreatedByTeacherId);
            }
        }
}









