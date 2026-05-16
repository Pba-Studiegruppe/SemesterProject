using Assignment_Api;
using Assignment_Domain.Entities;
using Assignment_Domain.SnapShots;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

    namespace Assignment_Tests.Domain
    {
        public class AssignmentTests
        {
            private static ExerciseSnapshotInput SnapshotWith(
                Guid? sourceExerciseId = null,
                string title = "Ex",
                string content = "Content",
                int questionCount = 1,
                int pointsPerQuestion = 0)
            {
                var questions = Enumerable.Range(0, questionCount)
                    .Select(i => new QuestionSnapshotInput(Guid.NewGuid(), $"Q{i}", "C"))
                    .ToList();

                return new ExerciseSnapshotInput(
                    sourceExerciseId ?? Guid.NewGuid(),
                    title,
                    content,
                    questions);
            }

            [Fact]
            public void Constructor_Should_Set_Properties()
            {
                // Arrange & Act
                var assignment = new Assignment("Test Title", "Test Description");

                // Assert
                assignment.Title.Should().Be("Test Title");
                assignment.Description.Should().Be("Test Description");
                assignment.AssignmentExercises.Should().BeEmpty();
                assignment.SubmittedAssignments.Should().BeEmpty();
            }

            [Fact]
            public void AddExerciseFromSnapshot_Should_Add_Exercise_With_Snapshot_Data()
            {
                // Arrange
                var assignment = new Assignment("a", "b");
                var sourceId = Guid.NewGuid();
                var snapshot = SnapshotWith(sourceId, "Snapshot Title", "Snapshot Content");

                // Act
                var ae = assignment.AddExerciseFromSnapshot(snapshot);

                // Assert
                assignment.AssignmentExercises.Should().ContainSingle().Which.Should().BeSameAs(ae);
                ae.SourceExerciseId.Should().Be(sourceId);
                ae.AssignmentId.Should().Be(assignment.Id);
                ae.Title.Should().Be("Snapshot Title");
                ae.Content.Should().Be("Snapshot Content");
                ae.Order.Should().Be(0);
                ae.SnapshotTakenAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(2));
            }

            [Fact]
            public void AddExerciseFromSnapshot_Should_Copy_Questions_With_Source_Ids()
            {
                // Arrange
                var assignment = new Assignment("a", "b");
                var q1Source = Guid.NewGuid();
                var q2Source = Guid.NewGuid();
                var snapshot = new ExerciseSnapshotInput(
                    Guid.NewGuid(),
                    "Ex",
                    "C",
                    new List<QuestionSnapshotInput>
                    {
                    new(q1Source, "Q1", "C1"),
                    new(q2Source, "Q2", "C2"),
                    });

                // Act
                var ae = assignment.AddExerciseFromSnapshot(snapshot);

                // Assert
                ae.Questions.Should().HaveCount(2);
                ae.Questions.Select(q => q.SourceQuestionId)
                    .Should().ContainInOrder(q1Source, q2Source);
                ae.Questions.Select(q => q.Order)
                    .Should().ContainInOrder(0, 1);
                ae.Questions.Should().AllSatisfy(q => q.Id.Should().NotBe(Guid.Empty));
            }

            [Fact]
            public void AddExerciseFromSnapshot_Should_Throw_When_Snapshot_Is_Null()
            {
                // Arrange
                var assignment = new Assignment("a", "b");

                // Act
                Action act = () => assignment.AddExerciseFromSnapshot(null!);

                // Assert
                act.Should().Throw<ArgumentNullException>();
            }

            [Fact]
            public void AddExerciseFromSnapshot_Should_Throw_When_Source_Already_Added()
            {
                // Arrange
                var assignment = new Assignment("a", "b");
                var sourceId = Guid.NewGuid();
                assignment.AddExerciseFromSnapshot(SnapshotWith(sourceId));

                // Act
                Action act = () =>
                    assignment.AddExerciseFromSnapshot(SnapshotWith(sourceId));

                // Assert
                act.Should()
                    .Throw<InvalidOperationException>()
                    .WithMessage($"*{sourceId}*already*");
            }

            [Fact]
            public void AddExerciseFromSnapshot_Should_Increment_Order_For_Each_Exercise()
            {
                // Arrange
                var assignment = new Assignment("a", "b");

                // Act
                var first = assignment.AddExerciseFromSnapshot(SnapshotWith());
                var second = assignment.AddExerciseFromSnapshot(SnapshotWith());
                var third = assignment.AddExerciseFromSnapshot(SnapshotWith());

                // Assert
                first.Order.Should().Be(0);
                second.Order.Should().Be(1);
                third.Order.Should().Be(2);
            }

            [Fact]
            public void RemoveExercise_Should_Remove_By_AssignmentExerciseId()
            {
                // Arrange
                var assignment = new Assignment("a", "b");
                var ae = assignment.AddExerciseFromSnapshot(SnapshotWith());

                // Act
                assignment.RemoveExercise(ae.Id);

                // Assert
                assignment.AssignmentExercises.Should().BeEmpty();
            }

            [Fact]
            public void RemoveExercise_Should_Throw_When_Not_Found()
            {
                // Arrange
                var assignment = new Assignment("a", "b");
                var unknownId = Guid.NewGuid();

                // Act
                Action act = () => assignment.RemoveExercise(unknownId);

                // Assert
                act.Should()
                    .Throw<KeyNotFoundException>()
                    .WithMessage($"*{unknownId}*");
            }

            [Fact]
            public void TotalPoints_Should_Sum_Points_Across_All_Exercises()
            {
                // Arrange
                var assignment = new Assignment("a", "b");
                var ae1 = assignment.AddExerciseFromSnapshot(SnapshotWith(questionCount: 2));
                var ae2 = assignment.AddExerciseFromSnapshot(SnapshotWith(questionCount: 1));

                ae1.SetQuestionPoints(ae1.Questions[0].Id, 5);
                ae1.SetQuestionPoints(ae1.Questions[1].Id, 3);
                ae2.SetQuestionPoints(ae2.Questions[0].Id, 7);

                // Act / Assert
                assignment.TotalPoints.Should().Be(15);
            }

            [Fact]
            public void TotalPoints_Should_Be_Zero_With_No_Exercises()
            {
                // Arrange
                var assignment = new Assignment("a", "b");

                // Act / Assert
                assignment.TotalPoints.Should().Be(0);
            }

            [Fact]
            public void AddSubmittedAssignment_Should_Add_Item()
            {
                // Arrange
                var assignment = new Assignment("a", "b");
                var submitted = new SubmittedAssignment();

                // Act
                assignment.AddSubmittedAssignment(submitted);

                // Assert
                assignment.SubmittedAssignments
                    .Should().ContainSingle()
                    .Which.Should().BeSameAs(submitted);
            }

            [Fact]
            public void UpdateTitle_Should_Change_Title()
            {
                // Arrange
                var assignment = new Assignment("old", null);

                // Act
                assignment.UpdateTitle("new");

                // Assert
                assignment.Title.Should().Be("new");
            }

            [Fact]
            public void UpdateDescription_Should_Change_Description()
            {
                // Arrange
                var assignment = new Assignment(null, "old");

                // Act
                assignment.UpdateDescription("new");

                // Assert
                assignment.Description.Should().Be("new");
            }

            [Theory]
            [InlineData(false, true)]
            [InlineData(true, false)]
            [InlineData(false, false)]
            public void Set_Publish_Should_Throw_When_Requirements_Not_Met(
                bool hasCourse, bool hasAssignments)
            {
                // Arrange
                var courseId = hasCourse ? Guid.NewGuid() : (Guid?)null;
                var set = new AssignmentSet(courseId, "title", "desc");

                if (hasAssignments)
                {
                    var assignment = new Assignment("a", "b");
                    assignment.AddExerciseFromSnapshot(SnapshotWith());
                    set.AddAssignment(assignment);
                }

                // Act
                Action act = () => set.Publish();

                // Assert
                act.Should().Throw<InvalidOperationException>();
            }
        }
    }