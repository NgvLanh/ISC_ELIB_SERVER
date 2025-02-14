namespace ISC_ELIB_SERVER.DTOs.Responses
{
    public class TeacherFamilyResponse
    {
        public long Id { get; set; }
        public long TeacherId { get; set; }
        public string GuardianName { get; set; } = string.Empty;
        public string GuardianPhone { get; set; } = string.Empty;
        public string GuardianAddressDetail { get; set; } = string.Empty;
        public string GuardianAddressFull { get; set; } = string.Empty;
        public long ProvinceCode { get; set; }
        public long DistrictCode { get; set; }
        public long WardCode { get; set; }
    }
}
