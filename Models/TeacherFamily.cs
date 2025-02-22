using System;

namespace ISC_ELIB_SERVER.Models
{
    public partial class TeacherFamily
    {
        public int Id { get; set; }
        public int TeacherId { get; set; }
        public string? GuardianName { get; set; }
        public string? GuardianPhone { get; set; }
        public string? GuardianAddressDetail { get; set; }
        public string? GuardianAddressFull { get; set; }

        public long? ProvinceCode { get; set; }
        public long? DistrictCode { get; set; }
        public long? WardCode { get; set; }
        public bool IsDeleted { get; set; } = false;

        public bool Active { get; set; }


        public virtual TeacherInfo? Teacher { get; set; }
    }
}
