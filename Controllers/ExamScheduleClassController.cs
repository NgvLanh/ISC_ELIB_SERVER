using ISC_ELIB_SERVER.DTOs.Requests;
using ISC_ELIB_SERVER.DTOs.Responses;
using ISC_ELIB_SERVER.Services;
using Microsoft.AspNetCore.Mvc;

namespace ISC_ELIB_SERVER.Controllers
{
    [Route("api/exam-schedule-classes")]
    [ApiController]
    public class ExamScheduleClassController : ControllerBase
    {
        private readonly ExamScheduleClassService _service;

        public ExamScheduleClassController(ExamScheduleClassService service)
        {
            _service = service;
        }

        [HttpGet]
        public ActionResult<ApiResponse<ICollection<ExamScheduleClassResponse>>> GetAll()
        {
            return Ok(_service.GetAll());
        }

        [HttpGet("{id}")]
        public ActionResult<ApiResponse<ExamScheduleClassResponse>> GetById(long id)
        {
            var response = _service.GetById(id);
            if (response.Code == 1) return NotFound(response);
            return Ok(response);
        }

        [HttpPost]
        public ActionResult<ApiResponse<ExamScheduleClassResponse>> Create([FromBody] ExamScheduleClassRequest request)
        {
            var response = _service.Create(request);
            return CreatedAtAction(nameof(GetById), new { id = response.Data?.Id }, response);
        }

        [HttpPut("{id}")]
        public ActionResult<ApiResponse<ExamScheduleClassResponse>> Update(long id, [FromBody] ExamScheduleClassRequest request)
        {
            var response = _service.Update(id, request);
            if (response.Code == 1) return NotFound(response);
            return Ok(response);
        }

        [HttpDelete("{id}")]
        public ActionResult<ApiResponse<object>> Delete(long id)
        {
            var response = _service.Delete(id);
            if (response.Code == 1) return NotFound(response);
            return Ok(response);
        }
    }
}
