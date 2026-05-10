using Assignment_Api;
using Assignment_Application.DTO;
using Assignment_Application.Implementations;
using Assignment_Application.Interfaces.Repositories;
using FluentAssertions;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace Assignment_Tests.Application
{
    public class AssignmentSetServiceTests_CreateAssignmentSetAsync_Tests
    {
        [Fact]
        public async Task CreateAssignmentSetAsync_Should_Return_AssignmentSet_With_Id()
        {
            // Arrange
            var repoMock = new Mock<IAssignmentSetRepository>();
            repoMock.Setup(r => r.CreateAsync(It.IsAny<AssignmentSet>())).Returns(Task.CompletedTask);

            var service = new AssignmentSetService(repoMock.Object);
            var request = new CreateAssignmentSetRequest
            {
                CourseId = Guid.NewGuid(),
                Title = "Assignment Set",
                Description = "Description"
            };

            // Act
            var result = await service.CreateAssignmentSetAsync(request);

            // Assert
            result.Should().NotBeNull();
            result.Id.Should().NotBe(Guid.Empty);
            result.CourseId.Should().Be(request.CourseId);
            result.Title.Should().Be(request.Title);
            result.Description.Should().Be(request.Description);
            repoMock.Verify(r => r.CreateAsync(It.IsAny<AssignmentSet>()), Times.Once);

            repoMock.Verify(
                r => r.SaveChangesAsync(),
                Times.Once);
        }

        [Fact]
        public async Task CreateAssignmentSetAsync_Should_Throw_If_Course_Is_Invalid()
        {
            // Arrange
            var repoMock = new Mock<IAssignmentSetRepository>();
            var service = new AssignmentSetService(repoMock.Object);
            var request = new CreateAssignmentSetRequest
            {
                CourseId = Guid.Empty,
                Title = "Assignment Set",
                Description = "Description"
            };

            // Act
            Func<Task> act = async () =>
                await service.CreateAssignmentSetAsync(request);

            // Assert
            await act.Should()
                .ThrowAsync<ArgumentException>();

            repoMock.Verify(
                r => r.CreateAsync(It.IsAny<AssignmentSet>()),
                Times.Never);

            repoMock.Verify(
                r => r.SaveChangesAsync(),
                Times.Never);
        }

        [Fact]
        public async Task CreateAssignmentSetAsync_Should_Initialize_Assignments_Collection()
        {
            // Arrange
            var repoMock = new Mock<IAssignmentSetRepository>();

            repoMock.Setup(r => r.CreateAsync(It.IsAny<AssignmentSet>()))
                .Returns(Task.CompletedTask);

            var service = new AssignmentSetService(repoMock.Object);

            var request = new CreateAssignmentSetRequest
            {
                CourseId = Guid.NewGuid(),
                Title = "Assignment Set",
                Description = "Description"
            };

            // Act
            var result = await service.CreateAssignmentSetAsync(request);

            // Assert
            result.Assignments.Should().NotBeNull();
            result.Assignments.Should().BeEmpty();
        }
    }

    public class AssignmentSetServiceTests_GetAssignmentSetsAsync_Tests
    {
        [Fact]
        public async Task GetAssignmentSetsAsync_Should_Return_AssignmentSet_When_Found()
        {
            // Arrange
            var assignmentSetId = Guid.NewGuid();

            var assignmentSet = new AssignmentSet(
                Guid.NewGuid(),
                "Assignment Set",
                "Description");

            var repoMock = new Mock<IAssignmentSetRepository>();

            repoMock.Setup(r => r.GetByIdAsync(assignmentSetId))
                .ReturnsAsync(assignmentSet);

            var service = new AssignmentSetService(repoMock.Object);

            // Act
            var result = await service.GetAssignmentSetsAsync(assignmentSetId);

            // Assert
            result.Should().NotBeNull();

            result.Title.Should().Be(assignmentSet.Title);

            result.Description.Should().Be(assignmentSet.Description);
        }

        [Fact]
        public async Task GetAssignmentSetsAsync_Should_Throw_NotFound_When_Id_Does_Not_Exist()
        {
            // Arrange
            var repoMock = new Mock<IAssignmentSetRepository>();

            repoMock.Setup(r => r.GetByIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync((AssignmentSet?)null);

            var service = new AssignmentSetService(repoMock.Object);

            // Act
            Func<Task> act = async () =>
                await service.GetAssignmentSetsAsync(Guid.NewGuid());

            // Assert
            await act.Should()
                .ThrowAsync<KeyNotFoundException>();
        }

        [Fact]
        public async Task GetAssignmentSetsAsync_Should_Include_Assignments_And_Exercises()
        {
            // Arrange
            var assignmentSetId = Guid.NewGuid();

            var assignmentSet = new AssignmentSet(
                Guid.NewGuid(),
                "Assignment Set",
                "Description");

            var assignment = new Assignment("Assignment","Assignment Description");

            assignment.AddExercise(Guid.NewGuid());

            assignmentSet.AddAssignment(assignment);

            var repoMock = new Mock<IAssignmentSetRepository>();

            repoMock.Setup(r => r.GetByIdAsync(assignmentSetId))
                .ReturnsAsync(assignmentSet);

            var service = new AssignmentSetService(repoMock.Object);

            // Act
            var result = await service.GetAssignmentSetsAsync(assignmentSetId);

            // Assert
            result.Assignments.Should().ContainSingle();

            result.Assignments.First()
                .Exercises.Should()
                .ContainSingle();
        }
    }

    public class AssignmentSetServiceTests_GetAssignmentSetsByCourseIdAsync_Tests
    {
        [Fact]
        public async Task GetAssignmentSetsByCourseIdAsync_Should_Return_All_AssignmentSets_For_Course()
        {
            // Arrange
            var courseId = Guid.NewGuid();

            var assignmentSets = new List<AssignmentSet>
        {
            new(courseId, "Set 1", "Description 1"),
            new(courseId, "Set 2", "Description 2")
        };

            var repoMock = new Mock<IAssignmentSetRepository>();

            repoMock.Setup(r => r.GetByCourseIdAsync(courseId))
                .ReturnsAsync(assignmentSets);

            var service = new AssignmentSetService(repoMock.Object);

            // Act
            var result = await service.GetAssignmentSetsByCourseIdAsync(courseId);

            // Assert
            result.Should().HaveCount(2);
        }

        [Fact]
        public async Task GetAssignmentSetsByCourseIdAsync_Should_Return_Empty_Collection_When_None_Exist()
        {
            // Arrange
            var repoMock = new Mock<IAssignmentSetRepository>();

            repoMock.Setup(r => r.GetByCourseIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync(new List<AssignmentSet>());

            var service = new AssignmentSetService(repoMock.Object);

            // Act
            var result = await service.GetAssignmentSetsByCourseIdAsync(Guid.NewGuid());

            // Assert
            result.Should().BeEmpty();
        }

        [Fact]
        public async Task GetAssignmentSetsByCourseIdAsync_Should_Filter_By_CourseId_Correctly()
        {
            // Arrange
            var courseId = Guid.NewGuid();

            var assignmentSets = new List<AssignmentSet>
        {
            new(courseId, "Set 1", "Description 1"),
            new(courseId, "Set 2", "Description 2")
        };

            var repoMock = new Mock<IAssignmentSetRepository>();

            repoMock.Setup(r => r.GetByCourseIdAsync(courseId))
                .ReturnsAsync(assignmentSets);

            var service = new AssignmentSetService(repoMock.Object);

            // Act
            var result = await service.GetAssignmentSetsByCourseIdAsync(courseId);

            // Assert
            result.Should()
                .OnlyContain(x => x.CourseId == courseId);
        }
    }

    public class AssignmentSetServiceTests_UpdateAssignmentSetAsync_Tests
    {
        [Fact]
        public async Task UpdateAssignmentSetAsync_Should_Update_Title_And_Description()
        {
            // Arrange
            var assignmentSetId = Guid.NewGuid();

            var assignmentSet = new AssignmentSet(
                Guid.NewGuid(),
                "Old Title",
                "Old Description");

            var repoMock = new Mock<IAssignmentSetRepository>();

            repoMock.Setup(r => r.GetByIdAsync(assignmentSetId))
                .ReturnsAsync(assignmentSet);

            var service = new AssignmentSetService(repoMock.Object);

            var request = new UpdateAssignmentSetRequest
            {
                Title = "New Title",
                Description = "New Description"
            };

            // Act
            var result = await service.UpdateAssignmentSetAsync(
                assignmentSetId,
                request);

            // Assert
            result.Title.Should().Be(request.Title);

            result.Description.Should().Be(request.Description);

            repoMock.Verify(
                r => r.SaveChangesAsync(),
                Times.Once);
        }

        [Fact]
        public async Task UpdateAssignmentSetAsync_Should_Throw_NotFound_When_AssignmentSet_Does_Not_Exist()
        {
            // Arrange
            var repoMock = new Mock<IAssignmentSetRepository>();

            repoMock.Setup(r => r.GetByIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync((AssignmentSet?)null);

            var service = new AssignmentSetService(repoMock.Object);

            var request = new UpdateAssignmentSetRequest
            {
                Title = "Updated",
                Description = "Updated"
            };

            // Act
            Func<Task> act = async () =>
                await service.UpdateAssignmentSetAsync(
                    Guid.NewGuid(),
                    request);

            // Assert
            await act.Should()
                .ThrowAsync<KeyNotFoundException>();
        }

        [Fact]
        public async Task UpdateAssignmentSetAsync_Should_Handle_Concurrency_RowVersion_Mismatch()
        {
            // Arrange
            var assignmentSet = new AssignmentSet(
                Guid.NewGuid(),
                "Title",
                "Description");

            var repoMock = new Mock<IAssignmentSetRepository>();

            repoMock.Setup(r => r.GetByIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync(assignmentSet);

            repoMock.Setup(r => r.SaveChangesAsync())
                .ThrowsAsync(new InvalidOperationException("RowVersion mismatch"));

            var service = new AssignmentSetService(repoMock.Object);
            var request = new UpdateAssignmentSetRequest
            {
                Title = "Updated",
                Description = "Updated"
            };

            // Act
            Func<Task> act = async () =>
                await service.UpdateAssignmentSetAsync(
                    Guid.NewGuid(),
                    request);

            // Assert
            await act.Should()
                .ThrowAsync<InvalidOperationException>()
                .WithMessage("*RowVersion mismatch*");
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        public async Task UpdateAssignmentSetAsync_Should_Validate_Input_And_Reject_Invalid_Updates(
            string title)
        {
            // Arrange
            var assignmentSet = new AssignmentSet(
                Guid.NewGuid(),
                "Title",
                "Description");

            var repoMock = new Mock<IAssignmentSetRepository>();
            repoMock.Setup(r => r.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(assignmentSet);

            var service = new AssignmentSetService(repoMock.Object);
            var request = new UpdateAssignmentSetRequest
            {
                Title = title,
                Description = "Updated Description"
            };

            // Act
            Func<Task> act = async () =>
                await service.UpdateAssignmentSetAsync(
                    Guid.NewGuid(),
                    request);

            // Assert
            await act.Should()
                .ThrowAsync<ArgumentException>();

            repoMock.Verify(
                r => r.SaveChangesAsync(),
                Times.Never);
        }
    }
}

