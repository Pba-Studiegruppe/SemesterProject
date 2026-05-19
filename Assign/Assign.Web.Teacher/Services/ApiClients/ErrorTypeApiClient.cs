using System.Net.Http.Json;
using Assign.Web.Teacher.Models.Assignment;

namespace Assign.Web.Teacher.Services.ApiClients
{
    /// <summary>
    /// Talks to Assignment-Api's /api/error-types endpoints. Currently used
    /// only by the evaluation page (to populate the "type of mistake"
    /// dropdown), but stands alone so future ErrorType management pages
    /// can reuse it.
    /// </summary>
    public class ErrorTypeApiClient
    {
        private readonly HttpClient _http;
        private readonly ILogger<ErrorTypeApiClient> _logger;

        public ErrorTypeApiClient(HttpClient http, ILogger<ErrorTypeApiClient> logger)
        {
            _http = http;
            _logger = logger;
        }

        public async Task<IReadOnlyList<ErrorTypeModel>> GetAllAsync(
            CancellationToken cancellationToken = default)
        {
            try
            {
                var items = await _http.GetFromJsonAsync<List<ErrorTypeModel>>(
                    "api/error-types", cancellationToken);
                return items ?? new List<ErrorTypeModel>();
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Failed to fetch error types");
                return Array.Empty<ErrorTypeModel>();
            }
        }
    }
}
