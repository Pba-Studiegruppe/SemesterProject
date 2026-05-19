using System.Net.Http.Json;
using Assign.Web.Teacher.Models.Assignment;

namespace Assign.Web.Teacher.Services.ApiClients
{
    /// <summary>
    /// Talks to Assignment-Api's /api/submitted-assignments endpoints.
    /// Base URL is configured under "AssignmentApi:BaseUrl" — submissions
    /// live in the same API as everything else.
    /// </summary>
    public class SubmittedAssignmentApiClient
    {
        private readonly HttpClient _http;
        private readonly ILogger<SubmittedAssignmentApiClient> _logger;

        public SubmittedAssignmentApiClient(
            HttpClient http,
            ILogger<SubmittedAssignmentApiClient> logger)
        {
            _http = http;
            _logger = logger;
        }

        public async Task<IReadOnlyList<SubmittedAssignmentSummary>> GetByAssignmentAsync(
            Guid assignmentId,
            CancellationToken cancellationToken = default)
        {
            try
            {
                var items = await _http.GetFromJsonAsync<List<SubmittedAssignmentSummary>>(
                    $"api/submitted-assignments?assignmentId={assignmentId}",
                    cancellationToken);
                return items ?? new List<SubmittedAssignmentSummary>();
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex,
                    "Failed to fetch submissions for assignment {AssignmentId}", assignmentId);
                throw;
            }
        }

        public async Task<SubmittedAssignmentDetail?> GetByIdAsync(
    Guid id, CancellationToken cancellationToken = default)
        {
            return await _http.GetFromJsonAsync<SubmittedAssignmentDetail>(
                $"api/submitted-assignments/{id}", cancellationToken);
        }

        public async Task<SubmittedQuestionDetail?> ScoreQuestionAsync(
            Guid submissionId, Guid questionId, ScoreQuestionRequest request,
            CancellationToken cancellationToken = default)
        {
            var resp = await _http.PatchAsJsonAsync(
                $"api/submitted-assignments/{submissionId}/questions/{questionId}/score",
                request, cancellationToken);
            resp.EnsureSuccessStatusCode();
            return await resp.Content.ReadFromJsonAsync<SubmittedQuestionDetail>(
                cancellationToken: cancellationToken);
        }

        public async Task<SubmittedExerciseDetail?> SetExerciseCommentAsync(
            Guid submissionId, Guid exerciseId, SetExerciseCommentRequest request,
            CancellationToken cancellationToken = default)
        {
            var resp = await _http.PatchAsJsonAsync(
                $"api/submitted-assignments/{submissionId}/exercises/{exerciseId}/comment",
                request, cancellationToken);
            resp.EnsureSuccessStatusCode();
            return await resp.Content.ReadFromJsonAsync<SubmittedExerciseDetail>(
                cancellationToken: cancellationToken);
        }

        public async Task<SubmittedAssignmentDetail?> MarkEvaluatedAsync(
            Guid submissionId, MarkEvaluatedRequest request,
            CancellationToken cancellationToken = default)
        {
            var resp = await _http.PostAsJsonAsync(
                $"api/submitted-assignments/{submissionId}/evaluate",
                request, cancellationToken);
            resp.EnsureSuccessStatusCode();
            return await resp.Content.ReadFromJsonAsync<SubmittedAssignmentDetail>(
                cancellationToken: cancellationToken);
        }

        public async Task<SubmittedAssignmentDetail?> ReturnSubmissionAsync(
            Guid submissionId, CancellationToken cancellationToken = default)
        {
            var resp = await _http.PostAsync(
                $"api/submitted-assignments/{submissionId}/return",
                content: null, cancellationToken);
            resp.EnsureSuccessStatusCode();
            return await resp.Content.ReadFromJsonAsync<SubmittedAssignmentDetail>(
                cancellationToken: cancellationToken);
        }
    }
}
