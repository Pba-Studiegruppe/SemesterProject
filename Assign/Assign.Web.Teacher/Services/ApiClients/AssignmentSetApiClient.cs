using System.Net.Http.Json;
using Assign.Web.Teacher.Models.AssignmentSet;

namespace Assign.Web.Teacher.Services.ApiClients
{
    /// <summary>
    /// Talks to Assignment-Api's /api/assignmentsets endpoints. Base URL is
    /// configured in appsettings.json under "AssignmentApi:BaseUrl".
    /// </summary>
    public class AssignmentSetApiClient
    {
        private readonly HttpClient _http;
        private readonly ILogger<AssignmentSetApiClient> _logger;

        public AssignmentSetApiClient(HttpClient http, ILogger<AssignmentSetApiClient> logger)
        {
            _http = http;
            _logger = logger;
        }

        public async Task<IReadOnlyList<AssignmentSetSummary>> GetAllAsync(
            CancellationToken cancellationToken = default)
        {
            try
            {
                var items = await _http.GetFromJsonAsync<List<AssignmentSetSummary>>(
                    "api/assignmentsets", cancellationToken);
                return items ?? new List<AssignmentSetSummary>();
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Failed to fetch assignment sets from {BaseAddress}", _http.BaseAddress);
                throw;
            }
        }

        public async Task<AssignmentSetSummary?> GetAsync(
            Guid id, CancellationToken cancellationToken = default)
        {
            return await _http.GetFromJsonAsync<AssignmentSetSummary>(
                $"api/assignmentsets/{id}", cancellationToken);
        }

        public async Task<AssignmentSetSummary?> PublishAsync(
    Guid id, CancellationToken cancellationToken = default)
        {
            var response = await _http.PostAsync(
                $"api/assignmentsets/{id}/publish", content: null, cancellationToken);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<AssignmentSetSummary>(
                cancellationToken: cancellationToken);
        }
    }
}
