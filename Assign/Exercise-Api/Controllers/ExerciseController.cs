using Exercise_Application.DTO;
using Exercise_Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace Exercise_Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ExerciseController : ControllerBase
    {
        private readonly IExerciseService _exerciseService;
        private readonly IExerciseSnapshotQueryService _snapshotQuery;
        private readonly IExerciseReviewQueryService _reviewQuery;

        public ExerciseController(
            IExerciseService exerciseService,
            IExerciseSnapshotQueryService snapshotQuery,
            IExerciseReviewQueryService reviewQuery)
        {
            _exerciseService = exerciseService;
            _snapshotQuery = snapshotQuery;
            _reviewQuery = reviewQuery;
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateExercise([FromBody] CreateExerciseRequest request)
        {
            var created = await _exerciseService.CreateExerciseAsync(request);
            return CreatedAtAction(nameof(GetReview), new { id = created.Id }, created);
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
            var updated = await _exerciseService.UpdateExerciseAsync(request);
            if (updated is null) return NotFound();
            return Ok(updated);
        }

        [HttpGet("{id}/snapshot")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetSnapshot(Guid id)
        {
            var projection = await _snapshotQuery.GetForSnapshotAsync(id);
            if (projection is null) return NotFound();
            return Ok(projection);
        }

        [HttpGet("{id}/review")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetReview(Guid id)
        {
            var projection = await _reviewQuery.GetForReviewAsync(id);
            if (projection is null) return NotFound();
            return Ok(projection);
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllExercises([FromQuery] string? search = null)
        {
            var exercises = await _exerciseService.GetAllExercisesAsync(search);
            return Ok(exercises);
        }
    }
}