using System;
using System.Collections.Generic;

namespace ISC_ELIB_SERVER.Models
{
    public partial class TrainingProgram
    {
        public long Id { get; set; }
        public string? Name { get; set; }
        public long MajorId { get; set; }
        public long SchoolFacilitiesId { get; set; }
        public DateOnly? StartDate { get; set; }
        public DateOnly? EndDate { get; set; }
        public string? Degree { get; set; }
        public string? TrainingForm { get; set; }
        public bool? Deleted { get; set; }
        public string? FileName { get; set; }
        public string? FilePath { get; set; }
    }
}
