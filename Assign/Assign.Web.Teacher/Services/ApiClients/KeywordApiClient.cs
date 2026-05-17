using System.Net.Http.Json;
using Assign.Web.Teacher.Models.Exercise;

namespace Assign_Frontend.Services;

/// <summary>
/// Talks to the Keyword endpoints exposed by Exercise-Api.
/// </summary>
public class KeywordApiClient
{
    //TODO: This could probably be changed to IHttpClientFactory and then we can have a single HttpClient for all API clients, but for now this is fine.
    private readonly HttpClient _http;
    private readonly ILogger<KeywordApiClient> _logger;

    public KeywordApiClient(HttpClient http, ILogger<KeywordApiClient> logger)
    {
        _http = http;
        _logger = logger;
    }

    /// <summary>GET /api/Keyword — returns every keyword in the system.</summary>
    public async Task<IReadOnlyList<KeywordDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            var keywords = await _http.GetFromJsonAsync<List<KeywordDto>>("api/Keyword", cancellationToken);
            return keywords ?? new List<KeywordDto>();
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Failed to fetch keywords from {BaseAddress}", _http.BaseAddress);
            return Array.Empty<KeywordDto>();
        }
    }
}
