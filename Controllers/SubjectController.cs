﻿using ISC_ELIB_SERVER.DTOs.Requests;
using ISC_ELIB_SERVER.Services;
using ISC_ELIB_SERVER.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ISC_ELIB_SERVER.Controllers
{
    [ApiController]
    [Route("api/subjects")]
    public class SubjectController: ControllerBase
    {
        private readonly ISubjectService _service;

        public SubjectController(ISubjectService service)
        {
            _service = service;
        }

        [HttpGet]
        public IActionResult GetSubject([FromQuery] int? page = null, [FromQuery] int? pageSize = null,
            [FromQuery] string? search = null, [FromQuery] string? sortColumn = null, [FromQuery] string? sortOrder = null)
        {
            var response = _service.GetSubject(page, pageSize, search, sortColumn, sortOrder);
            return response.Code == 0 ? Ok(response) : NotFound(response);
        }

        [HttpGet("{id}")]
        public IActionResult GetSubjectById(long id)
        {
            var response = _service.GetSubjectById(id);
            return response.Code == 0 ? Ok(response) : NotFound(response);
        }

        [HttpPost]
        public IActionResult CreateSubjectType([FromBody] SubjectRequest request)
        {
            var response = _service.CreateSubject(request);
            return response.Code == 0 ? Ok(response) : BadRequest(response);
        }

        [HttpPut("{id}")]
        public IActionResult UpdateSubject(long id, [FromBody] SubjectRequest request)
        {

            var response = _service.UpdateSubject(id, request);
            return response.Code == 0 ? Ok(response) : BadRequest(response);
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteSubject(long id)
        {
            var response = _service.DeleteSubject(id);
            return response.Code == 0 ? Ok(response) : BadRequest(response);
        }
    }
}
