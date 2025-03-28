﻿using ISC_ELIB_SERVER.DTOs.Requests;
using ISC_ELIB_SERVER.DTOs.Responses;
using ISC_ELIB_SERVER.Models;
using ISC_ELIB_SERVER.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;

namespace ISC_ELIB_SERVER.Controllers
{
    [ApiController]
    [Route("api/work-process")]
    public class WorkProcessController : ControllerBase
    {
        private readonly IWorkProcessService _service;
        public WorkProcessController(IWorkProcessService service)
        {
            _service = service;
        }

        [HttpGet()]
        public IActionResult GetWorkProcess([FromQuery] int page = 1, [FromQuery] int pageSize = 10,
            [FromQuery] string? search = "", [FromQuery] string sortColumn = "Id", [FromQuery] string sortOrder = "asc")
        {
            var response = _service.GetWorkProcess(page, pageSize, search, sortColumn, sortOrder);
            return Ok(response);
        }

        [HttpGet("getworkprocessnopaging")]
        public IActionResult GetWorkProcessNoPaging()
        {
            var response = _service.GetWorkProcessNoPaging();
            return Ok(response);
        }
        [HttpPost]
        public IActionResult CreateWorkProcess([FromBody] WorkProcessRequest workProcessRequest)
        {
            var response = _service.CreateWorkProcess(workProcessRequest);
            return response.Code == 0 ? Ok(response) : BadRequest(response);
        }

        
        [HttpGet("{id}")]
        public IActionResult GetWorkProcessById(long id)
        {
            var response = _service.GetWorkProcessById(id);
            return response.Code == 0 ? Ok(response) : NotFound(response);
        }
        [HttpPut("{id}")]
        public IActionResult UpdateWorkProcess(long id, [FromBody] WorkProcessRequest workProcess_UpdateRequest)
        {
            var response = _service.UpdateWorkProcess(id, workProcess_UpdateRequest);

            if (response.Code == 0)
            {
                return Ok(response);
            }
            else
            {
                return NotFound(response);
            }
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteUserStatus(long id)
        {
            var response = _service.DeleteWorkProcess(id);
            return response.Code == 0 ? Ok(response) : NotFound(response);
        }
    }
}
