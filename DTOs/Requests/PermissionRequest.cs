using System.ComponentModel.DataAnnotations;

namespace ISC_ELIB_SERVER.DTOs.Requests
{
    public class PermissionRequest
    {
        [Required(ErrorMessage = "Tên không được để trống")]
        [MaxLength(50, ErrorMessage = "Tên không được vượt quá 50 ký tự")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "IsActive không được để trống")]
        public bool IsActive { get; set; } = true;
    }
    public class PermissionGetById
    {
        [RegularExpression(@"^\d+$", ErrorMessage = "Id phải là số nguyên")]
        public long Id { get; set; }
    }
}
