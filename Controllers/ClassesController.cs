using ISC_ELIB_SERVER.DTOs.Requests;
using ISC_ELIB_SERVER.Services;
using Microsoft.AspNetCore.Mvc;

namespace ISC_ELIB_SERVER.Controllers
{

    [ApiController]
    [Route("api/class")]
    public class ClassesController : ControllerBase
    {
        private readonly IClassesService _service;

        public ClassesController(IClassesService service)
        {
            _service = service;
        }


        [HttpGet]
        public IActionResult GetClass([FromQuery] int? page = 1, [FromQuery] int? pageSize = 10, [FromQuery] string? sortColumn = "Id", [FromQuery] string? sortOrder = "asc")
        {
            var response = _service.GetClass(page, pageSize, sortColumn, sortOrder);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> CreateClass([FromBody] ClassesRequest classesRequest)
        {
            if (classesRequest == null)
            {
                return BadRequest(new { Code = 1, Message = "Dữ liệu không hợp lệ" });
            }

            var response = await _service.CreateClassAsync(classesRequest);
            return response.Code == 0 ? Ok(response) : BadRequest(response);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateClass([FromRoute] int id, [FromBody] ClassesRequest classesRequest)
        {
            if (classesRequest == null)
            {
                return BadRequest(new { Code = 1, Message = "Dữ liệu không hợp lệ" });
            }

            var response = await _service.UpdateClassAsync(id, classesRequest);
            return response.Code == 0 ? Ok(response) : BadRequest(response);
        }



        [HttpDelete("{id}")]
        public IActionResult DeleteClass(int id)
        {
            var response = _service.DeleteClass(id);

            return response.Code == 0 ? Ok(response) : BadRequest(response);
        }
        [HttpGet("{id}")]
        public IActionResult GetClassById(int id)
        {
            var response = _service.GetClassById(id);
            return response.Code == 0 ? Ok(response) : NotFound(response);
            }
        }
    }
