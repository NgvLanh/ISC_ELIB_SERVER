using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using ISC_ELIB_SERVER.Repositories;
using ISC_ELIB_SERVER.Models;

[Route("api/transfer-school")]
[ApiController]
public class TransferSchoolController : ControllerBase
{
    private readonly TransferSchoolRepo _transferSchoolRepo;

    public TransferSchoolController(TransferSchoolRepo transferSchoolRepo)
    {
        _transferSchoolRepo = transferSchoolRepo;
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
    [HttpGet("student/{studentInfoId}")]
    public IActionResult GetTransferSchoolByStudentId(int studentInfoId)
    {
        var result = _transferSchoolRepo.GetTransferSchoolByStudentId(studentInfoId);
        if (result == null)
        {
            return NotFound(new { message = "Không tìm thấy thông tin chuyển trường cho học sinh này" });
        }
        return Ok(new { message = "Lấy thông tin chi tiết chuyển trường thành công", data = result });
    }

    /// <summary>
    /// 3️⃣ Thêm mới thông tin chuyển trường
    /// </summary>
/*    [HttpPost("add/{studentId}")]
    public IActionResult PostTransferSchool(int studentId, [FromBody] TransferSchool transferSchool)
    {
        if (transferSchool == null)
        {
            return BadRequest(new { message = "Dữ liệu không hợp lệ" });
        }

        var result = _transferSchoolRepo.PostTransferSchool(studentId, transferSchool);
        if (result == null)
        {
            return BadRequest(new { message = "Thêm thông tin chuyển trường thất bại" });
        }
        return Ok(new { message = "Thêm thông tin chuyển trường thành công", data = result });
    }
*/

    /// <summary>
    /// 4️⃣ Cập nhật thông tin chuyển trường
    /// </summary>
    [HttpPut("update/{id}")]
    public IActionResult UpdateTransferSchool(int id, [FromBody] TransferSchool transferSchool)
    {
        if (transferSchool == null)
        {
            return BadRequest(new { message = "Dữ liệu cập nhật không hợp lệ" });
        }

        var result = _transferSchoolRepo.UpdateTransferSchool(id, transferSchool);
        if (result == null)
        {
            return NotFound(new { message = "Không tìm thấy thông tin chuyển trường để cập nhật" });
        }
        return Ok(new { message = "Cập nhật thông tin chuyển trường thành công", data = result });
    }
}
