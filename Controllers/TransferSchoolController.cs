using ISC_ELIB_SERVER.DTOs.Requests;
using ISC_ELIB_SERVER.DTOs.Responses;
using ISC_ELIB_SERVER.Models;
using ISC_ELIB_SERVER.Repositories;
using ISC_ELIB_SERVER.Services;
using Microsoft.AspNetCore.Mvc;
using Sprache;


namespace ISC_ELIB_SERVER.Controllers
{
    [ApiController]
    [Route("api/TransferSchool")]
    public class TransferSchoolController : ControllerBase
    {
        private readonly ITransferSchoolService _service;

        public TransferSchoolController(ITransferSchoolService service)
        {
            _service = service;
        }

        [HttpGet]
        public IActionResult GetTransferSchools([FromQuery] int page = 1, [FromQuery] int pageSize = 10,
            [FromQuery] string? search = "", [FromQuery] string sortColumn = "Id", [FromQuery] string sortOrder = "asc")
        {
            var response = _service.GetTransferSchools(page, pageSize, search, sortColumn, sortOrder);
            return Ok(response);
        }

        [HttpGet("{id}")]
        public IActionResult GetTransferSchoolById(long id)
        {
            var response = _service.GetTransferSchoolById(id);
            return response.Code == 0 ? Ok(response) : NotFound(response);
        }

        [HttpPost]
        public IActionResult CreateTransferSchool([FromBody] TransferSchool_AddRequest TransferSchoolRequest)
        {
            var response = _service.CreateTransferSchool(TransferSchoolRequest);
            return response.Code == 0 ? Ok(response) : BadRequest(response);
        }

        [HttpPut("{id}")]
        public IActionResult UpdateTransferSchool(long id, [FromBody] TransferSchool_UpdateRequest TransferSchool)
        {
            var response = _service.UpdateTransferSchool(id , TransferSchool);

            return response.Code == 0 ? Ok(response) : BadRequest(response);
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteTransferSchool(long id)
        {
            var response = _service.DeleteTransferSchool(id);
            var result = _service.GetTransferSchoolsNormal();
            return response.Code == 0 ? Ok(result) : BadRequest(result);
        }

    }
}
