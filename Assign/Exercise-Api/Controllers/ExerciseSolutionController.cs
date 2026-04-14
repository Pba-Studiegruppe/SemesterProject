using Exercise_Application.DTO;
using Exercise_Application.Implementations;
using Exercise_Application.Interfaces.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Exercise_Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExerciseSolutionController : ControllerBase
    {

        private readonly IExerciseSolutionService _exerciseSolutionService;
        public ExerciseSolutionController(IExerciseSolutionService exerciseSolutionService)
        {
            _exerciseSolutionService = exerciseSolutionService;
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateExerciseSolution([FromBody] CreateExerciseSolutionRequest dto)
        {
            try
            {
                var createdSolution = await _exerciseSolutionService.CreateExerciseSolutionAsync(dto);
                return CreatedAtAction(nameof(GetExerciseSolutionById), new { id = createdSolution.Id }, createdSolution);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetExerciseSolutionById(Guid id)
        {
            try
            {
                var solution = await _exerciseSolutionService.GetExerciseSolutionByIdAsync(id);
                if (solution == null)return NotFound();
                return Ok(solution);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("exercise/{exerciseId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetExerciseSolutionsByExerciseId(Guid exerciseId)
        {
            try
            {
                var solutions = await _exerciseSolutionService.GetExerciseSolutionsByExerciseIdAsync(exerciseId);
                return Ok(solutions);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }


        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateExerciseSolution(Guid id, [FromBody] UpdateExerciseSolutionRequest request)
        {
            try
            {
                var result = await _exerciseSolutionService.UpdateExerciseSolutionAsync(request);
                if (result == null) return NotFound();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
