using Assignment_Application.DTO;
using Assignment_Application.Implementations;
using Assignment_Application.Interfaces.Repositories;
using Assignment_Domain.Entities;
using Assignment_Domain.Shared;
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
    internal static class SubmissionTestData
    {
        public static Assignment AssignmentWith(
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
                        .Select(q => new QuestionSnapshotInput(
                            Guid.NewGuid(), $"Q{e}.{q}", $"QContent{e}.{q}"))
                        .ToList());

                var ae = assignment.AddExerciseFromSnapshot(snapshot);
                foreach (var q in ae.Questions)
                    ae.SetQuestionPoints(q.Id, pointsPerQuestion);
            }

            return assignment;
        }

        public static SubmittedAssignment SubmissionWith(
            Assignment? assignment = null,
            Guid? studentId = null)
        {
            var a = assignment ?? AssignmentWith();
            var submission = SubmittedAssignment.CreateFromAssignment(
                a, studentId ?? Guid.NewGuid());
            submission.Assignment = a; // simulate eager-loading for the test
            return submission;
        }
    }

    // ─────────────────────────────────────────────────────────────────────────
    // GetSubmittedAssignmentAsync
    // ─────────────────────────────────────────────────────────────────────────

    public class SubmittedAssignmentServiceTests_GetSubmittedAssignment_Tests
    {
        [Fact]
        public async Task Should_Return_Full_DTO_With_Denormalized_Source_Fields()
        {
            // Arrange
            var assignment = SubmissionTestData.AssignmentWith(
                exerciseCount: 1, questionsPerExercise: 2, pointsPerQuestion: 5);
            var submission = SubmissionTestData.SubmissionWith(assignment);

            var subRepo = new Mock<ISubmittedAssignmentRepository>();
            subRepo.Setup(r => r.GetByIdAsync(submission.Id)).ReturnsAsync(submission);
            var assignmentRepo = new Mock<IAssignmentRepository>();
            var service = new SubmittedAssignmentService(subRepo.Object, assignmentRepo.Object);

            // Act
            var dto = await service.GetSubmittedAssignmentAsync(submission.Id);

            // Assert
            dto.Id.Should().Be(submission.Id);
            dto.AssignmentId.Should().Be(assignment.Id);
            dto.Status.Should().Be(EvaluationStatus.Pending);
            dto.MaxPossibleScore.Should().Be(10); // 1 ex * 2 q * 5 pts
            dto.SubmittedExercises.Should().HaveCount(1);

            var se = dto.SubmittedExercises[0];
            var sourceExercise = assignment.AssignmentExercises.Single();
            se.SourceTitle.Should().Be(sourceExercise.Title);
            se.SourceContent.Should().Be(sourceExercise.Content);
            se.SourceOrder.Should().Be(sourceExercise.Order);
            se.SubmittedQuestions.Should().HaveCount(2);

            var firstQ = se.SubmittedQuestions[0];
            var sourceQ = sourceExercise.Questions
                .Single(q => q.Id == firstQ.AssignmentQuestionId);
            firstQ.SourceTitle.Should().Be(sourceQ.Title);
            firstQ.SourceContent.Should().Be(sourceQ.Content);
            firstQ.MaxPoints.Should().Be(5);
            firstQ.IsScored.Should().BeFalse();
        }

        [Fact]
        public async Task Should_Throw_When_Submission_Not_Found()
        {
            // Arrange
            var subRepo = new Mock<ISubmittedAssignmentRepository>();
            subRepo.Setup(r => r.GetByIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync((SubmittedAssignment?)null);
            var assignmentRepo = new Mock<IAssignmentRepository>();
            var service = new SubmittedAssignmentService(subRepo.Object, assignmentRepo.Object);

            // Act
            Func<Task> act = () => service.GetSubmittedAssignmentAsync(Guid.NewGuid());

            // Assert
            await act.Should().ThrowAsync<KeyNotFoundException>();
        }
    }

    // ─────────────────────────────────────────────────────────────────────────
    // GetSubmittedAssignmentsByAssignmentAsync
    // ─────────────────────────────────────────────────────────────────────────

    public class SubmittedAssignmentServiceTests_GetByAssignment_Tests
    {
        [Fact]
        public async Task Should_Return_All_Submissions_With_Source_Fields_Populated()
        {
            // Arrange
            var assignment = SubmissionTestData.AssignmentWith(questionsPerExercise: 1, pointsPerQuestion: 7);
            var s1 = SubmittedAssignment.CreateFromAssignment(assignment, Guid.NewGuid());
            var s2 = SubmittedAssignment.CreateFromAssignment(assignment, Guid.NewGuid());

            var assignmentRepo = new Mock<IAssignmentRepository>();
            assignmentRepo.Setup(r => r.GetByIdAsync(assignment.Id)).ReturnsAsync(assignment);

            var subRepo = new Mock<ISubmittedAssignmentRepository>();
            subRepo.Setup(r => r.GetByAssignmentIdAsync(assignment.Id))
                .ReturnsAsync(new[] { s1, s2 });

            var service = new SubmittedAssignmentService(subRepo.Object, assignmentRepo.Object);

            // Act
            var dtos = (await service.GetSubmittedAssignmentsByAssignmentAsync(assignment.Id)).ToList();

            // Assert
            dtos.Should().HaveCount(2);
            dtos.Should().AllSatisfy(d =>
            {
                d.MaxPossibleScore.Should().Be(7);
                d.SubmittedExercises.Single().SourceTitle.Should().NotBeEmpty();
            });
        }

        [Fact]
        public async Task Should_Throw_When_Assignment_Not_Found()
        {
            // Arrange
            var assignmentRepo = new Mock<IAssignmentRepository>();
            assignmentRepo.Setup(r => r.GetByIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync((Assignment?)null);
            var subRepo = new Mock<ISubmittedAssignmentRepository>();
            var service = new SubmittedAssignmentService(subRepo.Object, assignmentRepo.Object);

            // Act
            Func<Task> act = () => service.GetSubmittedAssignmentsByAssignmentAsync(Guid.NewGuid());

            // Assert
            await act.Should().ThrowAsync<KeyNotFoundException>();
        }

        [Fact]
        public async Task Should_Return_Empty_When_No_Submissions()
        {
            // Arrange
            var assignment = SubmissionTestData.AssignmentWith();
            var assignmentRepo = new Mock<IAssignmentRepository>();
            assignmentRepo.Setup(r => r.GetByIdAsync(assignment.Id)).ReturnsAsync(assignment);
            var subRepo = new Mock<ISubmittedAssignmentRepository>();
            subRepo.Setup(r => r.GetByAssignmentIdAsync(assignment.Id))
                .ReturnsAsync(Array.Empty<SubmittedAssignment>());
            var service = new SubmittedAssignmentService(subRepo.Object, assignmentRepo.Object);

            // Act
            var dtos = await service.GetSubmittedAssignmentsByAssignmentAsync(assignment.Id);

            // Assert
            dtos.Should().BeEmpty();
        }
    }

    // ─────────────────────────────────────────────────────────────────────────
    // CreateSubmittedAssignmentAsync
    // ─────────────────────────────────────────────────────────────────────────

    public class SubmittedAssignmentServiceTests_Create_Tests
    {
        [Fact]
        public async Task Should_Snapshot_From_Assignment_And_Persist()
        {
            // Arrange
            var assignment = SubmissionTestData.AssignmentWith(
                exerciseCount: 2, questionsPerExercise: 3, pointsPerQuestion: 4);
            var studentId = Guid.NewGuid();

            var assignmentRepo = new Mock<IAssignmentRepository>();
            assignmentRepo.Setup(r => r.GetByIdAsync(assignment.Id)).ReturnsAsync(assignment);

            var subRepo = new Mock<ISubmittedAssignmentRepository>();
            subRepo.Setup(r => r.CreateAsync(It.IsAny<SubmittedAssignment>()))
                .Returns(Task.CompletedTask);
            subRepo.Setup(r => r.SaveChangesAsync()).Returns(Task.CompletedTask);

            var service = new SubmittedAssignmentService(subRepo.Object, assignmentRepo.Object);
            var request = new CreateSubmittedAssignmentRequest
            {
                AssignmentId = assignment.Id,
                StudentId = studentId
            };

            // Act
            var dto = await service.CreateSubmittedAssignmentAsync(request);

            // Assert
            dto.Id.Should().NotBe(Guid.Empty);
            dto.AssignmentId.Should().Be(assignment.Id);
            dto.StudentId.Should().Be(studentId);
            dto.Status.Should().Be(EvaluationStatus.Pending);
            dto.SubmittedExercises.Should().HaveCount(2);
            dto.SubmittedExercises.SelectMany(se => se.SubmittedQuestions)
                .Should().HaveCount(6);
            dto.MaxPossibleScore.Should().Be(24); // 2 * 3 * 4

            subRepo.Verify(r => r.CreateAsync(It.IsAny<SubmittedAssignment>()), Times.Once);
            subRepo.Verify(r => r.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task Should_Throw_When_Request_Null()
        {
            // Arrange
            var service = new SubmittedAssignmentService(
                new Mock<ISubmittedAssignmentRepository>().Object,
                new Mock<IAssignmentRepository>().Object);

            // Act
            Func<Task> act = () => service.CreateSubmittedAssignmentAsync(null!);

            // Assert
            await act.Should().ThrowAsync<ArgumentNullException>();
        }

        [Fact]
        public async Task Should_Throw_When_Assignment_Not_Found()
        {
            // Arrange
            var assignmentRepo = new Mock<IAssignmentRepository>();
            assignmentRepo.Setup(r => r.GetByIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync((Assignment?)null);
            var service = new SubmittedAssignmentService(
                new Mock<ISubmittedAssignmentRepository>().Object, assignmentRepo.Object);

            var request = new CreateSubmittedAssignmentRequest
            {
                AssignmentId = Guid.NewGuid(),
                StudentId = Guid.NewGuid()
            };

            // Act
            Func<Task> act = () => service.CreateSubmittedAssignmentAsync(request);

            // Assert
            await act.Should().ThrowAsync<KeyNotFoundException>();
        }

        [Fact]
        public async Task Should_Propagate_Domain_Exception_When_StudentId_Empty()
        {
            // Arrange
            var assignment = SubmissionTestData.AssignmentWith();
            var assignmentRepo = new Mock<IAssignmentRepository>();
            assignmentRepo.Setup(r => r.GetByIdAsync(assignment.Id)).ReturnsAsync(assignment);
            var service = new SubmittedAssignmentService(
                new Mock<ISubmittedAssignmentRepository>().Object, assignmentRepo.Object);

            var request = new CreateSubmittedAssignmentRequest
            {
                AssignmentId = assignment.Id,
                StudentId = Guid.Empty
            };

            // Act
            Func<Task> act = () => service.CreateSubmittedAssignmentAsync(request);

            // Assert
            await act.Should().ThrowAsync<ArgumentException>();
        }
    }

    // ─────────────────────────────────────────────────────────────────────────
    // ScoreQuestionAsync
    // ─────────────────────────────────────────────────────────────────────────

    public class SubmittedAssignmentServiceTests_ScoreQuestion_Tests
    {
        [Fact]
        public async Task Should_Score_Question_And_Return_Updated_DTO()
        {
            // Arrange
            var assignment = SubmissionTestData.AssignmentWith(pointsPerQuestion: 10);
            var submission = SubmissionTestData.SubmissionWith(assignment);
            var sq = submission.SubmittedExercises.First().SubmittedQuestions.First();
            var errorTypeId = Guid.NewGuid();

            var subRepo = new Mock<ISubmittedAssignmentRepository>();
            subRepo.Setup(r => r.GetByIdAsync(submission.Id)).ReturnsAsync(submission);
            subRepo.Setup(r => r.SaveChangesAsync()).Returns(Task.CompletedTask);

            var service = new SubmittedAssignmentService(
                subRepo.Object, new Mock<IAssignmentRepository>().Object);

            var request = new ScoreQuestionRequest
            {
                Points = 8,
                Comment = "Almost there",
                ErrorTypeId = errorTypeId
            };

            // Act
            var dto = await service.ScoreQuestionAsync(submission.Id, sq.Id, request);

            // Assert
            dto.Id.Should().Be(sq.Id);
            dto.PointsAwarded.Should().Be(8);
            dto.Comment.Should().Be("Almost there");
            dto.ErrorTypeId.Should().Be(errorTypeId);
            dto.IsScored.Should().BeTrue();
            dto.SourceTitle.Should().NotBeEmpty();

            subRepo.Verify(r => r.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task Should_Throw_When_Request_Null()
        {
            var service = new SubmittedAssignmentService(
                new Mock<ISubmittedAssignmentRepository>().Object,
                new Mock<IAssignmentRepository>().Object);

            Func<Task> act = () => service.ScoreQuestionAsync(Guid.NewGuid(), Guid.NewGuid(), null!);

            await act.Should().ThrowAsync<ArgumentNullException>();
        }

        [Fact]
        public async Task Should_Throw_When_Submission_Not_Found()
        {
            var subRepo = new Mock<ISubmittedAssignmentRepository>();
            subRepo.Setup(r => r.GetByIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync((SubmittedAssignment?)null);
            var service = new SubmittedAssignmentService(
                subRepo.Object, new Mock<IAssignmentRepository>().Object);

            Func<Task> act = () => service.ScoreQuestionAsync(
                Guid.NewGuid(),
                Guid.NewGuid(),
                new ScoreQuestionRequest { Points = 1 });

            await act.Should().ThrowAsync<KeyNotFoundException>();
        }

        [Fact]
        public async Task Should_Propagate_Domain_Exception_When_Points_Exceed_Max()
        {
            // Arrange
            var assignment = SubmissionTestData.AssignmentWith(pointsPerQuestion: 5);
            var submission = SubmissionTestData.SubmissionWith(assignment);
            var sq = submission.SubmittedExercises.First().SubmittedQuestions.First();

            var subRepo = new Mock<ISubmittedAssignmentRepository>();
            subRepo.Setup(r => r.GetByIdAsync(submission.Id)).ReturnsAsync(submission);

            var service = new SubmittedAssignmentService(
                subRepo.Object, new Mock<IAssignmentRepository>().Object);

            // Act
            Func<Task> act = () => service.ScoreQuestionAsync(
                submission.Id,
                sq.Id,
                new ScoreQuestionRequest { Points = 99 });

            // Assert
            await act.Should().ThrowAsync<ArgumentOutOfRangeException>();
            subRepo.Verify(r => r.SaveChangesAsync(), Times.Never);
        }

        [Fact]
        public async Task Should_Propagate_Domain_Exception_When_Question_Not_In_Submission()
        {
            // Arrange
            var submission = SubmissionTestData.SubmissionWith();
            var subRepo = new Mock<ISubmittedAssignmentRepository>();
            subRepo.Setup(r => r.GetByIdAsync(submission.Id)).ReturnsAsync(submission);

            var service = new SubmittedAssignmentService(
                subRepo.Object, new Mock<IAssignmentRepository>().Object);

            // Act
            Func<Task> act = () => service.ScoreQuestionAsync(
                submission.Id,
                Guid.NewGuid(),
                new ScoreQuestionRequest { Points = 1 });

            // Assert
            await act.Should().ThrowAsync<KeyNotFoundException>();
        }
    }

    // ─────────────────────────────────────────────────────────────────────────
    // SetExerciseCommentAsync
    // ─────────────────────────────────────────────────────────────────────────

    public class SubmittedAssignmentServiceTests_SetExerciseComment_Tests
    {
        [Fact]
        public async Task Should_Set_Comment_And_Return_DTO()
        {
            // Arrange
            var submission = SubmissionTestData.SubmissionWith();
            var se = submission.SubmittedExercises.First();

            var subRepo = new Mock<ISubmittedAssignmentRepository>();
            subRepo.Setup(r => r.GetByIdAsync(submission.Id)).ReturnsAsync(submission);
            subRepo.Setup(r => r.SaveChangesAsync()).Returns(Task.CompletedTask);

            var service = new SubmittedAssignmentService(
                subRepo.Object, new Mock<IAssignmentRepository>().Object);

            // Act
            var dto = await service.SetExerciseCommentAsync(
                submission.Id, se.Id, new SetExerciseCommentRequest { Comment = "Well done" });

            // Assert
            dto.Id.Should().Be(se.Id);
            dto.OverallComment.Should().Be("Well done");
            dto.SourceTitle.Should().NotBeEmpty();

            subRepo.Verify(r => r.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task Should_Throw_When_Request_Null()
        {
            var service = new SubmittedAssignmentService(
                new Mock<ISubmittedAssignmentRepository>().Object,
                new Mock<IAssignmentRepository>().Object);

            Func<Task> act = () => service.SetExerciseCommentAsync(
                Guid.NewGuid(), Guid.NewGuid(), null!);

            await act.Should().ThrowAsync<ArgumentNullException>();
        }

        [Fact]
        public async Task Should_Throw_When_Submission_Not_Found()
        {
            var subRepo = new Mock<ISubmittedAssignmentRepository>();
            subRepo.Setup(r => r.GetByIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync((SubmittedAssignment?)null);
            var service = new SubmittedAssignmentService(
                subRepo.Object, new Mock<IAssignmentRepository>().Object);

            Func<Task> act = () => service.SetExerciseCommentAsync(
                Guid.NewGuid(), Guid.NewGuid(), new SetExerciseCommentRequest());

            await act.Should().ThrowAsync<KeyNotFoundException>();
        }

        [Fact]
        public async Task Should_Propagate_Domain_Exception_When_Exercise_Not_In_Submission()
        {
            var submission = SubmissionTestData.SubmissionWith();
            var subRepo = new Mock<ISubmittedAssignmentRepository>();
            subRepo.Setup(r => r.GetByIdAsync(submission.Id)).ReturnsAsync(submission);
            var service = new SubmittedAssignmentService(
                subRepo.Object, new Mock<IAssignmentRepository>().Object);

            Func<Task> act = () => service.SetExerciseCommentAsync(
                submission.Id, Guid.NewGuid(), new SetExerciseCommentRequest());

            await act.Should().ThrowAsync<KeyNotFoundException>();
        }
    }

    // ─────────────────────────────────────────────────────────────────────────
    // MarkEvaluatedAsync
    // ─────────────────────────────────────────────────────────────────────────

    public class SubmittedAssignmentServiceTests_MarkEvaluated_Tests
    {
        private static SubmittedAssignment FullyScoredSubmission(out Mock<ISubmittedAssignmentRepository> repoMock)
        {
            var submission = SubmissionTestData.SubmissionWith();
            var q = submission.SubmittedExercises.First().SubmittedQuestions.First();
            submission.ScoreQuestion(q.Id, 1, null, null);

            repoMock = new Mock<ISubmittedAssignmentRepository>();
            repoMock.Setup(r => r.GetByIdAsync(submission.Id)).ReturnsAsync(submission);
            repoMock.Setup(r => r.SaveChangesAsync()).Returns(Task.CompletedTask);
            return submission;
        }

        [Fact]
        public async Task Should_Mark_Completed_And_Return_DTO()
        {
            // Arrange
            var submission = FullyScoredSubmission(out var subRepo);
            var evaluatorId = Guid.NewGuid();
            var service = new SubmittedAssignmentService(
                subRepo.Object, new Mock<IAssignmentRepository>().Object);

            // Act
            var dto = await service.MarkEvaluatedAsync(
                submission.Id, new MarkEvaluatedRequest { EvaluatorId = evaluatorId });

            // Assert
            dto.Status.Should().Be(EvaluationStatus.Completed);
            dto.EvaluatorId.Should().Be(evaluatorId);
            subRepo.Verify(r => r.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task Should_Throw_When_Request_Null()
        {
            var service = new SubmittedAssignmentService(
                new Mock<ISubmittedAssignmentRepository>().Object,
                new Mock<IAssignmentRepository>().Object);

            Func<Task> act = () => service.MarkEvaluatedAsync(Guid.NewGuid(), null!);

            await act.Should().ThrowAsync<ArgumentNullException>();
        }

        [Fact]
        public async Task Should_Throw_When_Submission_Not_Found()
        {
            var subRepo = new Mock<ISubmittedAssignmentRepository>();
            subRepo.Setup(r => r.GetByIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync((SubmittedAssignment?)null);
            var service = new SubmittedAssignmentService(
                subRepo.Object, new Mock<IAssignmentRepository>().Object);

            Func<Task> act = () => service.MarkEvaluatedAsync(
                Guid.NewGuid(), new MarkEvaluatedRequest { EvaluatorId = Guid.NewGuid() });

            await act.Should().ThrowAsync<KeyNotFoundException>();
        }

        [Fact]
        public async Task Should_Propagate_Domain_Exception_When_Questions_Unscored()
        {
            // Arrange
            var submission = SubmissionTestData.SubmissionWith(); // nothing scored
            var subRepo = new Mock<ISubmittedAssignmentRepository>();
            subRepo.Setup(r => r.GetByIdAsync(submission.Id)).ReturnsAsync(submission);
            var service = new SubmittedAssignmentService(
                subRepo.Object, new Mock<IAssignmentRepository>().Object);

            // Act
            Func<Task> act = () => service.MarkEvaluatedAsync(
                submission.Id, new MarkEvaluatedRequest { EvaluatorId = Guid.NewGuid() });

            // Assert
            await act.Should().ThrowAsync<InvalidOperationException>();
        }
    }

    // ─────────────────────────────────────────────────────────────────────────
    // ReturnSubmissionAsync
    // ─────────────────────────────────────────────────────────────────────────

    public class SubmittedAssignmentServiceTests_Return_Tests
    {
        [Fact]
        public async Task Should_Return_Submission_When_Completed()
        {
            // Arrange
            var submission = SubmissionTestData.SubmissionWith();
            var q = submission.SubmittedExercises.First().SubmittedQuestions.First();
            submission.ScoreQuestion(q.Id, 1, null, null);
            submission.MarkEvaluated(Guid.NewGuid());

            var subRepo = new Mock<ISubmittedAssignmentRepository>();
            subRepo.Setup(r => r.GetByIdAsync(submission.Id)).ReturnsAsync(submission);
            subRepo.Setup(r => r.SaveChangesAsync()).Returns(Task.CompletedTask);

            var service = new SubmittedAssignmentService(
                subRepo.Object, new Mock<IAssignmentRepository>().Object);

            // Act
            var dto = await service.ReturnSubmissionAsync(submission.Id);

            // Assert
            dto.Status.Should().Be(EvaluationStatus.Returned);
            subRepo.Verify(r => r.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task Should_Throw_When_Not_Found()
        {
            var subRepo = new Mock<ISubmittedAssignmentRepository>();
            subRepo.Setup(r => r.GetByIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync((SubmittedAssignment?)null);
            var service = new SubmittedAssignmentService(
                subRepo.Object, new Mock<IAssignmentRepository>().Object);

            Func<Task> act = () => service.ReturnSubmissionAsync(Guid.NewGuid());

            await act.Should().ThrowAsync<KeyNotFoundException>();
        }

        [Fact]
        public async Task Should_Propagate_Domain_Exception_When_Not_Completed()
        {
            // Arrange
            var submission = SubmissionTestData.SubmissionWith();
            var subRepo = new Mock<ISubmittedAssignmentRepository>();
            subRepo.Setup(r => r.GetByIdAsync(submission.Id)).ReturnsAsync(submission);
            var service = new SubmittedAssignmentService(
                subRepo.Object, new Mock<IAssignmentRepository>().Object);

            // Act
            Func<Task> act = () => service.ReturnSubmissionAsync(submission.Id);

            // Assert
            await act.Should().ThrowAsync<InvalidOperationException>();
        }
    }
}
