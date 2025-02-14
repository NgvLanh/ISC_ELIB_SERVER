using ISC_ELIB_SERVER.DTOs.Requests;
using ISC_ELIB_SERVER.Services;
using Microsoft.AspNetCore.Mvc;

namespace ISC_ELIB_SERVER.Controllers
{
    [Route("api/test-submission-answer")]
    [ApiController]
    public class TestSubmissionAnswerController : Controller
    {
        private readonly ITestSubmissionAnswerService _service;

        public TestSubmissionAnswerController(ITestSubmissionAnswerService service)
        {
            _service = service;
        }

        [HttpGet]
        public IActionResult GetTestSubmissionAnswers
        (
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] string? search = "",
            [FromQuery] string sortColumn = "id",
            [FromQuery] string sortOrder = "asc"
        )
        {
            Console.WriteLine($"API called with params - Page: {page}, PageSize: {pageSize}, Search: {search}, SortColumn: {sortColumn}, SortOrder: {sortOrder}");

            var response = _service.GetTestSubmissionAnswers(page, pageSize, search, sortColumn, sortOrder);

            Console.WriteLine($"API response: {response.Data.Count} records returned");

            return Ok(response);
        }

        [HttpGet("{id}")]
        public IActionResult GetTestSubmissionAnswerById(int id)
        {
            var response = _service.GetTestSubmissionAnswerById(id);
            return response.Code == 0 ? Ok(response) : NotFound(response);
        }

        [HttpPost]
        public IActionResult CreateTestSubmissionAnswer([FromBody] TestSubmissionAnswerRequest request)
        {
            var response = _service.CreateTestSubmissionAnswer(request);

            if (response.Code == 0) // Success
                return Ok(response);

            if (response.Message.Contains("Conflict"))
                return Conflict(response);

            return BadRequest(response);
        }

        [HttpPut("{id}")]
        public IActionResult UpdateTestSubmissionAnswer(int id, [FromBody] TestSubmissionAnswerRequest request)
        {
            var response = _service.UpdateTestSubmissionAnswer(id, request);
            return response.Code == 0 ? Ok(response) : BadRequest(response);
        }

        [HttpDelete("{id}")]    
        public IActionResult DeleteTestSubmissionAnswer(int id)
        {
            var response = _service.DeleteTestSubmissionAnswer(id);
            return response.Code == 0 ? Ok(response) : NotFound(response);
        }
    }
}
