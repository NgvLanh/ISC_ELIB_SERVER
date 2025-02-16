using System;
using System.Collections.Generic;

namespace ISC_ELIB_SERVER.Models
{
    public partial class TeacherInfo
    {
        public TeacherInfo()
        {
            ExamScheduleClasses = new HashSet<ExamScheduleClass>();
            GradeLevels = new HashSet<GradeLevel>();
            Resignations = new HashSet<Resignation>();
            SubjectGroups = new HashSet<SubjectGroup>();
            TeacherFamilies = new HashSet<TeacherFamily>();
            TemporaryLeaves = new HashSet<TemporaryLeave>();
        }

        public long Id { get; set; }
        public string? Cccd { get; set; }
        public DateTime? IssuedDate { get; set; }
        public string? IssuedPlace { get; set; }
        public bool? UnionMember { get; set; }
        public DateTime? UnionDate { get; set; }
        public string? UnionPlace { get; set; }
        public bool? PartyMember { get; set; }
        public DateTime? PartyDate { get; set; }
        public long UserId { get; set; }
        public string? AddressFull { get; set; }
        public long ProvinceCode { get; set; }
        public long DistrictCode { get; set; }
        public long WardCode { get; set; }

        public virtual User User { get; set; } = null!;
        public virtual ICollection<ExamScheduleClass> ExamScheduleClasses { get; set; }
        public virtual ICollection<GradeLevel> GradeLevels { get; set; }
        public virtual ICollection<Resignation> Resignations { get; set; }
        public virtual ICollection<SubjectGroup> SubjectGroups { get; set; }
        public virtual ICollection<TeacherFamily> TeacherFamilies { get; set; }
        public virtual ICollection<TemporaryLeave> TemporaryLeaves { get; set; }
    }
}
