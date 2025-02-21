using ISC_ELIB_SERVER.DTOs.Requests;
using ISC_ELIB_SERVER.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ISC_ELIB_SERVER.Controllers
{
    [Route("api/exam-graders")]
    [ApiController]
    public class ExamGraderController : Controller
    {
        private readonly IExamGraderService _service;

        public ExamGraderController(IExamGraderService service)
        {
            _service = service;
        }

        [HttpGet]
        public IActionResult GetAllExamGraders(
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] string? search = "",
            [FromQuery] string sortColumn = "id",
            [FromQuery] string sortOrder = "asc")
        {
            var response = _service.GetExamGraders(page, pageSize, search, sortColumn, sortOrder);
            return Ok(response);
        }

        [HttpGet("{id}")]
        public IActionResult GetExamGraderById(int id)
        {
            var response = _service.GetExamGraderById(id);
            if (response.Code == 0)
                return Ok(response);
            return NotFound(response);
        }

        [HttpGet("exam/{examId}")]
        public IActionResult GetExamGraderByExamId(int examId)
        {
            var response = _service.GetExamGraderByExamId(examId);
            if (response.Code == 0)
                return Ok(response);
            return NotFound(response);
        }

        [HttpPost]
        public IActionResult CreateExamGrader([FromBody] ExamGraderRequest request)
        {
            var response = _service.CreateExamGrader(request);
            if (response.Code == 0)
                return CreatedAtAction(nameof(GetExamGraderById), new { id = response.Data.Id }, response);
            return BadRequest(response);
        }

        [HttpPut("{id}")]
        public IActionResult UpdateExamGrader(int id, [FromBody] ExamGraderRequest request)
        {
            var response = _service.UpdateExamGrader(id, request);
            if (response.Code == 0)
                return Ok(response);
            return BadRequest(response);
        }

        [HttpPut("{id}/toggle-active")]
        public IActionResult DeleteExamGrader(int id)
        {
            var response = _service.DeleteExamGrader(id);
            return response.Code == 0 ? Ok(response) : NotFound(response);
        }
    }
}
