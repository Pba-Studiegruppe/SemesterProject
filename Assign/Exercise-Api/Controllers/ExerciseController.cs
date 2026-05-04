using Exercise_Application.DTO;
using Exercise_Application.Implementations;
using Exercise_Application.Interfaces.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Exercise_Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ExerciseController : ControllerBase
    {
        private readonly IExerciseService _exerciseService;

        public ExerciseController(IExerciseService exerciseService)
        {
            _exerciseService = exerciseService;
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateExercise([FromBody] CreateExerciseRequest request)
        {
            var createdExercise = await _exerciseService.CreateExerciseAsync(request);

            return CreatedAtAction(nameof(GetExerciseById),
                new { id = createdExercise.Id },
                createdExercise);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetExerciseById(Guid id)
        {
            var exercise = await _exerciseService.GetExerciseByIdAsync(id);
            if (exercise == null) return NotFound();
            return Ok(exercise);
        }

        [HttpGet("by-teacher/{teacherId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetExercisesByTeacherId(Guid teacherId)
        {
            var exercises = await _exerciseService.GetExercisesByTeacherIdAsync(teacherId);
            return Ok(exercises);
        }

        [HttpGet("by-keywords")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetExercisesByKeywords([FromQuery] List<Guid> keywordIds)
        {
            var exercises = await _exerciseService.GetExerciseByExerciseKeywords(keywordIds);
            return Ok(exercises);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateExercise(Guid id, [FromBody] UpdateExerciseRequest request)
        {
            var updatedExercise = await _exerciseService.UpdateExerciseAsync(request);
            if (updatedExercise == null) return NotFound();
            return Ok(updatedExercise);
        }
    }
}
