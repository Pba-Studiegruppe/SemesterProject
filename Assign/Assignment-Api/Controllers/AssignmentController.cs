using Assignment_Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace Assignment_Api.Controllers
{
    public class AssignmentController : Controller
    {
        private readonly IAssignmentPdfService _pdfService;

        public AssignmentController(IAssignmentPdfService pdfService)
        {
            _pdfService = pdfService;
        }

        [HttpGet("{id}/pdf")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetPdf(Guid id)
        {
            try
            {
                var bytes = await _pdfService.GenerateAsync(id);
                return File(bytes, "application/pdf", $"assignment-{id:D}.pdf");
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }
    }
}
