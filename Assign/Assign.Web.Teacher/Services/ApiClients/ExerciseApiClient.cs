using System.Net.Http.Json;
using Assign.Web.Teacher.Models.Exercise;

namespace Assign_Frontend.Services;

/// <summary>
/// Talks to Exercise-Api. The base URL is configured in appsettings.json
/// under "ExerciseApi:BaseUrl".
/// </summary>
public class ExerciseApiClient
{
    private readonly HttpClient _http;
    private readonly ILogger<ExerciseApiClient> _logger;

    public ExerciseApiClient(HttpClient http, ILogger<ExerciseApiClient> logger)
    {
        _http = http;
        _logger = logger;
    }

    // POST /api/Exercise
    public async Task<ExerciseResponse?> CreateExerciseAsync(
        CreateExerciseRequest request,
        CancellationToken cancellationToken = default)
    {
        var response = await _http.PostAsJsonAsync("api/Exercise", request, cancellationToken);
        await EnsureSuccessOrThrowAsync(response, nameof(CreateExerciseAsync), cancellationToken);
        return await response.Content.ReadFromJsonAsync<ExerciseResponse>(cancellationToken: cancellationToken);
    }

    // PUT /api/Exercise/{id}
    public async Task<ExerciseResponse?> UpdateExerciseAsync(
        Guid id,
        UpdateExerciseRequest request,
        CancellationToken cancellationToken = default)
    {
        var response = await _http.PutAsJsonAsync($"api/Exercise/{id}", request, cancellationToken);
        if (response.StatusCode == System.Net.HttpStatusCode.NotFound) return null;
        await EnsureSuccessOrThrowAsync(response, nameof(UpdateExerciseAsync), cancellationToken);
        return await response.Content.ReadFromJsonAsync<ExerciseResponse>(cancellationToken: cancellationToken);
    }

    // GET /api/Exercise?search=
    public async Task<IReadOnlyList<ExerciseResponse>> GetAllAsync(
        string? search = null,
        CancellationToken cancellationToken = default)
    {
        var url = string.IsNullOrWhiteSpace(search)
            ? "api/Exercise"
            : $"api/Exercise?search={Uri.EscapeDataString(search)}";

        try
        {
            var items = await _http.GetFromJsonAsync<List<ExerciseResponse>>(url, cancellationToken);
            return items ?? new List<ExerciseResponse>();
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Failed to fetch exercises (search={Search})", search);
            return Array.Empty<ExerciseResponse>();
        }
    }

    // GET /api/Exercise/by-teacher/{teacherId}
    public async Task<IReadOnlyList<ExerciseResponse>> GetByTeacherAsync(
        Guid teacherId,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var items = await _http.GetFromJsonAsync<List<ExerciseResponse>>(
                $"api/Exercise/by-teacher/{teacherId}", cancellationToken);
            return items ?? new List<ExerciseResponse>();
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Failed to fetch exercises for teacher {TeacherId}", teacherId);
            return Array.Empty<ExerciseResponse>();
        }
    }

    // GET /api/Exercise/by-keywords?keywordIds=...&keywordIds=...
    public async Task<IReadOnlyList<ExerciseResponse>> GetByKeywordsAsync(
        IEnumerable<Guid> keywordIds,
        CancellationToken cancellationToken = default)
    {
        var query = string.Join("&", keywordIds.Select(id => $"keywordIds={id}"));
        var url = string.IsNullOrEmpty(query)
            ? "api/Exercise/by-keywords"
            : $"api/Exercise/by-keywords?{query}";

        try
        {
            var items = await _http.GetFromJsonAsync<List<ExerciseResponse>>(url, cancellationToken);
            return items ?? new List<ExerciseResponse>();
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Failed to fetch exercises by keywords");
            return Array.Empty<ExerciseResponse>();
        }
    }

    // GET /api/Exercise/{id}/snapshot
    public async Task<ExerciseSnapshotResponse?> GetSnapshotAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        var response = await _http.GetAsync($"api/Exercise/{id}/snapshot", cancellationToken);
        if (response.StatusCode == System.Net.HttpStatusCode.NotFound) return null;
        await EnsureSuccessOrThrowAsync(response, nameof(GetSnapshotAsync), cancellationToken);
        return await response.Content.ReadFromJsonAsync<ExerciseSnapshotResponse>(cancellationToken: cancellationToken);
    }

    // GET /api/Exercise/{id}/review
    public async Task<ExerciseReviewResponse?> GetReviewAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        var response = await _http.GetAsync($"api/Exercise/{id}/review", cancellationToken);
        if (response.StatusCode == System.Net.HttpStatusCode.NotFound) return null;
        await EnsureSuccessOrThrowAsync(response, nameof(GetReviewAsync), cancellationToken);
        return await response.Content.ReadFromJsonAsync<ExerciseReviewResponse>(cancellationToken: cancellationToken);
    }

    private async Task EnsureSuccessOrThrowAsync(
        HttpResponseMessage response,
        string operation,
        CancellationToken cancellationToken)
    {
        if (response.IsSuccessStatusCode) return;
        var body = await response.Content.ReadAsStringAsync(cancellationToken);
        _logger.LogWarning("{Operation} failed: {Status} {Body}", operation, response.StatusCode, body);
        throw new HttpRequestException(
            $"{operation} failed ({(int)response.StatusCode} {response.ReasonPhrase}): {body}");
    }
}