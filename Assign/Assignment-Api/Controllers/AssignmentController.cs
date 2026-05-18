using Assignment_Application.DTO;
using Assignment_Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace Assignment_Api.Controllers
{
    [ApiController]
    [Route("api/assignments")]
    public class AssignmentController : ControllerBase
    {
        private readonly IAssignmentService _service;
        private readonly IAssignmentPdfService _pdfService;

        public AssignmentController(IAssignmentService service, IAssignmentPdfService pdfService)
        {
            _service = service;
            _pdfService = pdfService;
        }

        // GET /api/assignments/{id}
        [HttpGet("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetAssignment(Guid id)
        {
            try
            {
                var result = await _service.GetAssignmentAsync(id);
                return Ok(result);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        // POST /api/assignments
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> CreateAssignment([FromBody] CreateAssignmentRequest request)
        {
            try
            {
                var result = await _service.CreateAssignmentAsync(request);
                return CreatedAtAction(nameof(GetAssignment), new { id = result.Id }, result);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);   // AssignmentSetId pointed to a non-existent set
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(ex.Message);   // Set is published, can't add to it
            }
        }

        // PUT /api/assignments/{id}
        [HttpPut("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateAssignment(Guid id, [FromBody] UpdateAssignmentRequest request)
        {
            try
            {
                var result = await _service.UpdateAssignmentAsync(id, request);
                return Ok(result);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // POST /api/assignments/{id}/exercises
        [HttpPost("{id:guid}/exercises")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> AddExercise(Guid id, [FromBody] CreateAssignmentExerciseRequest request)
        {
            try
            {
                var result = await _service.AddExerciseAsync(id, request);
                return Ok(result);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                // Duplicate exercise — domain throws this
                return Conflict(ex.Message);
            }
        }

        // DELETE /api/assignments/{id}/exercises/{assignmentExerciseId}
        [HttpDelete("{id:guid}/exercises/{assignmentExerciseId:guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> RemoveExercise(Guid id, Guid assignmentExerciseId)
        {
            try
            {
                await _service.RemoveExerciseAsync(id, new RemoveAssignmentExerciseRequest
                {
                    AssignmentExerciseId = assignmentExerciseId
                });
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        // PATCH /api/assignments/{id}/exercises/{aeId}/questions/{questionId}/points
        [HttpPatch("{id:guid}/exercises/{assignmentExerciseId:guid}/questions/{questionId:guid}/points")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> SetQuestionPoints(
            Guid id,
            Guid assignmentExerciseId,
            Guid questionId,
            [FromBody] SetQuestionPointsRequest request)
        {
            try
            {
                await _service.SetQuestionPointsAsync(id, assignmentExerciseId, questionId, request);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
            catch (ArgumentOutOfRangeException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // DELETE /api/assignments/{id}/exercises/{aeId}/questions/{questionId}
        [HttpDelete("{id:guid}/exercises/{assignmentExerciseId:guid}/questions/{questionId:guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> RemoveQuestion(Guid id, Guid assignmentExerciseId, Guid questionId)
        {
            try
            {
                await _service.RemoveQuestionAsync(id, assignmentExerciseId, questionId);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        // GET /api/assignments/{id}/pdf
        [HttpGet("{id:guid}/pdf")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetPdf(Guid id)
        {
            try
            {
                var bytes = await _pdfService.GenerateAsync(id);
                return File(bytes, "application/pdf", $"assignment-{id:D}.pdf");
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }
    }
}