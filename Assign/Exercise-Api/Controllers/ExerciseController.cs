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
        private readonly IExerciseSnapshotQueryService _snapshotQuery;
        private readonly IExerciseEvaluationQueryService _evaluationQuery;
        public ExerciseController(
            IExerciseService exerciseService,
            IExerciseSnapshotQueryService snapshotQuery,
            IExerciseEvaluationQueryService evaluationQuery)
        {
            _exerciseService = exerciseService;
            _snapshotQuery = snapshotQuery;
            _evaluationQuery = evaluationQuery;
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

        [HttpGet("{id}/snapshot")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetSnapshot(Guid id)
        {
            var projection = await _snapshotQuery.GetForSnapshotAsync(id);
            if (projection is null) return NotFound();
            return Ok(projection);
        }

        [HttpGet("{id}/evaluation")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetEvaluation(Guid id, [FromQuery] Guid teacherId)
        {
            try
            {
                var projection = await _evaluationQuery.GetForEvaluationAsync(id, teacherId);
                if (projection is null) return NotFound();
                return Ok(projection);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Forbid(ex.Message);
            }
        }
    }
}
