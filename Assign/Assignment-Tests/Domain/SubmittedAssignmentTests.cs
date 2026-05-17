using Assignment_Domain.Entities;
using Assignment_Domain.Shared;
using Assignment_Domain.SnapShots;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Assignment_Tests.Domain
{
    public class SubmittedAssignmentTests
    {
        private static Assignment AssignmentWith(
            int exerciseCount = 1,
            int questionsPerExercise = 1,
            int pointsPerQuestion = 5)
        {
            var assignment = new Assignment("Test", "Description");

            for (var e = 0; e < exerciseCount; e++)
            {
                var snapshot = new ExerciseSnapshotInput(
                    Guid.NewGuid(),
                    $"Ex{e}",
                    $"Content{e}",
                    Enumerable.Range(0, questionsPerExercise)
                        .Select(q => new QuestionSnapshotInput(Guid.NewGuid(), $"Q{e}.{q}", "C"))
                        .ToList());

                var ae = assignment.AddExerciseFromSnapshot(snapshot);
                foreach (var q in ae.Questions)
                    ae.SetQuestionPoints(q.Id, pointsPerQuestion);
            }

            return assignment;
        }

        // ── Construction / snapshotting ──────────────────────────────────────

        [Fact]
        public void CreateFromAssignment_Should_Set_Default_Properties()
        {
            // Arrange
            var assignment = AssignmentWith();
            var studentId = Guid.NewGuid();

            // Act
            var submission = SubmittedAssignment.CreateFromAssignment(assignment, studentId);

            // Assert
            submission.Id.Should().NotBe(Guid.Empty);
            submission.AssignmentId.Should().Be(assignment.Id);
            submission.StudentId.Should().Be(studentId);
            submission.Status.Should().Be(EvaluationStatus.Pending);
            submission.EvaluatorId.Should().BeNull();
            submission.TotalScore.Should().Be(0);
        }

        [Fact]
        public void CreateFromAssignment_Should_Snapshot_Every_Exercise_And_Question()
        {
            // Arrange
            var assignment = AssignmentWith(
                exerciseCount: 2,
                questionsPerExercise: 3,
                pointsPerQuestion: 4);

            // Act
            var submission = SubmittedAssignment.CreateFromAssignment(assignment, Guid.NewGuid());

            // Assert
            submission.SubmittedExercises.Should().HaveCount(2);
            submission.SubmittedExercises
                .SelectMany(se => se.SubmittedQuestions)
                .Should().HaveCount(6);
            submission.SubmittedExercises
                .SelectMany(se => se.SubmittedQuestions)
                .Should().AllSatisfy(q =>
                {
                    q.MaxPoints.Should().Be(4);
                    q.IsScored.Should().BeFalse();
                    q.PointsAwarded.Should().BeNull();
                });
        }

        [Fact]
        public void CreateFromAssignment_Should_Link_Submitted_Items_Back_To_Their_Source()
        {
            // Arrange
            var assignment = AssignmentWith(exerciseCount: 1, questionsPerExercise: 2);
            var srcExercise = assignment.AssignmentExercises.Single();
            var srcQuestionIds = srcExercise.Questions.Select(q => q.Id).ToHashSet();

            // Act
            var submission = SubmittedAssignment.CreateFromAssignment(assignment, Guid.NewGuid());

            // Assert
            var se = submission.SubmittedExercises.Single();
            se.AssignmentExerciseId.Should().Be(srcExercise.Id);
            se.SubmittedQuestions
                .Select(q => q.AssignmentQuestionId)
                .Should().BeEquivalentTo(srcQuestionIds);
        }

        [Fact]
        public void CreateFromAssignment_Should_Throw_When_Assignment_Null()
        {
            // Act
            Action act = () => SubmittedAssignment.CreateFromAssignment(null!, Guid.NewGuid());

            // Assert
            act.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void CreateFromAssignment_Should_Throw_When_StudentId_Empty()
        {
            // Arrange
            var assignment = AssignmentWith();

            // Act
            Action act = () => SubmittedAssignment.CreateFromAssignment(assignment, Guid.Empty);

            // Assert
            act.Should().Throw<ArgumentException>();
        }

        // ── Scoring a question ───────────────────────────────────────────────

        [Fact]
        public void ScoreQuestion_Should_Set_Points_Comment_And_ErrorType()
        {
            // Arrange
            var submission = SubmittedAssignment.CreateFromAssignment(
                AssignmentWith(pointsPerQuestion: 10), Guid.NewGuid());
            var sq = submission.SubmittedExercises.First().SubmittedQuestions.First();
            var errorTypeId = Guid.NewGuid();

            // Act
            submission.ScoreQuestion(sq.Id, 7, "Good effort, minor slip", errorTypeId);

            // Assert
            sq.PointsAwarded.Should().Be(7);
            sq.Comment.Should().Be("Good effort, minor slip");
            sq.ErrorTypeId.Should().Be(errorTypeId);
            sq.IsScored.Should().BeTrue();
        }

        [Fact]
        public void ScoreQuestion_Should_Allow_Null_Comment_And_ErrorType()
        {
            // Arrange
            var submission = SubmittedAssignment.CreateFromAssignment(
                AssignmentWith(), Guid.NewGuid());
            var sq = submission.SubmittedExercises.First().SubmittedQuestions.First();

            // Act
            submission.ScoreQuestion(sq.Id, 3, null, null);

            // Assert
            sq.PointsAwarded.Should().Be(3);
            sq.Comment.Should().BeNull();
            sq.ErrorTypeId.Should().BeNull();
        }

        [Fact]
        public void ScoreQuestion_Should_Transition_Status_From_Pending_To_InProgress()
        {
            // Arrange
            var submission = SubmittedAssignment.CreateFromAssignment(
                AssignmentWith(), Guid.NewGuid());
            var sq = submission.SubmittedExercises.First().SubmittedQuestions.First();

            // Act
            submission.ScoreQuestion(sq.Id, 3, null, null);

            // Assert
            submission.Status.Should().Be(EvaluationStatus.InProgress);
        }

        [Fact]
        public void ScoreQuestion_Should_Throw_When_Question_Not_In_Submission()
        {
            // Arrange
            var submission = SubmittedAssignment.CreateFromAssignment(
                AssignmentWith(), Guid.NewGuid());

            // Act
            Action act = () => submission.ScoreQuestion(Guid.NewGuid(), 1, null, null);

            // Assert
            act.Should().Throw<KeyNotFoundException>();
        }

        [Fact]
        public void ScoreQuestion_Should_Throw_When_Points_Exceed_MaxPoints()
        {
            // Arrange
            var submission = SubmittedAssignment.CreateFromAssignment(
                AssignmentWith(pointsPerQuestion: 5), Guid.NewGuid());
            var sq = submission.SubmittedExercises.First().SubmittedQuestions.First();

            // Act
            Action act = () => submission.ScoreQuestion(sq.Id, 6, null, null);

            // Assert
            act.Should().Throw<ArgumentOutOfRangeException>();
        }

        [Fact]
        public void ScoreQuestion_Should_Throw_When_Points_Negative()
        {
            // Arrange
            var submission = SubmittedAssignment.CreateFromAssignment(
                AssignmentWith(), Guid.NewGuid());
            var sq = submission.SubmittedExercises.First().SubmittedQuestions.First();

            // Act
            Action act = () => submission.ScoreQuestion(sq.Id, -1, null, null);

            // Assert
            act.Should().Throw<ArgumentOutOfRangeException>();
        }

        [Fact]
        public void ScoreQuestion_Should_Allow_Re_Scoring_While_Not_Returned()
        {
            // Arrange
            var submission = SubmittedAssignment.CreateFromAssignment(
                AssignmentWith(pointsPerQuestion: 10), Guid.NewGuid());
            var sq = submission.SubmittedExercises.First().SubmittedQuestions.First();
            submission.ScoreQuestion(sq.Id, 5, "first try", null);

            // Act
            submission.ScoreQuestion(sq.Id, 8, "on reflection", null);

            // Assert
            sq.PointsAwarded.Should().Be(8);
            sq.Comment.Should().Be("on reflection");
        }

        // ── Exercise-level comment ───────────────────────────────────────────

        [Fact]
        public void SetExerciseComment_Should_Set_Comment_On_Correct_Exercise()
        {
            // Arrange
            var submission = SubmittedAssignment.CreateFromAssignment(
                AssignmentWith(exerciseCount: 2), Guid.NewGuid());
            var target = submission.SubmittedExercises.Last();

            // Act
            submission.SetExerciseComment(target.Id, "Nice work overall");

            // Assert
            target.OverallComment.Should().Be("Nice work overall");
            submission.SubmittedExercises.First().OverallComment.Should().BeNull();
        }

        [Fact]
        public void SetExerciseComment_Should_Throw_When_Exercise_Not_Found()
        {
            // Arrange
            var submission = SubmittedAssignment.CreateFromAssignment(
                AssignmentWith(), Guid.NewGuid());

            // Act
            Action act = () => submission.SetExerciseComment(Guid.NewGuid(), "x");

            // Assert
            act.Should().Throw<KeyNotFoundException>();
        }

        [Fact]
        public void SetExerciseComment_Should_Transition_Status_From_Pending_To_InProgress()
        {
            // Arrange
            var submission = SubmittedAssignment.CreateFromAssignment(
                AssignmentWith(), Guid.NewGuid());
            var se = submission.SubmittedExercises.First();

            // Act
            submission.SetExerciseComment(se.Id, "Some overall thoughts");

            // Assert
            submission.Status.Should().Be(EvaluationStatus.InProgress);
        }

        // ── Total score ──────────────────────────────────────────────────────

        [Fact]
        public void TotalScore_Should_Sum_All_Scored_Questions()
        {
            // Arrange
            var submission = SubmittedAssignment.CreateFromAssignment(
                AssignmentWith(exerciseCount: 2, questionsPerExercise: 2, pointsPerQuestion: 10),
                Guid.NewGuid());
            var all = submission.SubmittedExercises
                .SelectMany(se => se.SubmittedQuestions).ToList();

            // Act
            submission.ScoreQuestion(all[0].Id, 8, null, null);
            submission.ScoreQuestion(all[1].Id, 5, null, null);
            submission.ScoreQuestion(all[2].Id, 10, null, null);
            // all[3] remains unscored

            // Assert
            submission.TotalScore.Should().Be(23);
        }

        // ── MarkEvaluated / Return ───────────────────────────────────────────

        [Fact]
        public void MarkEvaluated_Should_Set_Status_And_Evaluator_When_All_Scored()
        {
            // Arrange
            var submission = SubmittedAssignment.CreateFromAssignment(
                AssignmentWith(questionsPerExercise: 2), Guid.NewGuid());
            var all = submission.SubmittedExercises
                .SelectMany(se => se.SubmittedQuestions).ToList();
            submission.ScoreQuestion(all[0].Id, 3, null, null);
            submission.ScoreQuestion(all[1].Id, 4, null, null);
            var evaluatorId = Guid.NewGuid();

            // Act
            submission.MarkEvaluated(evaluatorId);

            // Assert
            submission.Status.Should().Be(EvaluationStatus.Completed);
            submission.EvaluatorId.Should().Be(evaluatorId);
        }

        [Fact]
        public void MarkEvaluated_Should_Throw_When_Any_Question_Unscored()
        {
            // Arrange
            var submission = SubmittedAssignment.CreateFromAssignment(
                AssignmentWith(questionsPerExercise: 2), Guid.NewGuid());
            var firstQ = submission.SubmittedExercises.First().SubmittedQuestions.First();
            submission.ScoreQuestion(firstQ.Id, 1, null, null);

            // Act
            Action act = () => submission.MarkEvaluated(Guid.NewGuid());

            // Assert
            act.Should().Throw<InvalidOperationException>();
        }

        [Fact]
        public void MarkEvaluated_Should_Throw_When_EvaluatorId_Empty()
        {
            // Arrange
            var submission = SubmittedAssignment.CreateFromAssignment(
                AssignmentWith(), Guid.NewGuid());
            var q = submission.SubmittedExercises.First().SubmittedQuestions.First();
            submission.ScoreQuestion(q.Id, 1, null, null);

            // Act
            Action act = () => submission.MarkEvaluated(Guid.Empty);

            // Assert
            act.Should().Throw<ArgumentException>();
        }

        [Fact]
        public void MarkEvaluated_Should_Throw_When_Already_Completed()
        {
            // Arrange
            var submission = SubmittedAssignment.CreateFromAssignment(
                AssignmentWith(), Guid.NewGuid());
            var q = submission.SubmittedExercises.First().SubmittedQuestions.First();
            submission.ScoreQuestion(q.Id, 1, null, null);
            submission.MarkEvaluated(Guid.NewGuid());

            // Act
            Action act = () => submission.MarkEvaluated(Guid.NewGuid());

            // Assert
            act.Should().Throw<InvalidOperationException>();
        }

        [Fact]
        public void Return_Should_Set_Status_To_Returned_When_Completed()
        {
            // Arrange
            var submission = SubmittedAssignment.CreateFromAssignment(
                AssignmentWith(), Guid.NewGuid());
            var q = submission.SubmittedExercises.First().SubmittedQuestions.First();
            submission.ScoreQuestion(q.Id, 1, null, null);
            submission.MarkEvaluated(Guid.NewGuid());

            // Act
            submission.Return();

            // Assert
            submission.Status.Should().Be(EvaluationStatus.Returned);
        }

        [Fact]
        public void Return_Should_Throw_When_Not_Completed()
        {
            // Arrange
            var submission = SubmittedAssignment.CreateFromAssignment(
                AssignmentWith(), Guid.NewGuid());

            // Act
            Action act = () => submission.Return();

            // Assert
            act.Should().Throw<InvalidOperationException>();
        }

        [Fact]
        public void ScoreQuestion_Should_Throw_When_Submission_Returned()
        {
            // Arrange
            var submission = SubmittedAssignment.CreateFromAssignment(
                AssignmentWith(), Guid.NewGuid());
            var q = submission.SubmittedExercises.First().SubmittedQuestions.First();
            submission.ScoreQuestion(q.Id, 1, null, null);
            submission.MarkEvaluated(Guid.NewGuid());
            submission.Return();

            // Act
            Action act = () => submission.ScoreQuestion(q.Id, 2, null, null);

            // Assert
            act.Should().Throw<InvalidOperationException>();
        }

        [Fact]
        public void SetExerciseComment_Should_Throw_When_Submission_Returned()
        {
            // Arrange
            var submission = SubmittedAssignment.CreateFromAssignment(
                AssignmentWith(), Guid.NewGuid());
            var q = submission.SubmittedExercises.First().SubmittedQuestions.First();
            var se = submission.SubmittedExercises.First();
            submission.ScoreQuestion(q.Id, 1, null, null);
            submission.MarkEvaluated(Guid.NewGuid());
            submission.Return();

            // Act
            Action act = () => submission.SetExerciseComment(se.Id, "late thought");

            // Assert
            act.Should().Throw<InvalidOperationException>();
        }
    }
}
