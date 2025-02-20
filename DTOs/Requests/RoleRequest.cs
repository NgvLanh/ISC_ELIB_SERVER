﻿using System.ComponentModel.DataAnnotations;

namespace ISC_ELIB_SERVER.DTOs.Requests
{
    public class RoleRequest
    {
        [Required(ErrorMessage = "Tên không được để trống")]
        [MaxLength(50, ErrorMessage = "Tên không được vượt quá 50 ký tự")]
        public string Name { get; set; } = string.Empty;

        [MaxLength(255, ErrorMessage = "Tên không được vượt quá 255 ký tự")]
        public string Description { get; set; } = string.Empty;

        [Required(ErrorMessage = "Trạng thái không được để trống")]
        public bool Active { get; set; } = true;
    }
}
