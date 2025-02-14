using ISC_ELIB_SERVER.DTOs.Requests;
using ISC_ELIB_SERVER.Services;
using Microsoft.AspNetCore.Mvc;

namespace ISC_ELIB_SERVER.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExamScheduleController : ControllerBase
    {
        private readonly ExamScheduleService _service;

        public ExamScheduleController(ExamScheduleService service)
        {
            _service = service;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var response = _service.GetAll();
            return StatusCode(response.Code == 0 ? 200 : 400, response);
        }

        [HttpGet("{id}")]
        public IActionResult GetById(long id)
        {
            var response = _service.GetById(id);
            return StatusCode(response.Code == 0 ? 200 : 404, response);
        }

        [HttpPost]
        public IActionResult Create([FromBody] ExamScheduleRequest request)
        {
            var response = _service.Create(request);
            return StatusCode(response.Code == 0 ? 201 : 400, response);
        }

        [HttpPut("{id}")]
        public IActionResult Update(long id, [FromBody] ExamScheduleRequest request)
        {
            var response = _service.Update(id, request);
            return StatusCode(response.Code == 0 ? 200 : 404, response);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(long id)
        {
            var response = _service.Delete(id);
            return StatusCode(response.Code == 0 ? 200 : 404, response);
        }
    }
}
