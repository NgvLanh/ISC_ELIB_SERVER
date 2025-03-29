using ISC_ELIB_SERVER.Models;
using ISC_ELIB_SERVER.DTOs.Requests;
using ISC_ELIB_SERVER.DTOs.Responses;
using System.Collections.Generic;
using ISC_ELIB_SERVER.Utils;
using System.Linq;

using Microsoft.EntityFrameworkCore;

namespace ISC_ELIB_SERVER.Repositories
{
    public class SessionRepo
    {
        private readonly isc_dbContext _context;
        public SessionRepo(isc_dbContext context)
        {
            _context = context;
        }

        public ICollection<Session> GetAllSessions()
        {
            return _context.Sessions.ToList();
        }

        public int Count()
        {
            return _context.Sessions.Count();
        }

        public Session GetSessionById(int id)
        {
            return _context.Sessions.FirstOrDefault(s => s.Id == id);
        }

        public Session CreateSession(Session session)
        {
            _context.Sessions.Add(session);
            _context.SaveChanges();
            return session;
        }

        public bool DeleteSession(int id)
        {
            var session = GetSessionById(id);
            if (session != null)
            {
                _context.Sessions.Remove(session);
                return _context.SaveChanges() > 0;
            }
            return false;
        }

        public Session UpdateSession(Session session)
        {
            _context.Sessions.Update(session);
            _context.SaveChanges();
            return session;
        }


        public Session? GetSessionByJoinInfo(JoinSessionRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.ShareCodeOrSessionId) || string.IsNullOrWhiteSpace(request.Password))
            {
                return null; // Không có đủ thông tin để tìm lớp
            }

            var query = _context.Sessions.AsQueryable();
            Session? session = null;

            // Kiểm tra nếu nhập số thì tìm theo SessionId
            if (int.TryParse(request.ShareCodeOrSessionId, out int sessionId))
            {
                session = query.FirstOrDefault(s => s.Id == sessionId);
            }
            else
            {
                // Nếu không phải số, tìm theo ShareCodeUrl
                session = query.FirstOrDefault(s => s.ShareCodeUrl == request.ShareCodeOrSessionId);
            }

            // Kiểm tra mật khẩu
            string hashedInputPassword = PasswordHasher.HashPassword(request.Password);
            if (session != null && session.Password == hashedInputPassword)
            {
                return session; // Trả về session nếu tìm thấy và mật khẩu đúng
            }

            return null; // Trả về null nếu không tìm thấy hoặc sai mật khẩu
        }


        public ICollection<SessionStudentResponse> GetFilteredSessions(SessionStudentFilterRequest request)
        {
            var query = _context.Sessions.AsQueryable();

            // Lấy lớp học của sinh viên
            var student = _context.Users.Include(u => u.Class)
                                        .FirstOrDefault(u => u.Id == request.studentId);

            if (student == null || student.Class == null)
            {
                return new List<SessionStudentResponse>(); // Không có lớp
            }

            // Lọc theo lớp học của sinh viên
            var classId = student.Class.Id;
            var teachingAssignments = _context.TeachingAssignments
                                              .Where(ta => ta.ClassId == classId && ta.Class.Active) // Lọc lớp học Đang hoạt động hay không
                                              .Select(ta => ta.Id);

            query = query.Where(s => s.TeachingAssignmentId.HasValue && teachingAssignments.Contains(s.TeachingAssignmentId.Value));

            // Lọc theo ngày
            if (request.Date.HasValue)
            {
                query = query.Where(s => s.StartDate.HasValue && s.StartDate.Value.Date == request.Date.Value.Date);
            }

            // Lọc theo môn học
            if (request.SubjectId.HasValue)
            {
                query = query.Where(s => s.TeachingAssignment != null && s.TeachingAssignment.SubjectId == request.SubjectId.Value);
            }

            // Lọc theo niên khóa
            if (request.AcademicYearId.HasValue)
            {
                query = query.Where(s => s.TeachingAssignment != null && s.TeachingAssignment.Class != null && s.TeachingAssignment.Class.AcademicYearId == request.AcademicYearId.Value);
            }

            // Lọc theo tên topic với LIKE (gần giống)
            if (!string.IsNullOrEmpty(request.TopicName))
            {
                Console.WriteLine($"topicName: {request.TopicName}");
                // query = query.Where(s => s.Description != null && EF.Functions.Like(s.Description, $"%{request.TopicName}%"));
                query = query.Where(s => EF.Functions.ILike(s.TeachingAssignment.Topics.Name, $"%{request.TopicName}%"));
            }
            // Lọc theo trạng thái lớp
            if (!string.IsNullOrEmpty(request.Status))
            {
                switch (request.Status.ToLower())
                {
                    case "CHUABATDAU":
                        query = query.Where(s => s.StartDate.HasValue && s.StartDate > DateTime.Now);
                        break;
                    case "DANGDIENRA":
                        query = query.Where(s => s.StartDate.HasValue && s.EndDate.HasValue && s.StartDate <= DateTime.Now && s.EndDate >= DateTime.Now);
                        break;
                    case "DAHOANTHANH":
                        query = query.Where(s => s.EndDate.HasValue && s.EndDate < DateTime.Now);
                        break;
                }
            }

            // query = request.SortOrder.ToLower() == "desc"
            //     ? query.OrderByDescending(s => EF.Property<object>(s.TeachingAssignment.Subject, request.SortColumn))
            //     : query.OrderBy(s => EF.Property<object>(s, request.SortColumn));

            query = request.SortOrder.ToLower() == "desc"
? query.OrderByDescending(s => request.SortColumn.ToLower() == "startdate"
? (object)s.StartDate
: (object)s.TeachingAssignment.Subject.Name)
: query.OrderBy(s => request.SortColumn.ToLower() == "startdate"
? (object)s.StartDate
: (object)s.TeachingAssignment.Subject.Name);



            // query = query.Skip((request.Page - 1) * request.PageSize).Take(request.PageSize);

            // Trả về danh sách kết quả
            return query.Select(s => new SessionStudentResponse
            {
                ClassId = s.TeachingAssignment.Class.Id,
                ClassCode = s.TeachingAssignment.Class.Code,
                SessionId = s.Id,
                Subject = new SubjectDto
                {
                    Id = s.TeachingAssignment.Subject.Id,
                    Name = s.TeachingAssignment.Subject.Name
                },
                Teacher = new TeacherDto
                {
                    Id = s.TeachingAssignment.User.Id,
                    Name = s.TeachingAssignment.User.FullName
                },
                Status = s.Status,
                // SessionTime = s.StartDate.HasValue ?    s.StartDate.Value : DateTime.MinValue // Thêm thời gian vào phản hồi
                SessionTime = s.StartDate.HasValue ? DateTimeUtils.FormatSessionTime(s.StartDate.Value) : "N/A"// Thay dổi dung để định dạng thời gian 
            }).ToList();
        }



    }
}
