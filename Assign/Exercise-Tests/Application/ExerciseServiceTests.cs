using Exercise_Application.DTO;
using Exercise_Application.Implementations;
using Exercise_Application.Interfaces.Repositories;
using FluentAssertions.Common;
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
            var expectedExercise = new Exercise(exerciseDto.Title, exerciseDto.Content, exerciseDto.CreatedByTeacherId);
            mockRepo.Setup(r => r.AddExerciseAsync(It.IsAny<Exercise>())).ReturnsAsync(expectedExercise);
            var service = new ExerciseService(mockRepo.Object);

            // Act
            var result = await service.CreateExerciseAsync(exerciseDto);

            // Assert

            mockRepo.Verify(r => r.AddExerciseAsync(It.IsAny<Exercise>()), Times.Once);
        }

        [Fact]
        public async Task CreateExercise_ShouldReturnCreatedExercise()
        {
            // Arrange
            var mockRepo = new Mock<IExerciseRepository>();
            var exerciseDto = new CreateExerciseRequest
            {
                Title = "Title1",
                Content = "Content1",
                CreatedAt = DateTime.UtcNow,
                CreatedByTeacherId = Guid.NewGuid(),
            };
            var expectedExercise = new Exercise(exerciseDto.Title, exerciseDto.Content, exerciseDto.CreatedByTeacherId);
            mockRepo.Setup(r => r.AddExerciseAsync(It.IsAny<Exercise>())).ReturnsAsync(expectedExercise);
            var service = new ExerciseService(mockRepo.Object);
            // Act
            var result = await service.CreateExerciseAsync(exerciseDto);
            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedExercise.Id, result.Id);
            Assert.Equal(expectedExercise.Title, result.Title);
            Assert.Equal(expectedExercise.Content, result.Content);
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


        [Fact]
        public async Task GetExerciseByIdAsync_WhenExerciseExists_ReturnsExercise()
        {
            // Arrange
            var exercise = new Exercise("Title1", "Content1", Guid.NewGuid());
            var mockRepo = new Mock<IExerciseRepository>();

            mockRepo.Setup(r => r.GetExerciseByIdAsync(exercise.Id))
                    .ReturnsAsync(exercise);

            var service = new ExerciseService(mockRepo.Object);

            // Act
            var result = await service.GetExerciseByIdAsync(exercise.Id);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(exercise.Id, result.Id);
            Assert.Equal(exercise.Title, result.Title);
            Assert.Equal(exercise.Content, result.Content);
        }



    }


    public class UpdateExerciseServiceTests
    {
        [Fact]
        public async Task UpdateExerciseAsync_ShouldCallRepositoryUpdate()
        {
            // Arrange
            var exercise = new Exercise("Title1", "Content1", Guid.NewGuid());
            var mockRepo = new Mock<IExerciseRepository>();
            var updateRequest = new UpdateExerciseRequest
            {
                Title = "Updated Title",
                Content = "Updated Content",
            };

            mockRepo.Setup(r => r.GetExerciseByIdAsync(exercise.Id))
                    .ReturnsAsync(exercise);
            mockRepo.Setup(r => r.UpdateExerciseAsync(exercise, exercise.RowVersion))
                    .ReturnsAsync(exercise);

            var service = new ExerciseService(mockRepo.Object);

            // Act
            var result = await service.UpdateExerciseAsync(exercise.Id, updateRequest);

            // Assert
            mockRepo.Verify(r => r.UpdateExerciseAsync(It.IsAny<Exercise>(), It.IsAny<byte[]>()), Times.Once);
        }

        [Fact]
        public async Task UpdateExerciseAsync_shouldUpdateExerciseProperties()
        {
            // Arrange
            var exercise = new Exercise("Title1", "Content1", Guid.NewGuid());
            var mockRepo = new Mock<IExerciseRepository>();
            var updateRequest = new UpdateExerciseRequest
            {
                Title = "Updated Title",
                Content = "Updated Content",
            };
            mockRepo.Setup(r => r.GetExerciseByIdAsync(exercise.Id))
                    .ReturnsAsync(exercise);
            mockRepo.Setup(r => r.UpdateExerciseAsync(exercise, exercise.RowVersion))
                    .ReturnsAsync(exercise);
            var service = new ExerciseService(mockRepo.Object);
            // Act
            var result = await service.UpdateExerciseAsync(exercise.Id, updateRequest);

            // Assert
            Assert.Equal(updateRequest.Title, exercise.Title);
            Assert.Equal(updateRequest.Content, exercise.Content);
        }
    }
}
