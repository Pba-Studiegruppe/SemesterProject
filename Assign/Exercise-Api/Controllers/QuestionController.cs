using Exercise_Application.DTO;
using Exercise_Application.Interfaces.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Exercise_Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QuestionController : ControllerBase
    {
        private readonly IQuestionService _questionService;

        public QuestionController(IQuestionService questionService)
        {
            _questionService = questionService;
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateQuestion([FromBody] CreateQuestionRequest request)
        {
            var CreatedQuestion = await _questionService.CreateQuestionAsync(request);

            return CreatedAtAction(nameof(GetQuestionById), new { id = CreatedQuestion.Id }, CreatedQuestion);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetQuestionById(Guid id)
        {
            var question = await _questionService.GetQuestionByIdAsync(id);
            if (question == null) { return NotFound(); }
            return Ok(question);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateQuestion(Guid id, [FromBody] UpdateQuestionRequest request)
        {
            var updatedQuestion = await _questionService.UpdateQuestionAsync(request);
            if (updatedQuestion == null) { return NotFound(); }
            return Ok(updatedQuestion);
        }
    }
}