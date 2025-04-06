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
using Microsoft.EntityFrameworkCore;

[Route("api/transfer-school")]
[ApiController]
public class TransferSchoolController : ControllerBase
{
    private readonly TransferSchoolRepo _transferSchoolRepo;
    private readonly ITransferSchoolService _service;
    private readonly isc_dbContext _context;


    public TransferSchoolController(TransferSchoolRepo transferSchoolRepo, isc_dbContext context,
        ITransferSchoolService service)
    {
        _transferSchoolRepo = transferSchoolRepo;
        _service = service;
        _context = context;
    }

    /// <summary>
    /// 1️⃣ Lấy danh sách học sinh đã chuyển trường
    /// </summary>
    [HttpGet("list")]
    public IActionResult GetTransferSchoolList()
    {
        var result = _transferSchoolRepo.GetTransferSchoolList();
        if (result == null)
        {
            return NotFound(new { code = 1, message = "Không có dữ liệu chuyển trường" });
        }
        return Ok(new { code = 0, message = "Lấy danh sách học sinh chuyển trường thành công", data = result });
    }

    /// <summary>
    /// 2️⃣ Lấy thông tin chuyển trường của một học sinh theo ID
    [HttpGet("byStudentCode/{studentCode}")]
    public async Task<IActionResult> GetTransferSchoolByStudentCode(string studentCode)
    {
        var studentResponse = _service.GetTransferSchoolByStudentCode(studentCode);

        if (studentResponse == null || studentResponse.Data == null)
        {
            return NotFound(new { code = 1, message = "Không tìm thấy học sinh với mã StudentCode đã cung cấp." });
        }

        // Trả về thông tin chuyển trường nếu tìm thấy
        return Ok(new { code = 0, message = "Success", data = studentResponse.Data });
    }

    [HttpGet("byStudentId/{studentId}")]
    public async Task<IActionResult> GetTransferSchoolByStudentId(int studentId)
    {
        // Gọi phương thức bất đồng bộ để lấy thông tin chuyển trường
        var result = _transferSchoolRepo.GetTransferSchoolByStudentId(studentId);

        if (result == null)
        {
            // Trả về NotFound nếu không tìm thấy thông tin
            return NotFound(new { code = 1, message = "Không tìm thấy thông tin chuyển trường cho học sinh này" });
        }

        // Trả về thông tin chuyển trường nếu tìm thấy
        return Ok(new { code = 0, message = "Lấy thông tin chi tiết chuyển trường thành công", data = result });
    }

    // Thêm mới TransferSchool
    [HttpPost]
    public IActionResult CreateTransferSchool([FromBody] TransferSchoolRequest request)
    {
        if (request == null || string.IsNullOrEmpty(request.StudentCode))
        {
            return BadRequest(new { Message = "Dữ liệu không hợp lệ. Vui lòng nhập StudentCode." });
        }

        // Lấy userId từ token
        var userId = GetUserId();  // Phương thức này lấy userId từ token
        if (userId == null)
        {
            return BadRequest(new { Message = "Không thể xác định userId." });
        }

        // Gán userId vào request trước khi gọi service
        request.UserId = userId.Value;

        // Gọi service để xử lý logic
        var response = _service.CreateTransferSchool(request);

        if (response.Data == null)
        {
            return BadRequest(response);
        }

        return Ok(response);
    }


    [HttpPut("{studentCode}")]
    public IActionResult UpdateTransferSchool(string studentCode, [FromBody] TransferSchoolRequest request)
    {
        if (request == null || string.IsNullOrEmpty(request.StudentCode))
            return BadRequest(new { Message = "Dữ liệu không hợp lệ. Vui lòng nhập StudentCode." });

        // Lấy userId từ token
        var userId = GetUserId();
        if (userId == null)
        {
            return BadRequest(new { Message = "Không thể xác định userId." });
        }

        // Gán userId vào request trước khi gọi service
        request.UserId = userId.Value;

        // Gọi service để xử lý logic
        var response = _service.UpdateTransferSchool(studentCode,request);

        if (response.Data == null)
            return BadRequest(response);

        return Ok(response);
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
