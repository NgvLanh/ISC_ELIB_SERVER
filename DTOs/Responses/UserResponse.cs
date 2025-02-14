namespace ISC_ELIB_SERVER.DTOs.Responses
{
    public class UserResponse
    {
        public long Id { get; set; }
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
        public long RoleId { get; set; }
        public long AcademicYearId { get; set; }
        public long UserStatusId { get; set; }
        public long ClassId { get; set; }
        public long EntryType { get; set; }
        public string? AddressFull { get; set; }
        public long ProvinceCode { get; set; }
        public long DistrictCode { get; set; }
        public long WardCode { get; set; }
        public string? Street { get; set; }
    }
}
