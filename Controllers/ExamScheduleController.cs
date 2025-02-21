using ISC_ELIB_SERVER.DTOs.Requests;
using ISC_ELIB_SERVER.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ISC_ELIB_SERVER.Controllers
{
    [Route("api/exam-schedules")]
    [ApiController]
    public class ExamScheduleController : Controller
    {
        private readonly IExamScheduleService _service;

        public ExamScheduleController(IExamScheduleService service)
        {
            _service = service;
        }

        [HttpGet]
        public IActionResult GetAllExamSchedules(
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] string? search = "",
            [FromQuery] string sortColumn = "id",
            [FromQuery] string sortOrder = "asc"
        )
        {
            var response = _service.GetExamSchedules(page, pageSize, search, sortColumn, sortOrder);
            return Ok(response);
        }

        [HttpGet("{id}")]
        public IActionResult GetExamScheduleById(int id)
        {
            var response = _service.GetExamScheduleById(id);
            if (response.Code == 0)
            {
                return Ok(response);
            }
            return NotFound(response);
        }

        [HttpGet("name/{name}")]
        public IActionResult GetExamScheduleByName(string name)
        {
            var response = _service.GetExamScheduleByName(name);
            if (response.Code == 0)
            {
                return Ok(response);
            }
            return NotFound(response);
        }

        [HttpPost]
        public IActionResult CreateExamSchedule([FromBody] ExamScheduleRequest request)
        {
            var response = _service.CreateExamSchedule(request);
            if (response.Code == 0)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

        [HttpPut("{id}")]
        public IActionResult UpdateExamSchedule(int id, [FromBody] ExamScheduleRequest request)
        {
            var response = _service.UpdateExamSchedule(id, request);
            if (response.Code == 0)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

        [HttpPut("{id}/toggle-active")]
        public IActionResult DeleteExamSchedule(int id)
        {
            var response = _service.DeleteExamSchedule(id);
            return response.Code == 0 ? Ok(response) : NotFound(response);
        }
    }
}
