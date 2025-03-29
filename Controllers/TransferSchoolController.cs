using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using ISC_ELIB_SERVER.Repositories;
using ISC_ELIB_SERVER.Models;
using Autofac.Core;
using ISC_ELIB_SERVER.DTOs.Requests;
using ISC_ELIB_SERVER.Services.Interfaces;
using ISC_ELIB_SERVER.DTOs.Requests.ISC_ELIB_SERVER.DTOs.Requests;

[Route("api/transfer-school")]
[ApiController]
public class TransferSchoolController : ControllerBase
{
    private readonly TransferSchoolRepo _transferSchoolRepo;
    private readonly ITransferSchoolService _service;


    public TransferSchoolController(TransferSchoolRepo transferSchoolRepo,
        ITransferSchoolService service)
    {
        _transferSchoolRepo = transferSchoolRepo;
        _service = service;
    }

    /// <summary>
    /// 1️⃣ Lấy danh sách học sinh đã chuyển trường
    /// </summary>
    [HttpGet("list")]
    public IActionResult GetTransferSchoolList()
    {
        var result = _transferSchoolRepo.GetTransferSchoolList();
        return Ok(new { message = "Lấy danh sách học sinh chuyển trường thành công", data = result });
    }

    /// <summary>
    /// 2️⃣ Lấy thông tin chuyển trường của một học sinh theo ID
    /// </summary>
    [HttpGet("student/{studentId}")]
    public IActionResult GetTransferSchoolByStudentId(int studentId)
    {
        var result = _transferSchoolRepo.GetTransferSchoolByStudentId(studentId);
        if (result == null)
        {
            return NotFound(new { message = "Không tìm thấy thông tin chuyển trường cho học sinh này" });
        }
        return Ok(new { message = "Lấy thông tin chi tiết chuyển trường thành công", data = result });
    }


    // Thêm mới TransferSchool
    [HttpPost]
    public IActionResult CreateTransferSchool([FromBody] TransferSchoolRequest request)
    {
        if (request == null)
            return BadRequest(new { Message = "Dữ liệu không hợp lệ." });

        var response = _service.CreateTransferSchool(request);
        if (response.Data == null)
            return BadRequest(response);

        return Ok(response);
    }

    // Cập nhật TransferSchool
    [HttpPut("{id}")]
    public IActionResult UpdateTransferSchool(int id, [FromBody] TransferSchoolRequest request)
    {
        if (request == null)
            return BadRequest(new { Code = 1, Message = "Dữ liệu không hợp lệ." });

        var response = _service.UpdateTransferSchool(id, request);
        return CreatedAtAction(nameof(GetTransferSchoolByStudentId), new { id = response.Data.StudentId }, response);
    }

    // Xóa mềm TransferSchool
    

}
