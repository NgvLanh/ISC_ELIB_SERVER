using ISC_ELIB_SERVER.Services;
using ISC_ELIB_SERVER.DTOs.Requests;
using Microsoft.AspNetCore.Mvc;

namespace ISC_ELIB_SERVER.Controllers
{
    [Route("api/schools")]
    [ApiController]
    public class SchoolController : ControllerBase
    {
        private readonly ISchoolService _service;

        public SchoolController(ISchoolService service)
        {
            _service = service;
        }

        [HttpGet]
        public IActionResult GetSchools([FromQuery] int? page = 1, [FromQuery] int? pageSize = 10, [FromQuery] string? search = "", [FromQuery] string? sortColumn = "Id", [FromQuery] string? sortOrder = "asc")
        {
            var response = _service.GetSchools(page, pageSize, search, sortColumn, sortOrder);
            return Ok(response);
        }

        [HttpGet("{id}")]
        public IActionResult GetSchoolById(long id)
        {
            var response = _service.GetSchoolById(id);
            return response.Code == 0 ? Ok(response) : BadRequest(response);
        }

        [HttpPost]
        public IActionResult CreateSchool([FromBody] SchoolRequest request)
        {
            var response = _service.CreateSchool(request);
            return response.Code == 0 ? Ok(response) : BadRequest(response);
        }

        [HttpPut("{id}")]
        public IActionResult UpdateSchool(long id, [FromBody] SchoolRequest request)
        {
            var response = _service.UpdateSchool(id, request);
            return response.Code == 0 ? Ok(response) : BadRequest(response);
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteSchool(long id)
        {
            var response = _service.DeleteSchool(id);
            return response.Code == 0 ? Ok(response) : BadRequest(response);
        }
    }
}
