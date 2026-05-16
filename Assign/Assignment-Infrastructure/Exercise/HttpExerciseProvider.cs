using Assignment_Api;
using Assignment_Application.Interfaces;
using Assignment_Domain.SnapShots;
using System.Net;
using System.Net.Http.Json;

namespace Assignment_Infrastructure.Exercise
{
    public class HttpExerciseProvider : IExerciseProvider
    {
        private readonly HttpClient _httpClient;

        public HttpExerciseProvider(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<ExerciseSnapshotInput?> GetExerciseSnapshotAsync(Guid exerciseId)
        {
            var response = await _httpClient.GetAsync($"api/exercise/{exerciseId}/snapshot");

            if (response.StatusCode == HttpStatusCode.NotFound)
                return null;

            response.EnsureSuccessStatusCode();

            var payload = await response.Content.ReadFromJsonAsync<SnapshotResponse>();
            if (payload is null) return null;

            return new ExerciseSnapshotInput(
                payload.Id,
                payload.Title,
                payload.Content,
                payload.Questions
                    .Select(q => new QuestionSnapshotInput(q.Id, q.Title, q.Content))
                    .ToList());
        }

        // Wire-format DTOs, private on purpose: the Assignment module does not depend
        // on Exercise's projection types. The HTTP boundary is where mapping happens.
        private sealed record SnapshotResponse(
            Guid Id,
            string Title,
            string Content,
            List<QuestionResponse> Questions);

        private sealed record QuestionResponse(
            Guid Id,
            string Title,
            string Content);
    }
}