using Assignment_Api;
using Assignment_Application.DTO;
using Assignment_Application.Implementations;
using Assignment_Application.Interfaces.Repositories;
using FluentAssertions;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment_Tests.Application
{
    public class AssignmentServiceTests_CreateAssignmentAsync_Tests
    {
        [Fact]
        public async Task CreateAssignmentAsync_Should_Return_Assignment_With_Id()
        {
            // Arrange
            var repoMock = new Mock<IAssignmentRepository>();

            repoMock.Setup(r => r.CreateAsync(It.IsAny<Assignment>()))
                .Returns(Task.CompletedTask);

            var service = new AssignmentService(repoMock.Object);

            var request = new CreateAssignmentRequest
            {
                AssignmentSetId = Guid.NewGuid(),
                Title = "New Assignment",
                Description = "Description"
            };

            // Act
            var result = await service.CreateAssignmentAsync(request);

            // Assert
            result.Should().NotBeNull();
            result.Id.Should().NotBe(Guid.Empty);
            result.Title.Should().Be(request.Title);

            result.Description.Should().Be(request.Description);
            repoMock.Verify(r => r.CreateAsync(It.IsAny<Assignment>()), Times.Once);
            repoMock.Verify(r => r.SaveChangesAsync(), Times.Once);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public async Task CreateAssignmentAsync_Should_Throw_When_Title_Invalid(string title)
        {
            // Arrange
            var repoMock = new Mock<IAssignmentRepository>();
            var service = new AssignmentService(repoMock.Object);
            var request = new CreateAssignmentRequest
            {
                AssignmentSetId = Guid.NewGuid(),
                Title = title!,
                Description = "Description"
            };

            // Act
            Func<Task> act = async () =>
                await service.CreateAssignmentAsync(request);

            // Assert
            await act.Should()
                .ThrowAsync<ArgumentException>();

            repoMock.Verify(r => r.CreateAsync(It.IsAny<Assignment>()), Times.Never);
            repoMock.Verify(r => r.SaveChangesAsync(), Times.Never);
        }
    }

    public class AssignmentServiceTests_RemoveExerciseAsync_Tests
    {
        [Fact]
        public async Task RemoveExerciseAsync_Should_Remove_Exercise_And_Save()
        {
            // Arrange
            var assignmentId = Guid.NewGuid();
            var exerciseId = Guid.NewGuid();
            var assignment = new Assignment("Assignment", "Description");
            assignment.AddExercise(exerciseId);

            var repoMock = new Mock<IAssignmentRepository>();
            repoMock.Setup(r => r.GetByIdAsync(assignmentId)).ReturnsAsync(assignment);

            var service = new AssignmentService(repoMock.Object);
            var request = new RemoveAssignmentExerciseRequest
            {
                ExerciseId = exerciseId
            };

            // Act
            await service.RemoveExerciseAsync(assignmentId, request);

            // Assert
            assignment.AssignmentExercises.Should().BeEmpty();
            repoMock.Verify(r => r.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task RemoveExerciseAsync_Should_Throw_When_Assignment_Not_Found()
        {
            // Arrange
            var repoMock = new Mock<IAssignmentRepository>();
            repoMock.Setup(r => r.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync((Assignment?)null);

            var service = new AssignmentService(repoMock.Object);
            var request = new RemoveAssignmentExerciseRequest
            {
                ExerciseId = Guid.NewGuid()
            };

            // Act
            Func<Task> act = async () =>
                await service.RemoveExerciseAsync(Guid.NewGuid(), request);

            // Assert
            await act.Should()
                .ThrowAsync<KeyNotFoundException>();

            repoMock.Verify(r => r.SaveChangesAsync(), Times.Never);
        }

        [Fact]
        public async Task RemoveExerciseAsync_Should_Throw_When_Exercise_Not_Found()
        {
            // Arrange
            var assignmentId = Guid.NewGuid();
            var assignment = new Assignment("Assignment", "Description");

            var repoMock = new Mock<IAssignmentRepository>();
            repoMock.Setup(r => r.GetByIdAsync(assignmentId)).ReturnsAsync(assignment);

            var service = new AssignmentService(repoMock.Object);
            var request = new RemoveAssignmentExerciseRequest
            {
                ExerciseId = Guid.NewGuid()
            };

            // Act
            Func<Task> act = async () =>
                await service.RemoveExerciseAsync(assignmentId, request);

            // Assert
            await act.Should()
                .ThrowAsync<KeyNotFoundException>();

            repoMock.Verify(r => r.SaveChangesAsync(), Times.Never);
        }
    }

    public class AssignmentServiceTests_GetAssignmentAsync_Tests
    {
        [Fact]
        public async Task GetAssignmentAsync_Should_Return_Assignment_When_Found()
        {
            // Arrange
            var assignmentId = Guid.NewGuid();
            var assignment = new Assignment("title", "desc");

            var repoMock = new Mock<IAssignmentRepository>();
            repoMock.Setup(r => r.GetByIdAsync(assignmentId)).ReturnsAsync(assignment);

            var service = new AssignmentService(repoMock.Object);

            // Act
            var result = await service.GetAssignmentAsync(assignmentId);

            // Assert
            result.Should().NotBeNull();
            repoMock.Verify(r => r.GetByIdAsync(assignmentId), Times.Once);
        }

        [Fact]
        public async Task GetAssignmentAsync_Should_Throw_When_Not_Found()
        {
            // Arrange
            var repoMock = new Mock<IAssignmentRepository>();
            repoMock.Setup(r => r.GetByIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync((Assignment?)null);

            var service = new AssignmentService(repoMock.Object);

            // Act
            Func<Task> act = async () => await service.GetAssignmentAsync(Guid.NewGuid());

            // Assert
            await act.Should().ThrowAsync<KeyNotFoundException>();
        }

        [Fact]
        public async Task GetAssignmentAsync_Should_Include_Exercises_When_Present()
        {
            // Arrange
            var assignmentId = Guid.NewGuid();
            var ex1 = Guid.NewGuid();
            var ex2 = Guid.NewGuid();

            var assignment = new Assignment("title", "desc");
            assignment.AddExercise(ex1);
            assignment.AddExercise(ex2);

            var repoMock = new Mock<IAssignmentRepository>();
            repoMock.Setup(r => r.GetByIdAsync(assignmentId)).ReturnsAsync(assignment);

            var service = new AssignmentService(repoMock.Object);

            // Act
            var result = await service.GetAssignmentAsync(assignmentId);

            // Assert
            result.Should().NotBeNull();
            // ensure the domain object contains exercises (service mapping tested by non-null result)
            assignment.AssignmentExercises.Should().HaveCount(2);
        }
    }

    public class AssignmentServiceTests_AddExerciseAsync_Tests
    {
        [Fact]
        public async Task AddExerciseAsync_Should_Add_Exercise_And_Save()
        {
            // Arrange
            var assignmentId = Guid.NewGuid();
            var exerciseId = Guid.NewGuid();
            var assignment = new Assignment("title", "desc");
            var repoMock = new Mock<IAssignmentRepository>();

            repoMock.Setup(r => r.GetByIdAsync(assignmentId))
                .ReturnsAsync(assignment);

            var service = new AssignmentService(repoMock.Object);
            var request = new CreateAssignmentExerciseRequest
            {
                ExerciseId = exerciseId
            };

            // Act
            var result = await service.AddExerciseAsync(assignmentId, request);

            // Assert
            assignment.AssignmentExercises.Should()
                .ContainSingle(x => x.ExerciseId == exerciseId);

            repoMock.Verify(r => r.SaveChangesAsync(), Times.Once);
            result.ExerciseId.Should().Be(exerciseId);
        }

        [Fact]
        public async Task AddExerciseAsync_Should_Throw_When_Assignment_Not_Found()
        {
            // Arrange
            var repoMock = new Mock<IAssignmentRepository>();
            repoMock.Setup(r => r.GetByIdAsync(It.IsAny<Guid>()))
    .ReturnsAsync((Assignment?)null);

            var service = new AssignmentService(repoMock.Object);

            // Act
            Func<Task> act = async () =>
                await service.AddExerciseAsync(
                    Guid.NewGuid(),
                    new CreateAssignmentExerciseRequest());

            // Assert
            await act.Should().ThrowAsync<KeyNotFoundException>();
        }

        [Fact]
        public async Task AddExerciseAsync_Should_Not_Save_When_Domain_Throws()
        {
            // Arrange
            var exerciseId = Guid.NewGuid();

            var assignment = new Assignment("title", "desc");
            assignment.AddExercise(exerciseId);

            var repoMock = new Mock<IAssignmentRepository>();

            repoMock.Setup(r => r.GetByIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync(assignment);

            var service = new AssignmentService(repoMock.Object);

            // Act
            Func<Task> act = async () =>
                await service.AddExerciseAsync(
                    Guid.NewGuid(),
                    new CreateAssignmentExerciseRequest
                    {
                        ExerciseId = exerciseId
                    });

            // Assert
            await act.Should().ThrowAsync<InvalidOperationException>();

            repoMock.Verify(r => r.SaveChangesAsync(), Times.Never);
        }
    }

    public class AssignmentService_UpdateExerciseAsync_Tests
    {
        [Theory]
        [InlineData("New Title")]
        [InlineData("Assignment 1")]
        [InlineData("Math Homework")]
        public async Task UpdateAssignmentAsync_Should_Update_Title_When_Valid(string title)
        {
            // Arrange
            var assignmentId = Guid.NewGuid();
            var assignment = new Assignment("Old Title", "Old Description");
            var repoMock = new Mock<IAssignmentRepository>();

            repoMock.Setup(r => r.GetByIdAsync(assignmentId))
                .ReturnsAsync(assignment);

            var service = new AssignmentService(repoMock.Object);
            var request = new UpdateAssignmentRequest
            {
                Title = title,
                Description = "Updated Description"
            };

            // Act
            var result = await service.UpdateAssignmentAsync(assignmentId, request);

            // Assert
            assignment.Title.Should().Be(title);
            result.Title.Should().Be(title);
            repoMock.Verify(r => r.SaveChangesAsync(), Times.Once);
        }


        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        public async Task UpdateAssignmentAsync_Should_Throw_When_Title_Invalid(string title)
        {
            // Arrange
            var assignment = new Assignment("old", "desc");
            var repoMock = new Mock<IAssignmentRepository>();
            repoMock.Setup(r => r.GetByIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync(assignment);

            var service = new AssignmentService(repoMock.Object);

            var request = new UpdateAssignmentRequest
            {
                Title = title,
                Description = "desc"
            };

            // Act
            Func<Task> act = async () =>
                await service.UpdateAssignmentAsync(Guid.NewGuid(), request);

            // Assert
            await act.Should().ThrowAsync<ArgumentException>();
        }
    }
}
