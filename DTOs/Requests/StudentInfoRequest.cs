﻿using ISC_ELIB_SERVER.Models;

namespace ISC_ELIB_SERVER.DTOs.Requests
{
    public class StudentInfoRequest
    {
        public long Id { get; set; }
        public string? GuardianName { get; set; }
        public string? GuardianPhone { get; set; }
        public string? GuardianJob { get; set; }
        public DateTime? GuardianDob { get; set; }
        public string? GuardianAddress { get; set; }
        public string? GuardianRole { get; set; }
        public long? UserId { get; set; }

        public virtual User? User { get; set; } = null!;
    }
}
