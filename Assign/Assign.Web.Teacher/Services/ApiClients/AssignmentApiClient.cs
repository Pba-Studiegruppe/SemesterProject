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

        // ---- Already implemented on the API side ----

        public async Task<byte[]> GetPdfAsync(Guid assignmentId)
        {
            var response = await _http.GetAsync($"api/assignments/{assignmentId}/pdf");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsByteArrayAsync();
        }

        // ---- Still need controller endpoints; service-layer logic exists ----

        public async Task<AssignmentBuilderModel?> GetAssignmentAsync(Guid assignmentId)
        {
            // Needs: GET /api/assignments/{id}
            return await _http.GetFromJsonAsync<AssignmentBuilderModel>($"api/assignments/{assignmentId}");
        }

        public async Task<AssignmentBuilderModel?> CreateAssignmentAsync(string title, string description)
        {
            // Needs: POST /api/assignments
            var resp = await _http.PostAsJsonAsync("api/assignments", new { title, description });
            resp.EnsureSuccessStatusCode();
            return await resp.Content.ReadFromJsonAsync<AssignmentBuilderModel>();
        }

        public async Task<AssignmentBuilderModel?> UpdateAssignmentAsync(Guid id, string title, string description)
        {
            // Needs: PUT /api/assignments/{id}
            var resp = await _http.PutAsJsonAsync($"api/assignments/{id}", new { title, description });
            resp.EnsureSuccessStatusCode();
            return await resp.Content.ReadFromJsonAsync<AssignmentBuilderModel>();
        }

        public async Task<AssignmentExerciseModel?> AddExerciseAsync(Guid assignmentId, Guid sourceExerciseId)
        {
            // Needs: POST /api/assignments/{id}/exercises  body: { exerciseId }
            var resp = await _http.PostAsJsonAsync(
                $"api/assignments/{assignmentId}/exercises",
                new { exerciseId = sourceExerciseId });
            resp.EnsureSuccessStatusCode();
            return await resp.Content.ReadFromJsonAsync<AssignmentExerciseModel>();
        }

        public async Task RemoveExerciseAsync(Guid assignmentId, Guid assignmentExerciseId)
        {
            // Needs: DELETE /api/assignments/{id}/exercises/{aeId}
            var resp = await _http.DeleteAsync($"api/assignments/{assignmentId}/exercises/{assignmentExerciseId}");
            resp.EnsureSuccessStatusCode();
        }

        public async Task SetQuestionPointsAsync(Guid assignmentId, Guid assignmentExerciseId, Guid questionId, int points)
        {
            // Needs: PATCH /api/assignments/{id}/exercises/{aeId}/questions/{qId}/points
            // AND a SetQuestionPoints method on IAssignmentService
            var resp = await _http.PatchAsJsonAsync(
                $"api/assignments/{assignmentId}/exercises/{assignmentExerciseId}/questions/{questionId}/points",
                new { points });
            resp.EnsureSuccessStatusCode();
        }

        public async Task RemoveQuestionAsync(Guid assignmentId, Guid assignmentExerciseId, Guid questionId)
        {
            // Needs: DELETE /api/assignments/{id}/exercises/{aeId}/questions/{qId}
            // AND a RemoveQuestion method on IAssignmentService
            var resp = await _http.DeleteAsync(
                $"api/assignments/{assignmentId}/exercises/{assignmentExerciseId}/questions/{questionId}");
            resp.EnsureSuccessStatusCode();
        }
    }
}