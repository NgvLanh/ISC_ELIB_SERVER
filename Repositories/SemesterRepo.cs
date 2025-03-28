using ISC_ELIB_SERVER.Models;
using Microsoft.EntityFrameworkCore;

namespace ISC_ELIB_SERVER.Repositories
{
    public class SemesterRepo
    {
        private readonly isc_dbContext _context;
        public SemesterRepo(isc_dbContext context)
        {
            _context = context;
        }

        public ICollection<Semester> GetSemesters()
        {
            return _context.Semesters
                .Where(a => a.Active)
                .ToList();
        }


        //public ICollection<object> GetCourseOfSemesters(int userId)
        //{
        //    var dayOfWeekMapping = new Dictionary<DayOfWeek, string>
        //    {
        //        { DayOfWeek.Sunday, "Chủ Nhật" },
        //        { DayOfWeek.Monday, "Thứ 2" },
        //        { DayOfWeek.Tuesday, "Thứ 3" },
        //        { DayOfWeek.Wednesday, "Thứ 4" },
        //        { DayOfWeek.Thursday, "Thứ 5" },
        //        { DayOfWeek.Friday, "Thứ 6" },
        //        { DayOfWeek.Saturday, "Thứ 7" }
        //    };

        //    var rawData = (from ta in _context.TeachingAssignments
        //                   where ta.UserId == userId
        //                   join s in _context.Semesters on ta.SemesterId equals s.Id
        //                   join ay in _context.AcademicYears on s.AcademicYearId equals ay.Id
        //                   join c in _context.Classes on ta.ClassId equals c.Id
        //                   join sub in _context.Subjects on ta.SubjectId equals sub.Id
        //                   join ses in _context.Sessions on ta.Id equals ses.TeachingAssignmentId into sesGroup
        //                   from ses in sesGroup.DefaultIfEmpty()

        //                   select new
        //                   {
        //                       SemesterId = s.Id,
        //                       Semester = s.Name,
        //                       Subject = sub.Name,
        //                       Class = c.Name,
        //                       StartDate = ta.StartDate,
        //                       EndDate = ta.EndDate,
        //                       SessionStart = ses != null ? ses.StartDate : null,
        //                       Status = ses != null ? ses.Status : null,
        //                       TeachingAssignmentId = ta.Id
        //                   }).ToList();

        //    // Lấy session có StartDate nhỏ nhất cho mỗi TeachingAssignment
        //    var groupedSessions = rawData
        //        .GroupBy(x => x.TeachingAssignmentId)
        //        .Select(g => new
        //        {
        //            TeachingAssignmentId = g.Key,
        //            EarliestSession = g.Where(x => x.SessionStart.HasValue)
        //                               .OrderBy(x => x.SessionStart)
        //                               .FirstOrDefault()
        //        }).ToDictionary(x => x.TeachingAssignmentId, x => x.EarliestSession);

        //    var transformedData = rawData
        //        .GroupBy(x => new { x.SemesterId, x.Semester, x.Subject, x.Class, x.StartDate, x.EndDate })
        //        .Select(g => new
        //        {
        //            g.Key.SemesterId,
        //            g.Key.Semester,
        //            Subject = g.Key.Subject,
        //            Class = g.Key.Class,
        //            Schedule = groupedSessions.ContainsKey(g.First().TeachingAssignmentId) &&
        //                       groupedSessions[g.First().TeachingAssignmentId]?.SessionStart != null
        //                ? dayOfWeekMapping[groupedSessions[g.First().TeachingAssignmentId].SessionStart.Value.DayOfWeek] +
        //                  " - " + groupedSessions[g.First().TeachingAssignmentId].SessionStart.Value.ToString("HH:mm")
        //                : "Không có lịch",
        //            Date = $"{g.Key.StartDate?.ToString("dd/MM") ?? ""} - {g.Key.EndDate?.ToString("dd/MM") ?? ""}",
        //            Status = g.First().Status
        //        }).ToList();

        //    var groupedData = transformedData
        //        .GroupBy(x => new { x.SemesterId, x.Semester })
        //        .Select(g => new
        //        {
        //            Id = g.Key.SemesterId,
        //            Semester = g.Key.Semester,
        //            Courses = g.Select(x => new
        //            {
        //                x.Subject,
        //                x.Class,
        //                x.Schedule,
        //                x.Date,
        //                x.Status
        //            }).ToList()
        //        }).ToList<object>();

        //    return groupedData;
        //}



        public ICollection<object> GetCourseOfSemesters(int userId)
        {
            var dayOfWeekMapping = new Dictionary<DayOfWeek, string>
            {
                { DayOfWeek.Sunday, "Chủ Nhật" },
                { DayOfWeek.Monday, "Thứ 2" },
                { DayOfWeek.Tuesday, "Thứ 3" },
                { DayOfWeek.Wednesday, "Thứ 4" },
                { DayOfWeek.Thursday, "Thứ 5" },
                { DayOfWeek.Friday, "Thứ 6" },
                { DayOfWeek.Saturday, "Thứ 7" }
            }                                   ;

            var statusMapping = new Dictionary<string, string>
            {
                { "Scheduled", "Chưa hoàn thành" },
                { "Ongoing", "Đang diễn ra" },
                { "Completed", "Đã hoàn thành" }
            };

            var rawData = (from ta in _context.TeachingAssignments
                           where ta.UserId == userId
                           join s in _context.Semesters on ta.SemesterId equals s.Id
                           join ay in _context.AcademicYears on s.AcademicYearId equals ay.Id
                           join c in _context.Classes on ta.ClassId equals c.Id
                           join sub in _context.Subjects on ta.SubjectId equals sub.Id
                           join ses in _context.Sessions on ta.Id equals ses.TeachingAssignmentId into sesGroup
                           from ses in sesGroup.DefaultIfEmpty()

                           select new
                           {
                               SemesterId = s.Id,
                               Semester = s.Name,
                               Subject = sub.Name,
                               Class = c.Name,
                               StartDate = ta.StartDate,
                               EndDate = ta.EndDate,
                               SessionStart = ses != null ? ses.StartDate : null,
                               Status = ses != null ? ses.Status : null,
                               TeachingAssignmentId = ta.Id
                           }).ToList();

            var groupedSessions = rawData
                .GroupBy(x => x.TeachingAssignmentId)
                .Select(g => new
                {
                    TeachingAssignmentId = g.Key,
                    EarliestSession = g.Where(x => x.SessionStart.HasValue)
                                       .OrderBy(x => x.SessionStart)
                                       .FirstOrDefault()
                }).ToDictionary(x => x.TeachingAssignmentId, x => x.EarliestSession);

            var transformedData = rawData
                .GroupBy(x => new { x.SemesterId, x.Semester, x.Subject, x.Class, x.StartDate, x.EndDate })
                .Select(g => new
                {
                    g.Key.SemesterId,
                    g.Key.Semester,
                    Subject = g.Key.Subject,
                    Class = g.Key.Class,
                    Schedule = groupedSessions.ContainsKey(g.First().TeachingAssignmentId) &&
                               groupedSessions[g.First().TeachingAssignmentId]?.SessionStart != null
                        ? dayOfWeekMapping[groupedSessions[g.First().TeachingAssignmentId].SessionStart.Value.DayOfWeek] +
                          " - " + groupedSessions[g.First().TeachingAssignmentId].SessionStart.Value.ToString("HH:mm")
                        : "Không có lịch",
                    Date = $"{g.Key.StartDate?.ToString("dd/MM") ?? ""} - {g.Key.EndDate?.ToString("dd/MM") ?? ""}",
                    Status = g.First().Status != null && statusMapping.ContainsKey(g.First().Status)
                        ? statusMapping[g.First().Status]
                        : "Không xác định"
                }).ToList();

            var groupedData = transformedData
                .GroupBy(x => new { x.SemesterId, x.Semester })
                .Select(g => new
                {
                    Id = g.Key.SemesterId,
                    Semester = g.Key.Semester,
                    Courses = g.Select(x => new
                    {
                        x.Subject,
                        x.Class,
                        x.Schedule,
                        x.Date,
                        x.Status
                    }).ToList()
                }).ToList<object>();

            return groupedData;
        }








        public Semester GetSemesterById(long id)
        {
            return _context.Semesters.Where(a => a.Active).FirstOrDefault(s => s.Id == id);
        }

        public Semester CreateSemester(Semester Semester)
        {
            _context.Semesters.Add(Semester);
            _context.SaveChanges();
            return Semester;
        }

        public Semester UpdateSemester(Semester Semester)
        {
            _context.Semesters.Update(Semester);
            _context.SaveChanges();
            return Semester;
        }

        public bool DeleteSemester(long id)
        {
            var Semester = GetSemesterById(id);
            if (Semester != null)
            {
                Semester.Active = false;
                _context.Semesters.Update(Semester);
                return _context.SaveChanges() > 0;
            }
            return false;
        }


        // Lành -- Cần học kỳ theo niên khoá
        public ICollection<Semester> GetSemestersByAcademicYearId(long academicYearId)
        {
            return _context.Semesters.Where(s => s.Active && s.AcademicYearId == academicYearId).ToList();
        }
    }
}