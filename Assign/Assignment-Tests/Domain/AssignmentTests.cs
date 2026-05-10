using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assignment_Api;
using FluentAssertions;
using Xunit;

namespace Assignment_Tests.Domain
{
    public class AssignmentTests
    {
        [Fact]
        public void Constructor_Should_Set_Properties()
        {
            // Arrange
            var setId = Guid.NewGuid();
            var title = "Test Title";
            var description = "Test Description";

            //act
            var assignment = new Assignment(title, description);

            //Assert
            assignment.AssignmentSetId.Should().Be(setId);
            assignment.Title.Should().Be(title);
            assignment.Description.Should().Be(description);
            assignment.AssignmentExercises.Should().BeEmpty();
            assignment.SubmittedAssignments.Should().BeEmpty();
        }

        [Fact]
        public void AddExercise_Should_Add_Exercise()
        {
            // Arrange
            var assignment = new Assignment(null, null);
            var exerciseId = Guid.NewGuid();

            // Act
            assignment.AddExercise(exerciseId);

            // Assert
            assignment.AssignmentExercises.Should().ContainSingle();
            assignment.AssignmentExercises.First().ExerciseId.Should().Be(exerciseId);
        }


        [Fact]
        public void AddExercise_Should_Throw_When_Duplicate()
        {
            // Arrange
            var assignment = new Assignment(null, null);
            var exerciseId = Guid.NewGuid();

            // Act
            assignment.AddExercise(exerciseId);
            Action act = () => assignment.AddExercise(exerciseId);

            // Assert
            act.Should()
                .Throw<InvalidOperationException>()
                .WithMessage($"*{exerciseId}*already exists*");
        }

        [Fact]
        public void RemoveExercise_Should_Remove_Existing_Exercise()
        {

            // Arrange
            var assignment = new Assignment(null, null);
            var exerciseId = Guid.NewGuid();

            // Act
            assignment.AddExercise(exerciseId);
            assignment.RemoveExercise(exerciseId);

            // Assert
            assignment.AssignmentExercises.Should().BeEmpty();
        }

        [Fact]
        public void RemoveExercise_Should_Throw_When_Not_Found()
        {
            // Arrange
            var assignment = new Assignment(null, null);
            var exerciseId = Guid.NewGuid();

            // Act
            Action act = () => assignment.RemoveExercise(exerciseId);

            // Assert
            act.Should()
                .Throw<KeyNotFoundException>()
                .WithMessage($"*{exerciseId}*not found*");
        }

        [Fact]
        public void AddSubmittedAssignment_Should_Add_Item()
        {
            // Arrange
            var assignment = new Assignment(null, null);
            var submitted = new SubmittedAssignment();

            // Act
            assignment.AddSubmittedAssignment(submitted);

            // Assert
            assignment.SubmittedAssignments.Should().ContainSingle()
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
        public void Publish_Should_Throw_When_Requirements_Not_Met(bool hasCourse, bool hasAssignments)
        {
            // Arrange
            var courseId = hasCourse
                ? Guid.NewGuid()
                : (Guid?)null;

            var set = new AssignmentSet(courseId, "title", "desc");

            if (hasAssignments)
            {
                var assignment = new Assignment("a", "b");
                assignment.AddExercise(Guid.NewGuid());

                set.AddAssignment(assignment);
            }

            // Act
            Action act = () => set.Publish();

            // Assert
            act.Should().Throw<InvalidOperationException>();
        }
    }
}
