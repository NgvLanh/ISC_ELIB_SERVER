using System;

namespace ISC_ELIB_SERVER.DTOs.Responses
{
    public class TransferSchoolResponse
    {
        public int Id { get; set; }
        public string? StudentName { get; set; }  // Thêm dấu `?`
        public string? StudentCode { get; set; } // Thêm dấu `?`

        public DateTime? TransferSchoolDate { get; set; }
        public string? TransferToSchool { get; set; }
        public string? SchoolAddress { get; set; }
        public string? Reason { get; set; }
        public string? AttachmentName { get; set; }
        public string? AttachmentPath { get; set; }

        public string? SemesterName { get; set; } // Thêm dấu `?`
        public int? ProvinceCode { get; set; }
        public int? DistrictCode { get; set; }
        public int? SemesterId { get; set; }
        public int? LeadershipId { get; set; }
    }

}
