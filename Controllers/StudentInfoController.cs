using ISC_ELIB_SERVER.DTOs.Requests;
using ISC_ELIB_SERVER.DTOs.Responses;
using ISC_ELIB_SERVER.Models;
using ISC_ELIB_SERVER.Services;
using ISC_ELIB_SERVER.Utils;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace ISC_ELIB_SERVER.Controllers
{
    [Route("api/studentinfos")]
    [ApiController]
    public class StudentInfoController : ControllerBase
    {
        private readonly IStudentInfoService _studentInfoService;

        public StudentInfoController(IStudentInfoService studentInfoService)
        {
            _studentInfoService = studentInfoService;
        }

        // GET: api/studentinfos
        [HttpGet]
        public ActionResult<ApiResponse<ICollection<StudentInfoResponses>>> GetStudentInfos(
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] string? search = "",
            [FromQuery] string sortColumn = "id",
            [FromQuery] string sortOrder = "asc")
        {
            return Ok(_studentInfoService.GetStudentInfos(page, pageSize, search, sortColumn, sortOrder));
        }

        // GET: api/studentinfos/{id}
        [HttpGet("{id}")]
        public ActionResult<ApiResponse<StudentInfoResponses>> GetStudentInfoById(long id)
        {
            return Ok(_studentInfoService.GetStudentInfoById(id));
        }

        // POST: api/studentinfos
        [HttpPost]
        public ActionResult<ApiResponse<StudentInfoResponses>> CreateStudentInfo([FromBody] StudentInfoRequest studentInfoRequest)
        {
            if (studentInfoRequest == null)
                return BadRequest("Invalid student info data.");

            return Ok(_studentInfoService.CreateStudentInfo(studentInfoRequest));
        }

        // PUT: api/studentinfos/{id}
        [HttpPut("{id}")]
        public ActionResult<ApiResponse<StudentInfoResponses>> UpdateStudentInfo(long id, [FromBody] StudentInfoRequest studentInfoRequest)
        {
            if (studentInfoRequest == null || id != studentInfoRequest.Id)
                return BadRequest("StudentInfo ID mismatch.");

            return Ok(_studentInfoService.UpdateStudentInfo(studentInfoRequest));
        }

        // DELETE: api/studentinfos/{id}
        [HttpDelete("{id}")]
        public ActionResult<ApiResponse<StudentInfoResponses>> DeleteStudentInfo(long id)
        {
            return Ok(_studentInfoService.DeleteStudentInfo(id));
        }
    }
}
