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
    }
}
