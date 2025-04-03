using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using ISC_ELIB_SERVER.Repositories;
using ISC_ELIB_SERVER.Models;
using Autofac.Core;
using ISC_ELIB_SERVER.DTOs.Requests;
using ISC_ELIB_SERVER.Services.Interfaces;
using ISC_ELIB_SERVER.DTOs.Requests.ISC_ELIB_SERVER.DTOs.Requests;
using ISC_ELIB_SERVER.Services;

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

        request.UserId = GetUserId();
        var response = _service.CreateTransferSchool(request);
        if (response.Data == null)
            return BadRequest(response);

        return Ok(response);
    }

    [HttpPut("{studentId}")]
    public IActionResult UpdateTransferSchool(int studentId, [FromBody] TransferSchoolRequest request)
    {
        try
        {
            var updatedTransfer = _service.UpdateTransferSchool(studentId, request);
            return updatedTransfer != null
                ? Ok("Cập nhật thành công!")
                : NotFound("Không tìm thấy dữ liệu để cập nhật!");
        }
        catch (Exception ex)
        {
            return BadRequest($"Lỗi: {ex.Message}");
        }
    }

    // Lấy userId từ token JWT
    private int? GetUserId()
    {
        var userIdString = User.FindFirst("Id")?.Value;
        Console.WriteLine($"User.FindFirst(\"Id\"): {userIdString}");

        if (string.IsNullOrEmpty(userIdString) || !int.TryParse(userIdString, out int userId))
        {
            return null; // Trả về null nếu không tìm thấy hoặc parse thất bại
        }

        return userId;
    }


}
