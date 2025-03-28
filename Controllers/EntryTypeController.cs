using ISC_ELIB_SERVER.DTOs.Requests;
using ISC_ELIB_SERVER.DTOs.Responses;
using ISC_ELIB_SERVER.Services;
using Microsoft.AspNetCore.Mvc;

namespace ISC_ELIB_SERVER.Controllers
{
    [ApiController]
    [Route("api/entry-types")]
    public class EntryTypeController : ControllerBase
    {
        private readonly IEntryTypeService _service;

        public EntryTypeController(IEntryTypeService service)
        {
            _service = service;
        }

        [HttpGet]
        public IActionResult GetEntryTypes([FromQuery] int page = 1, [FromQuery] int pageSize = 10,
            [FromQuery] string? search = "", [FromQuery] string sortColumn = "Id", [FromQuery] string sortOrder = "asc")
        {
            var response = _service.GetEntryTypes(page, pageSize, search, sortColumn, sortOrder);
            return Ok(response);
        }

        [HttpGet("{id}")]
        public IActionResult GetEntryTypeById(long id)
        {
            var response = _service.GetEntryTypeById(id);
            return response.Code == 0 ? Ok(response) : NotFound(response);
        }

        [HttpPost]
        public IActionResult CreateEntryType([FromBody] EntryTypeRequest entryTypeRequest)
        {
            var response = _service.CreateEntryType(entryTypeRequest);
            return response.Code == 0 ? Ok(response) : BadRequest(response);
        }

        [HttpPut("{id}")]
        public IActionResult UpdateEntryType(long id, [FromBody] EntryTypeRequest entryTypeRequest)
        {
            return Ok(ApiResponse<object>.Success("Chưa làm"));
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteEntryType(long id)
        {
            return Ok(ApiResponse<object>.Success("Chưa làm"));
        }
    }
}
