using Assignment_Application.DTO;
using Assignment_Application.Implementations;
using Assignment_Application.Interfaces;
using Assignment_Application.Interfaces.Repositories;
using Assignment_Domain.Entities;
using Assignment_Domain.SnapShots;
using FluentAssertions;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Assignment_Tests.Application
{
    internal static class TestData
    {
        public static ExerciseSnapshotInput Snapshot(
            Guid? sourceExerciseId = null,
            int questionCount = 1) =>
            new(
                sourceExerciseId ?? Guid.NewGuid(),
                "Snapshot Title",
                "Snapshot Content",
                Enumerable.Range(0, questionCount)
                    .Select(i => new QuestionSnapshotInput(Guid.NewGuid(), $"Q{i}", "C"))
                    .ToList());
    }

    public class AssignmentServiceTests_CreateAssignmentAsync_Tests
    {
        [Fact]
        public async Task CreateAssignmentAsync_Should_Return_Assignment_With_Id()
        {
            // Arrange
            var repoMock = new Mock<IAssignmentRepository>();
            repoMock.Setup(r => r.CreateAsync(It.IsAny<Assignment>()))
                .Returns(Task.CompletedTask);
            var providerMock = new Mock<IExerciseProvider>();

            var service = new AssignmentService(repoMock.Object, providerMock.Object);
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
            var providerMock = new Mock<IExerciseProvider>();
            var service = new AssignmentService(repoMock.Object, providerMock.Object);
            var request = new CreateAssignmentRequest
            {
                AssignmentSetId = Guid.NewGuid(),
                Title = title!,
                Description = "Description"
            };

            // Act
            Func<Task> act = () => service.CreateAssignmentAsync(request);

            // Assert
            await act.Should().ThrowAsync<ArgumentException>();
            repoMock.Verify(r => r.CreateAsync(It.IsAny<Assignment>()), Times.Never);
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
            var providerMock = new Mock<IExerciseProvider>();

            var service = new AssignmentService(repoMock.Object, providerMock.Object);

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
            var providerMock = new Mock<IExerciseProvider>();

            var service = new AssignmentService(repoMock.Object, providerMock.Object);

            // Act
            Func<Task> act = () => service.GetAssignmentAsync(Guid.NewGuid());

            // Assert
            await act.Should().ThrowAsync<KeyNotFoundException>();
        }

        [Fact]
        public async Task GetAssignmentAsync_Should_Map_AssignmentExercises_With_Questions()
        {
            // Arrange
            var assignmentId = Guid.NewGuid();
            var assignment = new Assignment("title", "desc");
            var snapshot = TestData.Snapshot(questionCount: 2);
            var ae = assignment.AddExerciseFromSnapshot(snapshot);
            ae.SetQuestionPoints(ae.Questions[0].Id, 3);
            ae.SetQuestionPoints(ae.Questions[1].Id, 7);

            var repoMock = new Mock<IAssignmentRepository>();
            repoMock.Setup(r => r.GetByIdAsync(assignmentId)).ReturnsAsync(assignment);
            var providerMock = new Mock<IExerciseProvider>();

            var service = new AssignmentService(repoMock.Object, providerMock.Object);

            // Act
            var result = await service.GetAssignmentAsync(assignmentId);

            // Assert
            result.Should().NotBeNull();
            result.TotalPoints.Should().Be(10);
            // Domain side-by-side check (mapping is service responsibility):
            assignment.AssignmentExercises.Should().ContainSingle();
            assignment.AssignmentExercises.First().Questions.Should().HaveCount(2);
        }
    }

    public class AssignmentServiceTests_AddExerciseAsync_Tests
    {
        [Fact]
        public async Task AddExerciseAsync_Should_Fetch_Snapshot_From_Provider_And_Save()
        {
            // Arrange
            var assignmentId = Guid.NewGuid();
            var sourceExerciseId = Guid.NewGuid();
            var assignment = new Assignment("title", "desc");
            var snapshot = TestData.Snapshot(sourceExerciseId, questionCount: 2);

            var repoMock = new Mock<IAssignmentRepository>();
            repoMock.Setup(r => r.GetByIdAsync(assignmentId)).ReturnsAsync(assignment);

            var providerMock = new Mock<IExerciseProvider>();
            providerMock.Setup(p => p.GetExerciseSnapshotAsync(sourceExerciseId))
                .ReturnsAsync(snapshot);

            var service = new AssignmentService(repoMock.Object, providerMock.Object);
            var request = new CreateAssignmentExerciseRequest { ExerciseId = sourceExerciseId };

            // Act
            var result = await service.AddExerciseAsync(assignmentId, request);

            // Assert
            assignment.AssignmentExercises.Should().ContainSingle();
            var ae = assignment.AssignmentExercises.First();
            ae.SourceExerciseId.Should().Be(sourceExerciseId);
            ae.Questions.Should().HaveCount(2);

            providerMock.Verify(p => p.GetExerciseSnapshotAsync(sourceExerciseId), Times.Once);
            repoMock.Verify(r => r.SaveChangesAsync(), Times.Once);

            result.SourceExerciseId.Should().Be(sourceExerciseId);
            result.Questions.Should().HaveCount(2);
        }

        [Fact]
        public async Task AddExerciseAsync_Should_Throw_When_Assignment_Not_Found()
        {
            // Arrange
            var repoMock = new Mock<IAssignmentRepository>();
            repoMock.Setup(r => r.GetByIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync((Assignment?)null);
            var providerMock = new Mock<IExerciseProvider>();

            var service = new AssignmentService(repoMock.Object, providerMock.Object);
            var request = new CreateAssignmentExerciseRequest { ExerciseId = Guid.NewGuid() };

            // Act
            Func<Task> act = () => service.AddExerciseAsync(Guid.NewGuid(), request);

            // Assert
            await act.Should().ThrowAsync<KeyNotFoundException>();
            providerMock.Verify(p => p.GetExerciseSnapshotAsync(It.IsAny<Guid>()), Times.Never);
            repoMock.Verify(r => r.SaveChangesAsync(), Times.Never);
        }

        [Fact]
        public async Task AddExerciseAsync_Should_Throw_When_Source_Exercise_Not_Found()
        {
            // Arrange
            var assignmentId = Guid.NewGuid();
            var assignment = new Assignment("title", "desc");

            var repoMock = new Mock<IAssignmentRepository>();
            repoMock.Setup(r => r.GetByIdAsync(assignmentId)).ReturnsAsync(assignment);

            var providerMock = new Mock<IExerciseProvider>();
            providerMock.Setup(p => p.GetExerciseSnapshotAsync(It.IsAny<Guid>()))
                .ReturnsAsync((ExerciseSnapshotInput?)null);

            var service = new AssignmentService(repoMock.Object, providerMock.Object);
            var request = new CreateAssignmentExerciseRequest { ExerciseId = Guid.NewGuid() };

            // Act
            Func<Task> act = () => service.AddExerciseAsync(assignmentId, request);

            // Assert
            await act.Should().ThrowAsync<KeyNotFoundException>();
            assignment.AssignmentExercises.Should().BeEmpty();
            repoMock.Verify(r => r.SaveChangesAsync(), Times.Never);
        }

        [Fact]
        public async Task AddExerciseAsync_Should_Not_Save_When_Domain_Throws_On_Duplicate()
        {
            // Arrange
            var assignmentId = Guid.NewGuid();
            var sourceExerciseId = Guid.NewGuid();
            var assignment = new Assignment("title", "desc");
            // already added once
            assignment.AddExerciseFromSnapshot(TestData.Snapshot(sourceExerciseId));

            var repoMock = new Mock<IAssignmentRepository>();
            repoMock.Setup(r => r.GetByIdAsync(assignmentId)).ReturnsAsync(assignment);

            var providerMock = new Mock<IExerciseProvider>();
            providerMock.Setup(p => p.GetExerciseSnapshotAsync(sourceExerciseId))
                .ReturnsAsync(TestData.Snapshot(sourceExerciseId));

            var service = new AssignmentService(repoMock.Object, providerMock.Object);
            var request = new CreateAssignmentExerciseRequest { ExerciseId = sourceExerciseId };

            // Act
            Func<Task> act = () => service.AddExerciseAsync(assignmentId, request);

            // Assert
            await act.Should().ThrowAsync<InvalidOperationException>();
            repoMock.Verify(r => r.SaveChangesAsync(), Times.Never);
        }
    }

    public class AssignmentServiceTests_RemoveExerciseAsync_Tests
    {
        [Fact]
        public async Task RemoveExerciseAsync_Should_Remove_AssignmentExercise_And_Save()
        {
            // Arrange
            var assignmentId = Guid.NewGuid();
            var assignment = new Assignment("a", "b");
            var ae = assignment.AddExerciseFromSnapshot(TestData.Snapshot());

            var repoMock = new Mock<IAssignmentRepository>();
            repoMock.Setup(r => r.GetByIdAsync(assignmentId)).ReturnsAsync(assignment);
            var providerMock = new Mock<IExerciseProvider>();

            var service = new AssignmentService(repoMock.Object, providerMock.Object);
            var request = new RemoveAssignmentExerciseRequest
            {
                AssignmentExerciseId = ae.Id
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
            repoMock.Setup(r => r.GetByIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync((Assignment?)null);
            var providerMock = new Mock<IExerciseProvider>();

            var service = new AssignmentService(repoMock.Object, providerMock.Object);
            var request = new RemoveAssignmentExerciseRequest
            {
                AssignmentExerciseId = Guid.NewGuid()
            };

            // Act
            Func<Task> act = () =>
                service.RemoveExerciseAsync(Guid.NewGuid(), request);

            // Assert
            await act.Should().ThrowAsync<KeyNotFoundException>();
            repoMock.Verify(r => r.SaveChangesAsync(), Times.Never);
        }

        [Fact]
        public async Task RemoveExerciseAsync_Should_Throw_When_AssignmentExercise_Not_Found()
        {
            // Arrange
            var assignmentId = Guid.NewGuid();
            var assignment = new Assignment("a", "b");

            var repoMock = new Mock<IAssignmentRepository>();
            repoMock.Setup(r => r.GetByIdAsync(assignmentId)).ReturnsAsync(assignment);
            var providerMock = new Mock<IExerciseProvider>();

            var service = new AssignmentService(repoMock.Object, providerMock.Object);
            var request = new RemoveAssignmentExerciseRequest
            {
                AssignmentExerciseId = Guid.NewGuid()
            };

            // Act
            Func<Task> act = () => service.RemoveExerciseAsync(assignmentId, request);

            // Assert
            await act.Should().ThrowAsync<KeyNotFoundException>();
            repoMock.Verify(r => r.SaveChangesAsync(), Times.Never);
        }
    }

    public class AssignmentService_UpdateAssignmentAsync_Tests
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
            repoMock.Setup(r => r.GetByIdAsync(assignmentId)).ReturnsAsync(assignment);
            var providerMock = new Mock<IExerciseProvider>();

            var service = new AssignmentService(repoMock.Object, providerMock.Object);
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
            repoMock.Setup(r => r.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(assignment);
            var providerMock = new Mock<IExerciseProvider>();

            var service = new AssignmentService(repoMock.Object, providerMock.Object);
            var request = new UpdateAssignmentRequest
            {
                Title = title,
                Description = "desc"
            };

            // Act
            Func<Task> act = () =>
                service.UpdateAssignmentAsync(Guid.NewGuid(), request);

            // Assert
            await act.Should().ThrowAsync<ArgumentException>();
        }
    }
}