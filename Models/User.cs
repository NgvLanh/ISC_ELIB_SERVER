using System;
using System.Collections.Generic;

namespace ISC_ELIB_SERVER.Models
{
    public partial class User
    {
        public User()
        {
            Achievements = new HashSet<Achievement>();
            Campuses = new HashSet<Campus>();
            ChangeClasses = new HashSet<ChangeClass>();
            Classes = new HashSet<Class>();
            Discussions = new HashSet<Discussion>();
            ExamGraders = new HashSet<ExamGrader>();
            Exemptions = new HashSet<Exemption>();
            NotificationSenders = new HashSet<Notification>();
            NotificationUsers = new HashSet<Notification>();
            Reserves = new HashSet<Reserve>();
            StudentInfos = new HashSet<StudentInfo>();
            StudentScores = new HashSet<StudentScore>();
            Supports = new HashSet<Support>();
            SystemSettings = new HashSet<SystemSetting>();
            TeacherInfos = new HashSet<TeacherInfo>();
            TeachingAssignments = new HashSet<TeachingAssignment>();
            Tests = new HashSet<Test>();
            TestsSubmissions = new HashSet<TestsSubmission>();
            TransferSchools = new HashSet<TransferSchool>();
        }

        public long Id { get; set; }
        public string? Code { get; set; }
        public string? Password { get; set; }
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
        public long? ClassId { get; set; }
        public long EntryType { get; set; }
        public string? AddressFull { get; set; }
        public long ProvinceCode { get; set; }
        public long DistrictCode { get; set; }
        public long WardCode { get; set; }
        public string? Street { get; set; }

        public virtual AcademicYear AcademicYear { get; set; } = null!;
        public virtual Class Class { get; set; } = null!;
        public virtual EntryType EntryTypeNavigation { get; set; } = null!;
        public virtual Role Role { get; set; } = null!;
        public virtual UserStatus UserStatus { get; set; } = null!;
        public virtual ICollection<Achievement> Achievements { get; set; }
        public virtual ICollection<Campus> Campuses { get; set; }
        public virtual ICollection<ChangeClass> ChangeClasses { get; set; }
        public virtual ICollection<Class> Classes { get; set; }
        public virtual ICollection<Discussion> Discussions { get; set; }
        public virtual ICollection<ExamGrader> ExamGraders { get; set; }
        public virtual ICollection<Exemption> Exemptions { get; set; }
        public virtual ICollection<Notification> NotificationSenders { get; set; }
        public virtual ICollection<Notification> NotificationUsers { get; set; }
        public virtual ICollection<Reserve> Reserves { get; set; }
        public virtual ICollection<StudentInfo> StudentInfos { get; set; }
        public virtual ICollection<StudentScore> StudentScores { get; set; }
        public virtual ICollection<Support> Supports { get; set; }
        public virtual ICollection<SystemSetting> SystemSettings { get; set; }
        public virtual ICollection<TeacherInfo> TeacherInfos { get; set; }
        public virtual ICollection<TeachingAssignment> TeachingAssignments { get; set; }
        public virtual ICollection<Test> Tests { get; set; }
        public virtual ICollection<TestsSubmission> TestsSubmissions { get; set; }
        public virtual ICollection<TransferSchool> TransferSchools { get; set; }
    }
}
