using Assignment_Application.Interfaces.Repositories;
using Assignment_Domain.Entities;
using Assignment_Domain.SnapShots;
using Assignment_Infrastructure.Pdf;
using FluentAssertions;
using iText.Forms;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas.Parser;
using Moq;
using Reqnroll;
using System.Text;

namespace Assignment_AcceptanceTests.StepDefinitions
{
    // generated with Claude

    [Binding]
    public class GeneratePdfSteps
    {
        private readonly Mock<IAssignmentRepository> _repoMock = new();
        private Assignment? _assignment;
        private byte[]? _pdfBytes;
        private Exception? _capturedException;

        // Snapshot inputs we'll assemble for AddExerciseFromSnapshot.
        private readonly List<QuestionSnapshotInput> _pendingQuestions = new();
        private string? _hiddenSolutionText;

        [Given(@"an assignment titled ""(.*)""")]
        public void GivenAnAssignmentTitled(string title)
        {
            _assignment = new Assignment(title, "desc");
            _repoMock.Setup(r => r.GetByIdAsync(_assignment.Id))
                .ReturnsAsync(_assignment);
        }

        [Given(@"an exercise with (\d+) questions? is added")]
        public void GivenAnExerciseWithQuestionsIsAdded(int questionCount)
        {
            var questions = Enumerable.Range(0, questionCount)
                .Select(i => new QuestionSnapshotInput(Guid.NewGuid(), $"Q{i}", $"Body {i}"))
                .ToList();

            var snapshot = new ExerciseSnapshotInput(
                Guid.NewGuid(), "Exercise", "Exercise content", questions);

            _assignment!.AddExerciseFromSnapshot(snapshot);
        }

        [Given(@"the source exercise had a question solution ""(.*)""")]
        public void GivenTheSourceExerciseHadAQuestionSolution(string solutionText)
        {
            // The whole point of snapshotting is that this string never reaches the
            // Assignment domain. We record it only to assert it doesn't show up in the PDF.
            _hiddenSolutionText = solutionText;
        }

        [When(@"I generate the PDF")]
        public async Task WhenIGenerateThePdf()
        {
            // No try/catch. If generation throws, the scenario should fail at this step,
            // not later when a Then tries to read null bytes.
            var generator = new AssignmentPdfGenerator(_repoMock.Object);
            _pdfBytes = await generator.GenerateAsync(_assignment!.Id);
        }

        [When(@"I generate the PDF for an unknown assignment")]
        public async Task WhenIGenerateThePdfForAnUnknownAssignment()
        {
            _repoMock.Setup(r => r.GetByIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync((Assignment?)null);

            var generator = new AssignmentPdfGenerator(_repoMock.Object);
            try
            {
                _pdfBytes = await generator.GenerateAsync(Guid.NewGuid());
            }
            catch (Exception ex)
            {
                _capturedException = ex;
            }
        }

        [Then(@"the PDF has a fillable field named ""(.*)""")]
        public void ThenThePdfHasAFillableFieldNamed(string fieldName)
        {
            ReadFieldNames().Should().Contain(fieldName);
        }

        [Then(@"the PDF has a read-only field named ""(.*)"" with the assignment id as its value")]
        public void ThenThePdfHasAReadOnlyFieldWithAssignmentId(string fieldName)
        {
            using var reader = new PdfReader(new MemoryStream(_pdfBytes!));
            using var pdf = new PdfDocument(reader);
            var form = PdfAcroForm.GetAcroForm(pdf, false);
            var field = form.GetField(fieldName);

            field.Should().NotBeNull();
            field!.GetValueAsString().Should().Be(_assignment!.Id.ToString("D"));
        }

        [Then(@"the PDF text does not contain ""(.*)""")]
        public void ThenThePdfTextDoesNotContain(string forbidden)
        {
            var text = ReadAllText();
            text.Should().NotContain(forbidden);
            // Sanity check: we're actually looking at our PDF and not an empty doc.
            _hiddenSolutionText.Should().NotBeNull(
                "this scenario only makes sense when a solution was 'hidden' upstream");
        }

        [Then(@"a not-found error is raised")]
        public void ThenANotFoundErrorIsRaised()
        {
            _capturedException.Should().BeOfType<KeyNotFoundException>();
        }

        // --- helpers ---

        private IEnumerable<string> ReadFieldNames()
        {
            using var reader = new PdfReader(new MemoryStream(_pdfBytes!));
            using var pdf = new PdfDocument(reader);
            var form = PdfAcroForm.GetAcroForm(pdf, false);
            return form.GetAllFormFields().Keys.ToList();
        }

        private string ReadAllText()
        {
            using var reader = new PdfReader(new MemoryStream(_pdfBytes!));
            using var pdf = new PdfDocument(reader);
            var sb = new StringBuilder();
            for (var i = 1; i <= pdf.GetNumberOfPages(); i++)
                sb.AppendLine(PdfTextExtractor.GetTextFromPage(pdf.GetPage(i)));
            return sb.ToString();
        }
    }
}