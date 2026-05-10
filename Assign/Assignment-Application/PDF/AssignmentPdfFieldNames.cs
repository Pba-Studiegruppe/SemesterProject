using System;
using System.Text.RegularExpressions;

// Lavet ved hjælp af Claude

namespace Assignment_Application.Pdf
{
    /// <summary>
    /// The AcroForm field-naming contract shared by PDF generation and PDF readback.
    /// Changes here are breaking changes for any submitted PDF that's already in the wild.
    /// </summary>
    public static class AssignmentPdfFieldNames
    {
        public const string MetaAssignmentId = "meta_assignmentId";
        public const string MetaStudentName = "meta_studentName";
        public const string MetaDate = "meta_date";

        public static string Answer(Guid assignmentQuestionId) =>
            $"q_{assignmentQuestionId:D}_answer";

        // Used by readback to recover the question id from a field name.
        public static readonly Regex AnswerFieldPattern =
            new(@"^q_(?<id>[0-9a-fA-F]{8}-[0-9a-fA-F]{4}-[0-9a-fA-F]{4}-[0-9a-fA-F]{4}-[0-9a-fA-F]{12})_answer$",
                RegexOptions.Compiled);

        public static bool TryParseAnswerField(string fieldName, out Guid questionId)
        {
            var m = AnswerFieldPattern.Match(fieldName);
            if (m.Success && Guid.TryParse(m.Groups["id"].Value, out questionId))
                return true;

            questionId = Guid.Empty;
            return false;
        }
    }
}