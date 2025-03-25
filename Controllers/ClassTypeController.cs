using ISC_ELIB_SERVER.DTOs.Requests;
using ISC_ELIB_SERVER.DTOs.Responses;
using ISC_ELIB_SERVER.Services;
using Microsoft.AspNetCore.Mvc;

namespace ISC_ELIB_SERVER.Controllers
{
    [ApiController]
    [Route("api/class-type")]
    public class ClassTypeController : ControllerBase
    {
        private readonly IClassTypeService _service;

        public ClassTypeController(IClassTypeService service)
        {
            _service = service;
        }

        [HttpGet]
        public IActionResult GetClassType([FromQuery] int? page = 1, [FromQuery] int? pageSize = 10, [FromQuery] string? sortColumn = "Id", [FromQuery] string? sortOrder = "asc")
        {
            var response = _service.GetClassTypes(page, pageSize, sortColumn, sortOrder);
            return Ok(response);
        }

        [HttpPost]
        public IActionResult CreateClassType([FromBody] ClassTypeRequest classTypeRequest)
        {
            var response = _service.CreateClassType(classTypeRequest);
            return response.Code == 0 ? Ok(response) : BadRequest(response);
        }

        [HttpPut("{id}")]
        public IActionResult UpdateClassType(int id, [FromBody] ClassTypeRequest classTypeRequest)
        {

            var response = _service.UpdateClassType(id, classTypeRequest);

            return response.Code == 0 ? Ok(response) : BadRequest(response);
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteClassType(int id)
        {
            var response = _service.DeleteClassType(id);

            return response.Code == 0 ? Ok(response) : BadRequest(response);
        }

        [HttpGet("{id}")]
        public IActionResult GetClassTypeById(int id)
        {
            var response = _service.GetClassTypeById(id);
            return response.Code == 0 ? Ok(response) : NotFound(response);
        }
        [HttpPost("by-name")]
        public IActionResult GetClassTypeByName([FromBody] NameRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Name))
            {
                return BadRequest(ApiResponse<ClassTypeResponse>.BadRequest("Tên không được để trống"));
            }

            var response = _service.GetClassTypeByName(request);
            return StatusCode(response.Code, response);
        }

        [HttpPost("by-school-year")]
        public IActionResult GetClassTypesBySchoolYear([FromBody] YearRequest request)
        {
            if (request == null || string.IsNullOrWhiteSpace(request.Year))
            {
                return BadRequest(ApiResponse<ICollection<ClassTypeResponse>>.BadRequest("Niên khóa không được để trống"));
            }

            var response = _service.GetClassTypesBySchoolYear(request);

            return StatusCode(response.Code, response);
        }





    }
}
