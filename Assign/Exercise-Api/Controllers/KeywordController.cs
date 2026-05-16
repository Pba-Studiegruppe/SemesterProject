using Exercise_Application.Interfaces.Services;
using Exercise_Application.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Exercise_Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class KeywordController : ControllerBase
    {
        private readonly IKeywordService _keywordService;

        public KeywordController(IKeywordService keywordService)
        {
            _keywordService = keywordService;
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateKeyword([FromBody] CreateKeywordRequest dto)
        {
            try
            {
                var createdKeyword = await _keywordService.CreateKeywordAsync(dto);
                return CreatedAtAction(nameof(GetKeyword), new { id = createdKeyword.Id }, createdKeyword);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetKeyword(Guid id)
        {
            var keyword = await _keywordService.GetKeywordAsync(id);
            if (keyword == null)
            {
                return NotFound(new { message = "Keyword not found." });
            }
            return Ok(keyword);
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllKeywords()
        {
            var keywords = await _keywordService.GetAllKeywordsAsync();
            return Ok(keywords);


        }
    }
}
