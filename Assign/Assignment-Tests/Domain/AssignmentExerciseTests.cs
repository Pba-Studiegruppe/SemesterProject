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
        public class AssignmentExerciseTests
        {
            private static AssignmentExercise CreateExerciseInAssignment(int questionCount = 1)
            {
                var assignment = new Assignment("a", "b");
                var snapshot = new ExerciseSnapshotInput(
                    Guid.NewGuid(),
                    "Ex Title",
                    "Ex Content",
                    Enumerable.Range(0, questionCount)
                        .Select(i => new QuestionSnapshotInput(Guid.NewGuid(), $"Q{i}", $"Content {i}"))
                        .ToList());

                return assignment.AddExerciseFromSnapshot(snapshot);
            }

            [Fact]
            public void AssignmentExercise_Should_Have_Snapshot_Fields_Set()
            {
                // Arrange / Act
                var ae = CreateExerciseInAssignment(questionCount: 2);

                // Assert
                ae.Id.Should().NotBe(Guid.Empty);
                ae.AssignmentId.Should().NotBe(Guid.Empty);
                ae.SourceExerciseId.Should().NotBe(Guid.Empty);
                ae.Title.Should().Be("Ex Title");
                ae.Content.Should().Be("Ex Content");
                ae.Questions.Should().HaveCount(2);
                ae.SnapshotTakenAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(2));
            }

            [Fact]
            public void Questions_Should_Have_Their_Own_Ids_Distinct_From_Source()
            {
                // Arrange / Act
                var ae = CreateExerciseInAssignment(questionCount: 1);
                var q = ae.Questions[0];

                // Assert
                q.Id.Should().NotBe(Guid.Empty);
                q.SourceQuestionId.Should().NotBe(Guid.Empty);
                q.Id.Should().NotBe(q.SourceQuestionId);
                q.AssignmentExerciseId.Should().Be(ae.Id);
            }

            [Fact]
            public void TotalPoints_Should_Sum_Question_Points()
            {
                // Arrange
                var ae = CreateExerciseInAssignment(questionCount: 3);
                ae.SetQuestionPoints(ae.Questions[0].Id, 4);
                ae.SetQuestionPoints(ae.Questions[1].Id, 6);
                ae.SetQuestionPoints(ae.Questions[2].Id, 0);

                // Act / Assert
                ae.TotalPoints.Should().Be(10);
            }

            [Fact]
            public void TotalPoints_Should_Default_To_Zero()
            {
                // Arrange / Act
                var ae = CreateExerciseInAssignment(questionCount: 2);

                // Assert
                ae.TotalPoints.Should().Be(0);
            }

            [Fact]
            public void RemoveQuestion_Should_Remove_Existing_Question()
            {
                // Arrange
                var ae = CreateExerciseInAssignment(questionCount: 2);
                var toRemove = ae.Questions[0].Id;
                var keep = ae.Questions[1].Id;

                // Act
                ae.RemoveQuestion(toRemove);

                // Assert
                ae.Questions.Should().ContainSingle().Which.Id.Should().Be(keep);
            }

            [Fact]
            public void RemoveQuestion_Should_Throw_When_Not_Found()
            {
                // Arrange
                var ae = CreateExerciseInAssignment();
                var unknownId = Guid.NewGuid();

                // Act
                Action act = () => ae.RemoveQuestion(unknownId);

                // Assert
                act.Should()
                    .Throw<KeyNotFoundException>()
                    .WithMessage($"*{unknownId}*");
            }

            [Fact]
            public void SetQuestionPoints_Should_Update_Points_When_Question_Exists()
            {
                // Arrange
                var ae = CreateExerciseInAssignment();
                var qId = ae.Questions[0].Id;

                // Act
                ae.SetQuestionPoints(qId, 12);

                // Assert
                ae.Questions[0].Points.Should().Be(12);
                ae.TotalPoints.Should().Be(12);
            }

            [Fact]
            public void SetQuestionPoints_Should_Throw_When_Question_Not_Found()
            {
                // Arrange
                var ae = CreateExerciseInAssignment();
                var unknownId = Guid.NewGuid();

                // Act
                Action act = () => ae.SetQuestionPoints(unknownId, 5);

                // Assert
                act.Should().Throw<KeyNotFoundException>();
            }

            [Fact]
            public void SetQuestionPoints_Should_Throw_When_Points_Negative()
            {
                // Arrange
                var ae = CreateExerciseInAssignment();
                var qId = ae.Questions[0].Id;

                // Act
                Action act = () => ae.SetQuestionPoints(qId, -1);

                // Assert
                act.Should().Throw<ArgumentOutOfRangeException>();
            }
        }
    }