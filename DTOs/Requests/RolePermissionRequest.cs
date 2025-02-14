using System.ComponentModel.DataAnnotations;

namespace ISC_ELIB_SERVER.DTOs.Requests
{
    public class RolePermissionRequest
    {
        [Required(ErrorMessage = "PermissionId không được để trống")]
        [RegularExpression(@"^\d+$", ErrorMessage = "PermissionId phải là số nguyên")]
        public long PermissionId { get; set; }

        [Required(ErrorMessage = "RoleId không được để trống")]
        [RegularExpression(@"^\d+$", ErrorMessage = "RoleId phải là số nguyên")]
        public long RoleId { get; set; }

        [Required(ErrorMessage = "IsActive không được để trống")]
        public bool IsActive { get; set; } = true;
    }
}
