﻿using System.ComponentModel.DataAnnotations;

namespace ISC_ELIB_SERVER.DTOs.Requests
{
    public class SubjectRequest
    {
        [Required(ErrorMessage = "Mã không được để trống")]
        public string Code { get; set; }
        [Required(ErrorMessage = "Tên không được để trống")]

        [MaxLength(100, ErrorMessage = "Tên không được vượt quá 100 ký tự")]
        public string Name { get; set; }
        public int HoursSemester1 { get; set; }
        public int HoursSemester2 { get; set; }
        public long SubjectGroupId { get; set; }
        public long SubjectTypeId { get; set; }
    }
}
