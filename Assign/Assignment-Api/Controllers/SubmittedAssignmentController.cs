using Assignment_Application.DTO;
using Assignment_Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace Assignment_Api.Controllers
{
    [ApiController]
    [Route("api/submitted-assignments")]
    public class SubmittedAssignmentController : ControllerBase
    {
        private readonly ISubmittedAssignmentService _service;

        public SubmittedAssignmentController(ISubmittedAssignmentService service)
        {
            _service = service;
        }

        // GET /api/submitted-assignments/{id}
        [HttpGet("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetSubmittedAssignment(Guid id)
        {
            try
            {
                var result = await _service.GetSubmittedAssignmentAsync(id);
                return Ok(result);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        // GET /api/submitted-assignments?assignmentId={guid}
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetByAssignment([FromQuery] Guid? assignmentId)
        {
            if (assignmentId is null || assignmentId == Guid.Empty)
                return BadRequest("assignmentId query parameter is required.");

            try
            {
                var result = await _service.GetSubmittedAssignmentsByAssignmentAsync(assignmentId.Value);
                return Ok(result);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        // POST /api/submitted-assignments
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Create([FromBody] CreateSubmittedAssignmentRequest request)
        {
            try
            {
                var result = await _service.CreateSubmittedAssignmentAsync(request);
                return CreatedAtAction(nameof(GetSubmittedAssignment), new { id = result.Id }, result);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // PATCH /api/submitted-assignments/{id}/questions/{submittedQuestionId}/score
        [HttpPatch("{id:guid}/questions/{submittedQuestionId:guid}/score")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> ScoreQuestion(
            Guid id,
            Guid submittedQuestionId,
            [FromBody] ScoreQuestionRequest request)
        {
            try
            {
                var result = await _service.ScoreQuestionAsync(id, submittedQuestionId, request);
                return Ok(result);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (ArgumentException ex)
            {
                // covers ArgumentNullException (null request) and
                // ArgumentOutOfRangeException (invalid points)
                return BadRequest(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                // submission is in Returned state
                return Conflict(ex.Message);
            }
        }

        // PATCH /api/submitted-assignments/{id}/exercises/{submittedExerciseId}/comment
        [HttpPatch("{id:guid}/exercises/{submittedExerciseId:guid}/comment")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> SetExerciseComment(
            Guid id,
            Guid submittedExerciseId,
            [FromBody] SetExerciseCommentRequest request)
        {
            try
            {
                var result = await _service.SetExerciseCommentAsync(id, submittedExerciseId, request);
                return Ok(result);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(ex.Message);
            }
        }

        // POST /api/submitted-assignments/{id}/evaluate
        [HttpPost("{id:guid}/evaluate")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> MarkEvaluated(
            Guid id,
            [FromBody] MarkEvaluatedRequest request)
        {
            try
            {
                var result = await _service.MarkEvaluatedAsync(id, request);
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
            catch (InvalidOperationException ex)
            {
                // either unscored questions remain, or status isn't Pending/InProgress
                return Conflict(ex.Message);
            }
        }

        // POST /api/submitted-assignments/{id}/return
        [HttpPost("{id:guid}/return")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> ReturnSubmission(Guid id)
        {
            try
            {
                var result = await _service.ReturnSubmissionAsync(id);
                return Ok(result);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
            catch (InvalidOperationException ex)
            {
                // only Completed submissions can be returned
                return Conflict(ex.Message);
            }
        }
    }
}
