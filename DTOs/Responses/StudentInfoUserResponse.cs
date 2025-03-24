﻿namespace ISC_ELIB_SERVER.DTOs.Responses
{
    public class StudentInfoUserResponse
    {
        public int Id { get; set; }  // ID từ bảng StudentInfo
        public string? FullName { get; set; }  // Tên đầy đủ từ bảng User
        public DateTime? Dob { get; set; }  // Ngày sinh từ bảng User
        public bool? Gender { get; set; }  // Giới tính từ bảng User
        public string? Nation { get; set; }  // Quốc tịch từ bảng User
        public string? ClassName { get; set; } // Tên lớp từ bảng Class
        public string? UserStatusName { get; set; } // Tên trạng thái từ bảng UserStatus
    }
}
