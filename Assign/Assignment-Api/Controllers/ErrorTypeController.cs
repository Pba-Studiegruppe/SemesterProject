using Assignment_Application.DTO;
using Assignment_Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace Assignment_Api.Controllers
{
    [ApiController]
    [Route("api/error-types")]
    public class ErrorTypeController : ControllerBase
    {
        private readonly IErrorTypeService _service;

        public ErrorTypeController(IErrorTypeService service)
        {
            _service = service;
        }

        // GET /api/error-types
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll()
        {
            var result = await _service.GetAllErrorTypesAsync();
            return Ok(result);
        }

        // GET /api/error-types/{id}
        [HttpGet("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Get(Guid id)
        {
            try
            {
                var result = await _service.GetErrorTypeAsync(id);
                return Ok(result);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        // POST /api/error-types
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create([FromBody] CreateErrorTypeRequest request)
        {
            try
            {
                var result = await _service.CreateErrorTypeAsync(request);
                return CreatedAtAction(nameof(Get), new { id = result.Id }, result);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // PUT /api/error-types/{id}
        [HttpPut("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateErrorTypeRequest request)
        {
            try
            {
                var result = await _service.UpdateErrorTypeAsync(id, request);
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
    }
}
