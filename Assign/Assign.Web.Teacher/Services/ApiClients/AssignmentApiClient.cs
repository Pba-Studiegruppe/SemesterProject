using System.Net.Http.Json;
using Assign.Web.Teacher.Models.Assignment;

namespace Assign.Web.Teacher.Services.ApiClients
{
    public class AssignmentApiClient
    {
        private readonly HttpClient _http;

        public AssignmentApiClient(HttpClient http)
        {
            _http = http;
        }

        public async Task<byte[]> GetPdfAsync(Guid assignmentId)
        {
            var response = await _http.GetAsync($"api/assignments/{assignmentId}/pdf");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsByteArrayAsync();
        }

        public async Task<AssignmentBuilderModel?> GetAssignmentAsync(Guid assignmentId)
        {
            return await _http.GetFromJsonAsync<AssignmentBuilderModel>($"api/assignments/{assignmentId}");
        }

        public async Task<AssignmentBuilderModel?> CreateAssignmentAsync(
            string title, string description, Guid? assignmentSetId = null)
        {
            var resp = await _http.PostAsJsonAsync(
                "api/assignments",
                new { title, description, assignmentSetId });
            resp.EnsureSuccessStatusCode();
            return await resp.Content.ReadFromJsonAsync<AssignmentBuilderModel>();
        }

        public async Task<AssignmentBuilderModel?> UpdateAssignmentAsync(Guid id, string title, string description)
        {
            var resp = await _http.PutAsJsonAsync($"api/assignments/{id}", new { title, description });
            resp.EnsureSuccessStatusCode();
            return await resp.Content.ReadFromJsonAsync<AssignmentBuilderModel>();
        }

        public async Task<AssignmentExerciseModel?> AddExerciseAsync(Guid assignmentId, Guid sourceExerciseId)
        {
            var resp = await _http.PostAsJsonAsync(
                $"api/assignments/{assignmentId}/exercises",
                new { exerciseId = sourceExerciseId });
            resp.EnsureSuccessStatusCode();
            return await resp.Content.ReadFromJsonAsync<AssignmentExerciseModel>();
        }

        public async Task RemoveExerciseAsync(Guid assignmentId, Guid assignmentExerciseId)
        {
            var resp = await _http.DeleteAsync($"api/assignments/{assignmentId}/exercises/{assignmentExerciseId}");
            resp.EnsureSuccessStatusCode();
        }

        public async Task SetQuestionPointsAsync(Guid assignmentId, Guid assignmentExerciseId, Guid questionId, int points)
        {
            var resp = await _http.PatchAsJsonAsync(
                $"api/assignments/{assignmentId}/exercises/{assignmentExerciseId}/questions/{questionId}/points",
                new { points });
            resp.EnsureSuccessStatusCode();
        }

        public async Task RemoveQuestionAsync(Guid assignmentId, Guid assignmentExerciseId, Guid questionId)
        {
            var resp = await _http.DeleteAsync(
                $"api/assignments/{assignmentId}/exercises/{assignmentExerciseId}/questions/{questionId}");
            resp.EnsureSuccessStatusCode();
        }
    }
}