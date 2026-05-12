using System.Net.Http.Json;
using Assign_Frontend.Models;

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

    /// <summary>
    /// POST /api/Exercise — creates a new exercise.
    /// </summary>
    /// <remarks>
    /// NOTE: The current ExerciseController in Exercise-Api is missing
    /// [Route("api/[controller]")] and [ApiController] attributes. Add those
    /// for this endpoint to be reachable at /api/Exercise.
    /// </remarks>
    public async Task<ExerciseResponse?> CreateExerciseAsync(
        CreateExerciseRequest request,
        CancellationToken cancellationToken = default)
    {
        var response = await _http.PostAsJsonAsync("api/Exercise", request, cancellationToken);

        if (!response.IsSuccessStatusCode)
        {
            var body = await response.Content.ReadAsStringAsync(cancellationToken);
            _logger.LogWarning("CreateExercise failed: {Status} {Body}", response.StatusCode, body);
            throw new HttpRequestException(
                $"Failed to create exercise ({(int)response.StatusCode} {response.ReasonPhrase}): {body}");
        }

        return await response.Content.ReadFromJsonAsync<ExerciseResponse>(cancellationToken: cancellationToken);
    }
}
