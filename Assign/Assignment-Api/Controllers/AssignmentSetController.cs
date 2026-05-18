using Assignment_Application.DTO;
using Assignment_Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace Assignment_Api.Controllers
{
    [ApiController]
    [Route("api/assignmentsets")]
    public class AssignmentSetController : ControllerBase
    {
        private readonly IAssignmentSetService _service;

        public AssignmentSetController(IAssignmentSetService service)
        {
            _service = service;
        }

        // GET /api/assignmentsets
        // GET /api/assignmentsets?courseId={guid}
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll([FromQuery] Guid? courseId)
        {
            if (courseId is null || courseId == Guid.Empty)
            {
                var all = await _service.GetAllAssignmentSetsAsync();
                return Ok(all);
            }

            var filtered = await _service.GetAssignmentSetsByCourseIdAsync(courseId.Value);
            return Ok(filtered);
        }

        // GET /api/assignmentsets/{id}
        [HttpGet("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Get(Guid id)
        {
            try
            {
                var result = await _service.GetAssignmentSetsAsync(id);
                return Ok(result);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        // POST /api/assignmentsets
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create([FromBody] CreateAssignmentSetRequest request)
        {
            try
            {
                var result = await _service.CreateAssignmentSetAsync(request);
                return CreatedAtAction(nameof(Get), new { id = result.Id }, result);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // PUT /api/assignmentsets/{id}
        [HttpPut("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateAssignmentSetRequest request)
        {
            try
            {
                var result = await _service.UpdateAssignmentSetAsync(id, request);
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

        // POST /api/assignmentsets/{id}/publish
        [HttpPost("{id:guid}/publish")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> Publish(Guid id)
        {
            try
            {
                var result = await _service.PublishAssignmentSetAsync(id);
                return Ok(result);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(ex.Message);
            }
        }
    }
}
