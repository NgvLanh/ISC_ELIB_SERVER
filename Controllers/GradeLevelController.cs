using ISC_ELIB_SERVER.Services;
using ISC_ELIB_SERVER.DTOs.Requests;
using Microsoft.AspNetCore.Mvc;

namespace ISC_ELIB_SERVER.Controllers
{
    [Route("api/grade-levels")]
    [ApiController]
    public class GradeLevelController : ControllerBase
    {
        private readonly IGradeLevelService _service;

        public GradeLevelController(IGradeLevelService service)
        {
            _service = service;
        }

        [HttpGet]
        public IActionResult GetGradeLevels([FromQuery] int? page = 1, [FromQuery] int? pageSize = 10, [FromQuery] string? sortColumn = "Id", [FromQuery] string? sortOrder = "asc")
        {
            var response = _service.GetGradeLevels(page, pageSize, sortColumn, sortOrder);
            return Ok(response);
        }

        [HttpGet("{id}")]
        public IActionResult GetGradeLevelById(long id)
        {
            var response = _service.GetGradeLevelById(id);
            return response.Code == 0 ? Ok(response) : BadRequest(response);
        }

        [HttpPost]
        public IActionResult CreateGradeLevel([FromBody] GradeLevelRequest request)
        {
            var response = _service.CreateGradeLevel(request);
            return response.Code == 0 ? Ok(response) : BadRequest(response);
        }

        [HttpPut("{id}")]
        public IActionResult UpdateGradeLevel(long id, [FromBody] GradeLevelRequest request)
        {
            var response = _service.UpdateGradeLevel(id, request);
            return response.Code == 0 ? Ok(response) : BadRequest(response);
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteGradeLevel(long id)
        {
            var response = _service.DeleteGradeLevel(id);
            return response.Code == 0 ? Ok(response) : BadRequest(response);
        }
    }
}