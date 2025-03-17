using System;

namespace ISC_ELIB_SERVER.Models
{
<<<<<<< HEAD
        public partial class TeacherFamily
        {
                public int Id { get; set; }
                public int TeacherId { get; set; }
                public string? GuardianName { get; set; }
                public string? GuardianPhone { get; set; }
                public string? GuardianAddressDetail { get; set; }
                public string? GuardianAddressFull { get; set; }
                public int ProvinceCode { get; set; }
                public int DistrictCode { get; set; }
                public int WardCode { get; set; }
                public bool Active { get; set; }
=======
    public partial class TeacherFamily
    {
        public int Id { get; set; }
        public int TeacherId { get; set; }
        public string? GuardianName { get; set; }
        public string? GuardianPhone { get; set; }
        public string? GuardianAddressDetail { get; set; }
        public string? GuardianAddressFull { get; set; }

        public int ProvinceCode { get; set; }
        public int DistrictCode { get; set; }
        public int WardCode { get; set; }
        public bool IsDeleted { get; set; } = false;

        public bool Active { get; set; }

>>>>>>> 30ea54130e2f90c7eb7720bf35ca70328b23fbb4

                public virtual TeacherInfo? Teacher { get; set; }
        }
}
