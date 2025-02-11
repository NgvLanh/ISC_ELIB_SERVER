using System;
using System.Collections.Generic;

namespace ISC_ELIB_SERVER.Models
{
    public partial class TeacherFamily
    {
        public long Id { get; set; }
        public long TeacherId { get; set; }
        public string? GuardianName { get; set; }
        public string? GuardianPhone { get; set; }
        public string? GuardianAddressDetail { get; set; }
        public string? GuardianAddressFull { get; set; }
        public long ProvinceCode { get; set; }
        public long DistrictCode { get; set; }
        public long WardCode { get; set; }

        public virtual TeacherInfo Teacher { get; set; } = null!;
    }
}
