using System.ComponentModel.DataAnnotations.Schema;

namespace ISC_ELIB_SERVER.DTOs.Requests
{
    public class UserRequest
    {
        public string? Code { get; set; }
        public string? Password { get; set; }
        public string? FullName { get; set; }
        public DateTime? Dob { get; set; }
        public bool? Gender { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public string? PlaceBirth { get; set; }
        public string? Nation { get; set; }
        public string? Religion { get; set; }
        public DateTime? EnrollmentDate { get; set; }
        public int RoleId { get; set; }
        public int AcademicYearId { get; set; }
        public int UserStatusId { get; set; }
        public int ClassId { get; set; }
        public int EntryType { get; set; }
        public string? AddressFull { get; set; }
        public int ProvinceCode { get; set; }
        public int DistrictCode { get; set; }
        public int WardCode { get; set; }
        public string? Street { get; set; }
        public bool Active { get; set; }
        public string? AvatarUrl { get; set; }

        // Phương thức kiểm tra tính hợp lệ
        public bool IsValid()
        {
            // Kiểm tra các trường bắt buộc: FullName, Email và Password
            if (string.IsNullOrEmpty(FullName))
            {
                return false; // Tên đầy đủ không được để trống
            }

            if (string.IsNullOrEmpty(Email) || !Email.Contains("@"))
            {
                return false; // Email không hợp lệ
            }

            if (string.IsNullOrEmpty(Password))
            {
                return false; // Mật khẩu không được để trống
            }

            return true;
        }
    }
}
