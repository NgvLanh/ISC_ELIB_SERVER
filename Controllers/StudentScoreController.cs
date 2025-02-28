using ISC_ELIB_SERVER.DTOs.Requests;
using ISC_ELIB_SERVER.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ISC_ELIB_SERVER.Controllers
{
    [ApiController]
    [Route("api/studen-score")]
    public class StudentScoreController : ControllerBase
    {
        private readonly IStudentScoreService _service;

        public StudentScoreController(IStudentScoreService service)
        {
            _service = service;
        }

        [HttpGet]
        public IActionResult GetStudentScore([FromQuery] int page = 1, [FromQuery] int pageSize = 10,
            [FromQuery] string? search = "", [FromQuery] string sortColumn = "Id", [FromQuery] string sortOrder = "asc")
        {
            var response = _service.GetStudentScores(page, pageSize, search, sortColumn, sortOrder);
            return Ok(response);
        }


        [HttpGet("{id}")]
        public IActionResult GetStudentScoreById(long id)
        {
            var response = _service.GetStudentScoreById(id);
            return response.Code == 0 ? Ok(response) : NotFound(response);
        }

        [HttpPost]
        public IActionResult CreateStudentScore([FromBody] StudentScoreRequest studentScoreRequest)
        {
            var response = _service.CreateStudentScore(studentScoreRequest);
            return response.Code == 0 ? Ok(response) : BadRequest(response);
        }

        [HttpPut("{id}")]
        public IActionResult UpdateStudentScore(long id, [FromBody] StudentScoreRequest studentScoreRequest)
        {

            var response = _service.UpdateStudentScore(id, studentScoreRequest);

            return response.Code == 0 ? Ok(response) : BadRequest(response);
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteStudentScore(long id)
        {
            var response = _service.DeleteStudentScore(id);

            return response.Code == 0 ? Ok(response) : NotFound(response);
        }
    }
}
