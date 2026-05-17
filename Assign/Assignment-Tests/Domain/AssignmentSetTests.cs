using Assignment_Api;
using Assignment_Domain.Entities;
using Assignment_Domain.SnapShots;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment_Tests.Domain
{
    public class AssignmentSetTests
    {
        [Fact]
        public void AddAssignment_Should_Add_When_Valid()
        {
            // Arrange
            var set = new AssignmentSet(Guid.NewGuid(), "title", "desc");
            var assignment = new Assignment("a", "b");

            // Act
            set.AddAssignment(assignment);

            // Assert
            set.Assignments.Should().ContainSingle()
                .Which.Should().BeSameAs(assignment);
        }

        [Fact]
        public void AddAssignment_Should_Throw_When_Null()
        {
            // Arrange
            var set = new AssignmentSet(Guid.NewGuid(), "title", "desc");

            // Act
            Action act = () => set.AddAssignment(null!);

            // Assert
            act.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void AddAssignment_Should_Throw_When_Duplicate()
        {
            // Arrange
            var set = new AssignmentSet(Guid.NewGuid(), "title", "desc");
            var assignment = new Assignment("a", "b");

            set.AddAssignment(assignment);

            // Act
            Action act = () => set.AddAssignment(assignment);

            // Assert
            act.Should().Throw<InvalidOperationException>();
        }

        [Fact]
        public void RemoveAssignment_Should_Remove_When_Exists()
        {
            // Arrange
            var set = new AssignmentSet(Guid.NewGuid(), "title", "desc");
            var assignment = new Assignment("a", "b");
            set.AddAssignment(assignment);

            // Act
            set.RemoveAssignment(assignment);

            // Assert
            set.Assignments.Should().BeEmpty();
        }

        [Fact]
        public void RemoveAssignment_Should_Throw_When_Not_Found()
        {
            // Arrange
            var set = new AssignmentSet(Guid.NewGuid(), "title", "desc");
            var assignment = new Assignment("a", "b");

            // Act
            Action act = () => set.RemoveAssignment(assignment);

            // Assert
            act.Should().Throw<KeyNotFoundException>();
        }
    }

    public class AssignmentSet_PublishTests
    {
        [Fact]
        public void Publish_Should_Set_IsPublished_When_All_Assignments_Are_Valid()
        {
            // Arrange
            var set = new AssignmentSet(Guid.NewGuid(), "title", "desc");

            var assignment = new Assignment("a", "b");
            assignment.AddExerciseFromSnapshot(new ExerciseSnapshotInput(
            Guid.NewGuid(), "Ex", "C",
            new List<QuestionSnapshotInput> { new(Guid.NewGuid(), "Q", "C") }));

            set.AddAssignment(assignment);

            // Act
            Action act = () => set.Publish();

            // Assert
            act.Should().NotThrow();
            set.IsPublihsed.Should().BeTrue();
        }

        [Fact]
        public void Publish_Should_Return_False_When_No_CourseId()
        {
            // Arrange
            var set = new AssignmentSet(null, "title", "desc");
            set.AddAssignment(new Assignment("a", "b"));

            // Act
            Action act = () => set.Publish();

            // Assert
            act.Should().Throw<InvalidOperationException>()
                .WithMessage("*Cannot publish without course*");
        }

        [Fact]
        public void Publish_Should_Return_False_When_No_Assignments()
        {
            // Arrange
            var set = new AssignmentSet(Guid.NewGuid(), "title", "desc");

            // Act
            Action act = () => set.Publish();

            // Assert
            act.Should().Throw<InvalidOperationException>()
                .WithMessage("*Cannot publish without assignments*");
        }
        [Fact]
        public void Publish_Should_Return_False_When_Assignment_Has_No_Exercises()
        {
            // Arrange
            var set = new AssignmentSet(Guid.NewGuid(), "title", "desc");

            var assignment = new Assignment("a", "b");
            // No exercises added

            set.AddAssignment(assignment);

            // Act
            Action act = () => set.Publish();

            // Assert
            act.Should().Throw<InvalidOperationException>()
                .WithMessage("*All assignments must be valid*");
        }

        [Fact]
        public void Publish_Should_Return_False_When_Any_Assignment_Is_Invalid()
        {
            // Arrange
            var set = new AssignmentSet(Guid.NewGuid(), "title", "desc");

            var valid = new Assignment("valid", "desc");
            valid.AddExerciseFromSnapshot(new ExerciseSnapshotInput(Guid.NewGuid(), "Ex", "C",
                new List<QuestionSnapshotInput> { new(Guid.NewGuid(), "Q", "C") }));

            var invalid = new Assignment("invalid", "desc");
            // no exercises

            set.AddAssignment(valid);
            set.AddAssignment(invalid);

            // Act
            Action act = () => set.Publish();

            // Assert
            act.Should().Throw<InvalidOperationException>()
                .WithMessage("*All assignments must be valid*");
        }
    }

    public class AssignmentSet_GradeSheetTests
    {
        [Fact]
        public void AddGradeSheet_Should_Throw_When_No_Assignments()
        {
            // Arrange
            var set = new AssignmentSet(Guid.NewGuid(), "title", "desc");

            // Act
            Action act = () => set.AddGradeSheet();

            // Assert
            act.Should().Throw<InvalidOperationException>();
        }
    }

    public class AssignmentSet_PublishGradesTests
    {
        [Fact]
        public void PublishGrades_Should_Throw_When_No_Gradesheets()
        {
            // Arrange
            var set = new AssignmentSet(Guid.NewGuid(), "title", "desc");

            // Act
            Action act = () => set.PublishGrades();

            // Assert
            act.Should().Throw<InvalidOperationException>();
        }

        [Fact]
        public void PublishGrades_Should_Throw_When_Assignments_Have_No_Submissions()
        {
            // Arrange
            var set = new AssignmentSet(Guid.NewGuid(), "title", "desc");
            var assignment = new Assignment("a", "b");

            set.AddAssignment(assignment);
            set.AddGradeSheet();

            // Act
            Action act = () => set.PublishGrades();

            // Assert
            act.Should().Throw<InvalidOperationException>();
        }
    }
}
