using Exercise_Application.DTO;
using Exercise_Application.Interfaces.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
namespace Exercise_Api.Controllers
{
    public class QuestionSolutionController : Controller
    {
        private readonly IQuestionSolutionService _questionSolutionService;

        public QuestionSolutionController(IQuestionSolutionService questionSolutionService)
        {
            _questionSolutionService = questionSolutionService;
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateQuestionSolution([FromBody] CreateQuestionSolutionRequest dto)
        {
            var createdSolution = await _questionSolutionService.CreateQuestionSolutionAsync(dto);

            if (createdSolution == null)
            {
                return BadRequest("Failed to create question solution.");
            }

            return CreatedAtAction(nameof(GetQuestionSolution), new { id = createdSolution.Id }, createdSolution);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetQuestionSolution(Guid id)
        {
            var solution = await _questionSolutionService.GetQuestionSolutionByIdAsync(id);
            if (solution == null) { return NotFound(); }
            return Ok(solution);
        }
    }
}
