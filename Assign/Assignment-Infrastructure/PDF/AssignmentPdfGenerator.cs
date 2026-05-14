using Assignment_Api;
using Assignment_Application.Interfaces.Repositories;
using Assignment_Application.Interfaces.Services;
using Assignment_Application.Pdf;
using iText.Forms;
using iText.Forms.Fields;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;
using iText.Kernel.Font;
using iText.IO.Font.Constants;


// Lavet ved hjælp af Claude

namespace Assignment_Infrastructure.Pdf
{
    public class AssignmentPdfGenerator : IAssignmentPdfService
    {
        private readonly IAssignmentRepository _repository;

        public AssignmentPdfGenerator(IAssignmentRepository repository)
        {
            _repository = repository;
        }


        public async Task<byte[]> GenerateAsync(Guid assignmentId)
        {
            var assignment = await _repository.GetByIdAsync(assignmentId);
            if (assignment is null)
                throw new KeyNotFoundException($"Assignment '{assignmentId}' not found.");

            var bold = PdfFontFactory.CreateFont(StandardFonts.HELVETICA_BOLD);
            using var ms = new MemoryStream();
            using (var writer = new PdfWriter(ms))
            using (var pdf = new PdfDocument(writer))
            using (var doc = new Document(pdf))
            {
                var form = PdfAcroForm.GetAcroForm(pdf, true);

                // Header
                doc.Add(new Paragraph(assignment.Title ?? string.Empty)
                    .SetFontSize(20)
                    .SetFont(bold));

                if (!string.IsNullOrWhiteSpace(assignment.Description))
                    doc.Add(new Paragraph(assignment.Description));

                // Metadata fields
                AddMetaField(doc, form, pdf, "Student name:", AssignmentPdfFieldNames.MetaStudentName);
                AddMetaField(doc, form, pdf, "Date:", AssignmentPdfFieldNames.MetaDate);
                AddReadOnlyHidden(form, pdf,
                    AssignmentPdfFieldNames.MetaAssignmentId, assignment.Id.ToString("D"));

                // Exercises and questions
                foreach (var exercise in assignment.AssignmentExercises.OrderBy(e => e.Order))
                {
                    doc.Add(new Paragraph(exercise.Title)
                        .SetFontSize(14)
                        .SetFont(bold)
                        .SetMarginTop(16));

                    if (!string.IsNullOrWhiteSpace(exercise.Content))
                        doc.Add(new Paragraph(exercise.Content));

                    foreach (var question in exercise.Questions.OrderBy(q => q.Order))
                    {
                        doc.Add(new Paragraph($"{question.Title}  ({question.Points} pts)")
                            .SetFont(bold)
                            .SetMarginTop(8));

                        if (!string.IsNullOrWhiteSpace(question.Content))
                            doc.Add(new Paragraph(question.Content));

                        AddAnswerField(doc, form, pdf, question.Id);
                    }
                }
            }

            return ms.ToArray();
        }

        private static void AddMetaField(
            Document doc, PdfAcroForm form, PdfDocument pdf, string label, string fieldName)
        {
            doc.Add(new Paragraph(label).SetMarginTop(6));
            var rect = new Rectangle(0, 0, 300, 20);
            var field = new TextFormFieldBuilder(pdf, fieldName)
                .SetWidgetRectangle(rect)
                .CreateText();
            field.SetValue(string.Empty);
            form.AddField(field);
        }

        private static void AddReadOnlyHidden(
            PdfAcroForm form, PdfDocument pdf, string fieldName, string value)
        {
            var field = new TextFormFieldBuilder(pdf, fieldName)
                .SetWidgetRectangle(new Rectangle(0, 0, 1, 1))
                .CreateText();
            field.SetValue(value);
            field.SetReadOnly(true);
            form.AddField(field);
        }

        private static void AddAnswerField(
            Document doc, PdfAcroForm form, PdfDocument pdf, Guid questionId)
        {
            var fieldName = AssignmentPdfFieldNames.Answer(questionId);
            var rect = new Rectangle(0, 0, 500, 80);
            var field = new TextFormFieldBuilder(pdf, fieldName)
                .SetWidgetRectangle(rect)
                .CreateText();
            field.SetMultiline(true);
            field.SetValue(string.Empty);
            form.AddField(field);

            // Reserve space in the layout flow so the widget is positioned visibly.
            doc.Add(new Paragraph(" ").SetMarginBottom(80));
        }
    }
}