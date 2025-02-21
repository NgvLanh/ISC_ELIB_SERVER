using ISC_ELIB_SERVER.DTOs.Requests;
using ISC_ELIB_SERVER.Requests;
using ISC_ELIB_SERVER.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ISC_ELIB_SERVER.Controllers
{
    [Route("api/exam-schedule-classes")]
    [ApiController]
    public class ExamScheduleClassController : Controller
    {
        private readonly IExamScheduleClassService _service;

        public ExamScheduleClassController(IExamScheduleClassService service)
        {
            _service = service;
        }

        [HttpGet]
        public IActionResult GetAllExamScheduleClasses(
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] int? classId = null,
            [FromQuery] int? exampleSchedule = null,
            [FromQuery] int? supervisoryTeacherId = null,
            [FromQuery] string sortColumn = "id",
            [FromQuery] string sortOrder = "asc"
        )
        {
            var response = _service.GetExamScheduleClasses(page, pageSize, classId, exampleSchedule, supervisoryTeacherId, sortColumn, sortOrder);
            return Ok(response);
        }

        [HttpGet("{id}")]
        public IActionResult GetExamScheduleClassById(int id)
        {
            var response = _service.GetExamScheduleClassById(id);
            if (response.Code == 0)
                return Ok(response);
            return NotFound(response);
        }

        [HttpGet("filter")]
        public IActionResult GetExamScheduleClassByFilter(
            [FromQuery] int? classId = null,
            [FromQuery] int? exampleSchedule = null,
            [FromQuery] int? supervisoryTeacherId = null
        )
        {
            var response = _service.GetExamScheduleClassByFilter(classId, exampleSchedule, supervisoryTeacherId);
            if (response.Code == 0)
                return Ok(response);
            return NotFound(response);
        }

        [HttpPost]
        public IActionResult CreateExamScheduleClass([FromBody] ExamScheduleClassRequest request)
        {
            var response = _service.CreateExamScheduleClass(request);
            if (response.Code == 0)
                return CreatedAtAction(nameof(GetExamScheduleClassById), new { id = response.Data.Id }, response);
            return BadRequest(response);
        }

        [HttpPut("{id}")]
        public IActionResult UpdateExamScheduleClass(int id, [FromBody] ExamScheduleClassRequest request)
        {
            var response = _service.UpdateExamScheduleClass(id, request);
            if (response.Code == 0)
                return Ok(response);
            return BadRequest(response);
        }

        [HttpPut("{id}/toggle-active")]
        public IActionResult DeleteExamScheduleClass(int id)
        {
            var response = _service.DeleteExamScheduleClass(id);
            return response.Code == 0 ? Ok(response) : NotFound(response);
        }
    }
}
