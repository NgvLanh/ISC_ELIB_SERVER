﻿using ISC_ELIB_SERVER.DTOs.Requests;
using ISC_ELIB_SERVER.DTOs.Responses;
using ISC_ELIB_SERVER.Models;
using ISC_ELIB_SERVER.Services;
using ISC_ELIB_SERVER.Services.Interfaces;
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

        // Lấy danh sách học viên theo user figma
        [HttpGet("all")]
        public IActionResult GetAllStudents()
        {
            var result = _studentInfoService.GetAllStudents();
            return Ok(result);
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
        public ActionResult<ApiResponse<StudentInfoResponses>> GetStudentInfoById(int id)
        {
            return Ok(_studentInfoService.GetStudentInfoById(id));
        }

        // POST: api/studentinfos
        [HttpPost]
        public ActionResult<ApiResponse<StudentInfoResponses>> CreateStudentInfo([FromBody] StudentInfoRequest studentInfoRequest)
        {
            if (studentInfoRequest == null)
                return BadRequest("Dữ liệu thông tin học sinh không hợp lệ.");

            return Ok(_studentInfoService.CreateStudentInfo(studentInfoRequest));
        }

        // PUT: api/studentinfos/{id}
        [HttpPut("{id}")]
        public ActionResult<ApiResponse<StudentInfoResponses>> UpdateStudentInfo(int id, [FromBody] StudentInfoRequest studentInfoRequest)
        {
            if (studentInfoRequest == null)
                return BadRequest("Dữ liệu không hợp lệ.");

            return Ok(_studentInfoService.UpdateStudentInfo(id, studentInfoRequest));
        }

        // DELETE: api/studentinfos/{id}
        [HttpDelete("{id}")]
        public ActionResult<ApiResponse<StudentInfoResponses>> DeleteStudentInfo(int id)
        {
            return Ok(_studentInfoService.DeleteStudentInfo(id));
        }

        //Lấy danh sách học sinh theo lớp
        //[HttpGet("class/{classId}")]
        //public IActionResult GetStudentsByClass(int classId, int page = 1, int pageSize = 10)
        //{
        //    var result = _studentInfoService.GetStudentInfosByClassId(classId, page, pageSize);
        //    return Ok(result);
        //}

        // Lấy danh sách sinh viên theo UserId
        [HttpGet("user/{userId}")]
        public IActionResult GetStudentsByUserId(int userId)
        {
            var result = _studentInfoService.GetStudentsByUserId(userId);
            return Ok(result);
        }

        // Lấy danh sách học viên theo ClassId với thông tin chi tiết từ User và UserStatus
        [HttpGet("class/{classId}/students")]
        public IActionResult GetStudentsByClass(int classId)
        {
            var result = _studentInfoService.GetStudentsByClass(classId);
            return Ok(result);
        }

    }
}
