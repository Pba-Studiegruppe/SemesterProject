using Assignment_Api;
using Assignment_Application.Interfaces.Repositories;
using Assignment_Domain.Entities;
using Assignment_Domain.SnapShots;
using Assignment_Infrastructure.Pdf;
using FluentAssertions;
using iText.Forms;
using iText.Forms.Fields;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas.Parser;
using Moq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Assignment_Tests.Infrastructure
{
    // Generated with Claude.
    public class AssignmentPdfGeneratorTests_ErrorBehavior
    {
        [Fact]
        public async Task GenerateAsync_Should_Throw_When_Assignment_Not_Found()
        {
            // Arrange
            var repoMock = new Mock<IAssignmentRepository>();
            repoMock.Setup(r => r.GetByIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync((Assignment?)null);

            var generator = new AssignmentPdfGenerator(repoMock.Object);

            // Act
            Func<Task> act = () => generator.GenerateAsync(Guid.NewGuid());

            // Assert
            await act.Should().ThrowAsync<KeyNotFoundException>();
        }
    }

    public class AssignmentPdfGeneratorTests_PdfStructure
    {
        [Fact]
        public async Task GenerateAsync_Should_Return_Non_Empty_Byte_Array()
        {
            // Arrange
            var (generator, _) = PdfTestHarness.WithAssignment(PdfTestHarness.SimpleAssignment());

            // Act
            var bytes = await generator.GenerateAsync(PdfTestHarness.SimpleAssignmentId);

            // Assert
            bytes.Should().NotBeNullOrEmpty();
        }

        [Fact]
        public async Task GenerateAsync_Should_Return_Bytes_With_Pdf_Signature()
        {
            // Arrange
            var (generator, _) = PdfTestHarness.WithAssignment(PdfTestHarness.SimpleAssignment());

            // Act
            var bytes = await generator.GenerateAsync(PdfTestHarness.SimpleAssignmentId);

            // Assert
            // PDFs begin with "%PDF-"
            Encoding.ASCII.GetString(bytes, 0, 5).Should().Be("%PDF-");
        }
    }

     






    public class AssignmentPdfGeneratorTests_Content
    {
        [Fact]
        public async Task GenerateAsync_Should_Render_All_Exercise_Titles()
        {
            // Arrange
            var assignment = new Assignment("a", "b");
            assignment.AddExerciseFromSnapshot(new ExerciseSnapshotInput(
                Guid.NewGuid(), "Algebra Warmup", "C",
                new List<QuestionSnapshotInput> { new(Guid.NewGuid(), "Q", "C") }));
            assignment.AddExerciseFromSnapshot(new ExerciseSnapshotInput(
                Guid.NewGuid(), "Geometry Round", "C",
                new List<QuestionSnapshotInput> { new(Guid.NewGuid(), "Q", "C") }));

            var (generator, _) = PdfTestHarness.WithAssignment(assignment);

            // Act
            var bytes = await generator.GenerateAsync(assignment.Id);

            // Assert
            var text = PdfTestHarness.ReadText(bytes);
            text.Should().Contain("Algebra Warmup");
            text.Should().Contain("Geometry Round");
        }

        [Fact]
        public async Task GenerateAsync_Should_Render_Question_Titles_And_Content()
        {
            // Arrange
            var assignment = new Assignment("a", "b");
            assignment.AddExerciseFromSnapshot(new ExerciseSnapshotInput(
                Guid.NewGuid(), "Ex", "C",
                new List<QuestionSnapshotInput>
                {
                    new(Guid.NewGuid(), "Question One", "What is 2 + 2?"),
                    new(Guid.NewGuid(), "Question Two", "Solve for x: 3x = 9"),
                }));

            var (generator, _) = PdfTestHarness.WithAssignment(assignment);

            // Act
            var bytes = await generator.GenerateAsync(assignment.Id);

            // Assert
            var text = PdfTestHarness.ReadText(bytes);
            text.Should().Contain("Question One");
            text.Should().Contain("What is 2 + 2?");
            text.Should().Contain("Question Two");
            text.Should().Contain("Solve for x: 3x = 9");
        }

        [Fact]
        public async Task GenerateAsync_Should_Render_Question_Points()
        {
            // Arrange
            var assignment = new Assignment("a", "b");
            var ae = assignment.AddExerciseFromSnapshot(new ExerciseSnapshotInput(
                Guid.NewGuid(), "Ex", "C",
                new List<QuestionSnapshotInput> { new(Guid.NewGuid(), "Q1", "C") }));
            ae.SetQuestionPoints(ae.Questions[0].Id, 17);

            var (generator, _) = PdfTestHarness.WithAssignment(assignment);

            // Act
            var bytes = await generator.GenerateAsync(assignment.Id);

            // Assert
            // The exact format ("17 pts", "(17 pts)", "17 points") is generator-defined;
            // assert only that the number appears in proximity to "pts".
            var text = PdfTestHarness.ReadText(bytes);
            text.Should().MatchRegex(@"17\s*pts");
        }
    }

    internal static class PdfTestHarness
    {
        public static readonly Guid SimpleAssignmentId = Guid.NewGuid();

        public static Assignment SimpleAssignment()
        {
            // A small but non-empty assignment, with a deterministic id so tests
            // that take an id directly can use SimpleAssignmentId.
            var assignment = new Assignment("Simple Assignment", "desc");
            // Reflect the id we promise.
            typeof(Assignment).GetProperty(nameof(Assignment.Id))!
                .SetValue(assignment, SimpleAssignmentId);

            assignment.AddExerciseFromSnapshot(new ExerciseSnapshotInput(
                Guid.NewGuid(), "Ex", "Content",
                new List<QuestionSnapshotInput>
                {
                    new(Guid.NewGuid(), "Q", "Body"),
                }));
            return assignment;
        }

        public static Assignment BuildAssignment(
            int exerciseCount,
            int questionsPerExercise,
            int pointsPerQuestion = 5)
        {
            var assignment = new Assignment("Test Assignment", "desc");

            for (var e = 0; e < exerciseCount; e++)
            {
                var questions = Enumerable.Range(0, questionsPerExercise)
                    .Select(q => new QuestionSnapshotInput(
                        Guid.NewGuid(), $"Q{e}.{q}", $"Body {e}.{q}"))
                    .ToList();

                var ae = assignment.AddExerciseFromSnapshot(new ExerciseSnapshotInput(
                    Guid.NewGuid(), $"Exercise {e}", $"Ex {e} content", questions));

                foreach (var q in ae.Questions)
                    ae.SetQuestionPoints(q.Id, pointsPerQuestion);
            }

            return assignment;
        }

        public static (AssignmentPdfGenerator generator, Mock<IAssignmentRepository> repoMock)
            WithAssignment(Assignment assignment)
        {
            var repoMock = new Mock<IAssignmentRepository>();
            repoMock.Setup(r => r.GetByIdAsync(assignment.Id))
                .ReturnsAsync(assignment);
            return (new AssignmentPdfGenerator(repoMock.Object), repoMock);
        }

        public static IReadOnlyDictionary<string, string?> ReadFields(byte[] pdfBytes)
        {
            using var reader = new PdfReader(new MemoryStream(pdfBytes));
            using var pdf = new PdfDocument(reader);
            var form = PdfAcroForm.GetAcroForm(pdf, false);
            return form
                .GetAllFormFields()
                .ToDictionary(kvp => kvp.Key, kvp => (string?)kvp.Value.GetValueAsString());
        }

        public static bool IsReadOnly(byte[] pdfBytes, string fieldName)
        {
            using var reader = new PdfReader(new MemoryStream(pdfBytes));
            using var pdf = new PdfDocument(reader);
            var form = PdfAcroForm.GetAcroForm(pdf, false);
            var field = form.GetField(fieldName);
            if (field is null)
                throw new InvalidOperationException(
                    $"Field '{fieldName}' was not found in the PDF.");
            return field.IsReadOnly();
        }

        public static string ReadText(byte[] pdfBytes)
        {
            using var reader = new PdfReader(new MemoryStream(pdfBytes));
            using var pdf = new PdfDocument(reader);
            var sb = new StringBuilder();
            for (var i = 1; i <= pdf.GetNumberOfPages(); i++)
                sb.AppendLine(PdfTextExtractor.GetTextFromPage(pdf.GetPage(i)));
            return sb.ToString();
        }
    }
}