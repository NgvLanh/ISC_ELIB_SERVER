﻿using ISC_ELIB_SERVER.Models;

namespace ISC_ELIB_SERVER.DTOs.Responses
{
    public class StudentInfoResponses
    {
<<<<<<< HEAD
        public long Id { get; set; }
=======
        public int Id { get; set; }
>>>>>>> dev
        public string? GuardianName { get; set; }
        public string? GuardianPhone { get; set; }
        public string? GuardianJob { get; set; }
        public DateTime? GuardianDob { get; set; }
        public string? GuardianAddress { get; set; }
        public string? GuardianRole { get; set; }
<<<<<<< HEAD
        public long UserId { get; set; }
=======
        public int UserId { get; set; }
>>>>>>> dev

        public virtual User User { get; set; } = null!;
    }
}
