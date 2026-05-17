using Assignment_Api;
using Assignment_Application.Interfaces.Repositories;
using Assignment_Application.Interfaces.Services;
using iText.IO.Font.Constants;
using iText.Kernel.Colors;
using iText.Kernel.Font;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas.Draw;
using iText.Layout;
using iText.Layout.Borders;
using iText.Layout.Element;
using iText.Layout.Properties;

// Lavet ved hjælp af Claude

namespace Assignment_Infrastructure.Pdf
{
    public class AssignmentPdfGenerator : IAssignmentPdfService
    {
        private readonly IAssignmentRepository _repository;

        // ── Visual constants (tuned to match the layout mockup) ──────────────
        private static readonly Color HeadingColor = new DeviceRgb(26, 39, 71);     // dark navy
        private static readonly Color BodyColor = new DeviceRgb(40, 40, 40);     // near-black for body text
        private static readonly Color MutedColor = new DeviceRgb(125, 125, 125);  // gray for descriptions / hints / points
        private static readonly Color RuleColor = new DeviceRgb(200, 200, 200);  // light gray for section dividers
        private static readonly Color WriteLineColor = new DeviceRgb(180, 180, 180);  // gray for handwriting lines

        private const float QuestionIndent = 28f;
        private const int WriteLinesPerQuestion = 2;

        public AssignmentPdfGenerator(IAssignmentRepository repository)
        {
            _repository = repository;
        }

        public async Task<byte[]> GenerateAsync(Guid assignmentId)
        {
            var assignment = await _repository.GetByIdAsync(assignmentId);
            if (assignment is null)
                throw new KeyNotFoundException($"Assignment '{assignmentId}' not found.");

            var regular = PdfFontFactory.CreateFont(StandardFonts.HELVETICA);
            var bold = PdfFontFactory.CreateFont(StandardFonts.HELVETICA_BOLD);

            using var ms = new MemoryStream();
            using (var writer = new PdfWriter(ms))
            using (var pdf = new PdfDocument(writer))
            {
                pdf.SetDefaultPageSize(PageSize.A4);
                using var doc = new Document(pdf);
                doc.SetMargins(54f, 54f, 54f, 54f);
                doc.SetFont(regular).SetFontColor(BodyColor);

                AddTitleBlock(doc, bold, assignment.Title, assignment.Description);
                AddStudentDateRow(doc);
                AddDivider(doc);

                var exercises = assignment.AssignmentExercises.OrderBy(e => e.Order).ToList();
                for (int i = 0; i < exercises.Count; i++)
                {
                    var exercise = exercises[i];

                    AddExerciseHeader(doc, bold,
                        exerciseNumber: i + 1,
                        title: exercise.Title,
                        content: exercise.Content);

                    var questions = exercise.Questions.OrderBy(q => q.Order).ToList();
                    if (questions.Count == 0)
                    {
                        AddEmptyExerciseNotice(doc);
                    }
                    else
                    {
                        for (int q = 0; q < questions.Count; q++)
                        {
                            var question = questions[q];
                            AddQuestionBlock(doc, bold,
                                questionNumber: q + 1,
                                title: question.Title ?? string.Empty,
                                content: question.Content,
                                points: question.Points);
                        }
                    }

                    if (i < exercises.Count - 1)
                        AddDivider(doc);
                }
            }

            return ms.ToArray();
        }

        // ── Title + subtitle ─────────────────────────────────────────────────

        private static void AddTitleBlock(Document doc, PdfFont bold, string? title, string? description)
        {
            doc.Add(new Paragraph((title ?? string.Empty).ToUpperInvariant())
                .SetFont(bold)
                .SetFontSize(26)
                .SetFontColor(HeadingColor)
                .SetTextAlignment(TextAlignment.CENTER)
                .SetMultipliedLeading(1.1f)
                .SetMarginTop(0)
                .SetMarginBottom(4));

            if (!string.IsNullOrWhiteSpace(description))
            {
                doc.Add(new Paragraph(description)
                    .SetFontSize(10)
                    .SetFontColor(MutedColor)
                    .SetTextAlignment(TextAlignment.CENTER)
                    .SetMarginTop(0)
                    .SetMarginBottom(14));
            }
        }

        // ── Student / Date row ───────────────────────────────────────────────

        private static void AddStudentDateRow(Document doc)
        {
            var table = new Table(UnitValue.CreatePercentArray(new float[] { 14, 36, 14, 36 }))
                .UseAllAvailableWidth()
                .SetMarginTop(6)
                .SetMarginBottom(6);

            table.AddCell(LabelCell("Student:"));
            table.AddCell(WriteLineCell());
            table.AddCell(LabelCell("Date:"));
            table.AddCell(WriteLineCell());

            doc.Add(table);
        }

        private static Cell LabelCell(string text) =>
            new Cell()
                .Add(new Paragraph(text).SetFontSize(10).SetMargin(0))
                .SetBorder(Border.NO_BORDER)
                .SetVerticalAlignment(VerticalAlignment.BOTTOM)
                .SetPaddingBottom(2);

        private static Cell WriteLineCell() =>
            new Cell()
                .SetBorder(Border.NO_BORDER)
                .SetBorderBottom(new SolidBorder(WriteLineColor, 0.75f))
                .SetHeight(16);

        // ── Section divider ──────────────────────────────────────────────────

        private static void AddDivider(Document doc)
        {
            var line = new SolidLine(0.5f);
            line.SetColor(RuleColor);
            doc.Add(new LineSeparator(line)
                .SetMarginTop(14)
                .SetMarginBottom(10));
        }

        // ── Exercise header ──────────────────────────────────────────────────

        private static void AddExerciseHeader(Document doc, PdfFont bold,
            int exerciseNumber, string? title, string? content)
        {
            // "EXERCISE N · Title" — \u00B7 is the middle dot, encodable in WinAnsi.
            doc.Add(new Paragraph()
                .SetFont(bold)
                .SetFontSize(15)
                .SetFontColor(HeadingColor)
                .SetMarginTop(4)
                .SetMarginBottom(2)
                .Add(new Text($"EXERCISE {exerciseNumber} \u00B7 "))
                .Add(new Text(title ?? string.Empty)));

            if (!string.IsNullOrWhiteSpace(content))
            {
                doc.Add(new Paragraph(content)
                    .SetFontSize(10)
                    .SetFontColor(MutedColor)
                    .SetMarginTop(0)
                    .SetMarginBottom(6));
            }
        }

        // ── Empty-exercise notice ────────────────────────────────────────────

        private static void AddEmptyExerciseNotice(Document doc)
        {
            var notice = new Div()
                .Add(new Paragraph("No questions have been added to this exercise yet.")
                    .SetFontSize(10)
                    .SetFontColor(MutedColor)
                    .SetMargin(0))
                .SetBorder(new SolidBorder(RuleColor, 0.5f))
                .SetPaddingTop(10).SetPaddingBottom(10)
                .SetPaddingLeft(14).SetPaddingRight(14)
                .SetMarginLeft(QuestionIndent)
                .SetMarginTop(6)
                .SetMarginBottom(6);

            doc.Add(notice);
        }

        // ── Question block ───────────────────────────────────────────────────

        private static void AddQuestionBlock(Document doc, PdfFont bold,
            int questionNumber, string title, string? content, int points)
        {
            // Header row: "[N] question..." on the left, "N pts" on the right.
            // The bottom border of both cells acts as the underline under the title.
            var headerTable = new Table(UnitValue.CreatePercentArray(new float[] { 85, 15 }))
                .UseAllAvailableWidth()
                .SetMarginLeft(QuestionIndent)
                .SetMarginTop(10)
                .SetMarginBottom(0);

            var titlePara = new Paragraph()
                .SetFont(bold)
                .SetFontSize(11)
                .SetFontColor(BodyColor)
                .SetMargin(0)
                .Add(new Text($"[{questionNumber}] "))
                .Add(new Text(title));

            var pointsPara = new Paragraph($"{points} pts")
                .SetFontSize(9)
                .SetFontColor(MutedColor)
                .SetTextAlignment(TextAlignment.RIGHT)
                .SetMargin(0);

            headerTable.AddCell(new Cell()
                .Add(titlePara)
                .SetBorder(Border.NO_BORDER)
                .SetBorderBottom(new SolidBorder(RuleColor, 0.5f))
                .SetPaddingTop(0).SetPaddingBottom(6)
                .SetPaddingLeft(0).SetPaddingRight(0));

            headerTable.AddCell(new Cell()
                .Add(pointsPara)
                .SetBorder(Border.NO_BORDER)
                .SetBorderBottom(new SolidBorder(RuleColor, 0.5f))
                .SetPaddingTop(2).SetPaddingBottom(6)
                .SetPaddingLeft(0).SetPaddingRight(0)
                .SetVerticalAlignment(VerticalAlignment.TOP));

            doc.Add(headerTable);

            // Optional hint / content paragraph with a small arrow prefix.
            // Helvetica's WinAnsi encoding doesn't contain "→" (U+2192), so we use
            // the single right-pointing angle quotation (›, U+203A) as a close
            // visual substitute. If you want the actual arrow, register a Unicode
            // TTF (e.g. DejaVu Sans) with Identity-H encoding and swap the prefix.
            if (!string.IsNullOrWhiteSpace(content))
            {
                doc.Add(new Paragraph("\u203A " + content)
                    .SetFontSize(9)
                    .SetFontColor(MutedColor)
                    .SetMarginTop(4)
                    .SetMarginBottom(4));
            }

            // Plain printable lines for handwritten answers — no AcroForm fields.
            for (int i = 0; i < WriteLinesPerQuestion; i++)
            {
                var line = new SolidLine(0.5f);
                line.SetColor(WriteLineColor);
                doc.Add(new LineSeparator(line)
                    .SetMarginTop(8)
                    .SetMarginBottom(0));
            }
        }
    }
}