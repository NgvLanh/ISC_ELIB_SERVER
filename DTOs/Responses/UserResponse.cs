namespace ISC_ELIB_SERVER.DTOs.Responses
{
    public class UserResponse
    {
        public int Id { get; set; }
        public string? Code { get; set; }
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
        public int? ProvinceCode { get; set; }
        public int? DistrictCode { get; set; }
        public int? WardCode { get; set; }      
        public string? Street { get; set; }
        public bool Active { get; set; }
        public string? AvatarUrl { get; set; }
        // Thêm thông tin địa chỉ chi tiết
        public string? ProvinceName { get; set; }
        public string? DistrictName { get; set; }
        public string? WardName { get; set; }
    }
}
