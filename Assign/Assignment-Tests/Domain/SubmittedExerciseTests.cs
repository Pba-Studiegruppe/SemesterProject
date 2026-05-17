using Assignment_Domain.Entities;
using Assignment_Domain.SnapShots;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Assignment_Tests.Domain
{
    public class SubmittedExerciseTests
    {
        private static SubmittedAssignment SubmissionFor(int questionCount, int pointsPerQuestion)
        {
            var assignment = new Assignment("a", "b");
            var ae = assignment.AddExerciseFromSnapshot(new ExerciseSnapshotInput(
                Guid.NewGuid(),
                "Ex",
                "Content",
                Enumerable.Range(0, questionCount)
                    .Select(i => new QuestionSnapshotInput(Guid.NewGuid(), $"Q{i}", "C"))
                    .ToList()));
            foreach (var q in ae.Questions)
                ae.SetQuestionPoints(q.Id, pointsPerQuestion);

            return SubmittedAssignment.CreateFromAssignment(assignment, Guid.NewGuid());
        }

        [Fact]
        public void TotalScore_Should_Sum_Scored_Questions()
        {
            // Arrange
            var submission = SubmissionFor(questionCount: 3, pointsPerQuestion: 10);
            var se = submission.SubmittedExercises.Single();
            var qs = se.SubmittedQuestions.ToList();

            // Act
            submission.ScoreQuestion(qs[0].Id, 5, null, null);
            submission.ScoreQuestion(qs[1].Id, 8, null, null);
            // qs[2] unscored

            // Assert
            se.TotalScore.Should().Be(13);
        }

        [Fact]
        public void TotalScore_Should_Be_Zero_When_Nothing_Scored()
        {
            // Arrange
            var submission = SubmissionFor(questionCount: 2, pointsPerQuestion: 5);
            var se = submission.SubmittedExercises.Single();

            // Act / Assert
            se.TotalScore.Should().Be(0);
        }

        [Fact]
        public void OverallComment_Should_Default_To_Null()
        {
            // Arrange
            var submission = SubmissionFor(questionCount: 1, pointsPerQuestion: 1);
            var se = submission.SubmittedExercises.Single();

            // Assert
            se.OverallComment.Should().BeNull();
        }
    }
}
