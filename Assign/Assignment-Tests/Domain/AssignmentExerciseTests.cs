using Assignment_Api;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment_Tests.Domain
{
    public class AssignmentExerciseTests
    {
        [Fact]
        public void Constructor_Should_Set_Properties()
        {
            // Arrange
            var assignmentId = Guid.NewGuid();
            var exerciseId = Guid.NewGuid();

            // Act
            var ae = new AssignmentExercise(assignmentId, exerciseId);

            // Assert
            ae.AssignmentId.Should().Be(assignmentId);
            ae.ExerciseId.Should().Be(exerciseId);
        }
    }
}
