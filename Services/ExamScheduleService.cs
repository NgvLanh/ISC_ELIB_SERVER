using AutoMapper;
using ISC_ELIB_SERVER.DTOs.Responses;
using ISC_ELIB_SERVER.DTOs.Requests;
using ISC_ELIB_SERVER.Models;
using ISC_ELIB_SERVER.Repositories;
using System.Collections.Generic;

namespace ISC_ELIB_SERVER.Services
{

    public interface IExamScheduleService
    {
        ApiResponse<PagedResult<ExamScheduleResponse>> GetAll(
        int page,
        int pageSize,
        string? search,
        string? sortBy,
        bool isDescending,
        int? academicYearId,
        int? semesterId,
            int? gradeLevelsId,  
        int? classId
    );
        ApiResponse<List<CalendarExamResponse>> GetForCalendarStructured(
        int academicYearId, int? semesterId, int? gradeLevelsId, int? classId);
        ApiResponse<ExamScheduleDetailResponse> GetScheduleWithClasses(long id);

        ApiResponse<ExamScheduleResponse> GetById(long id);
        ApiResponse<List<ExamScheduleResponse>> Create(ExamScheduleRequest request);
        ApiResponse<ExamScheduleResponse> Update(long id, ExamScheduleRequest request);
        ApiResponse<object> Delete(long id);

    }
    public class ExamScheduleService: IExamScheduleService
    {
        private readonly ExamScheduleRepo _repository;
        private readonly IMapper _mapper;

        public ExamScheduleService(ExamScheduleRepo repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public ApiResponse<PagedResult<ExamScheduleResponse>> GetAll(int page, int pageSize, string? search, string? sortBy, bool isDescending, int? academicYearId, int? semesterId, int? gradeLevelsId,  
    int? classId)
        {
            var entities = _repository.GetAll(page, pageSize, search, sortBy, isDescending, academicYearId, semesterId,   gradeLevelsId, 
    classId );
            var responses = _mapper.Map<ICollection<ExamScheduleResponse>>(entities.Items);

            var result = new PagedResult<ExamScheduleResponse>(responses, entities.TotalItems, page, pageSize);
            return ApiResponse<PagedResult<ExamScheduleResponse>>.Success(result);
        }


        public ApiResponse<List<CalendarExamResponse>> GetForCalendarStructured(
            int academicYearId, int? semesterId, int? gradeLevelsId, int? classId)
        {
            // 1. Lấy entities
            var entities = _repository.GetForCalendar(academicYearId, semesterId, gradeLevelsId, classId);

            // 2. Map sang DTO cơ bản
            var exams = _mapper.Map<List<ExamScheduleResponse>>(entities);

            // 3. Lọc bỏ record không có ngày thi
            var withDate = exams.Where(e => e.ExamDay.HasValue).ToList();

            // 4. Nhóm: Year → Month → Day
            var calendar = withDate
                .GroupBy(e => e.ExamDay.Value.Year)
                .Select(yearG => new CalendarExamResponse
                {
                    Year = yearG.Key,
                    Months = yearG
                        .GroupBy(e => e.ExamDay.Value.Month)
                        .Select(monthG => new CalendarMonth
                        {
                            Month = monthG.Key,
                            Days = monthG
                                .GroupBy(e => e.ExamDay.Value.Day)
                                .Select(dayG => new CalendarDay
                                {
                                    Day = dayG.Key,
                                    Exams = dayG
                                        .OrderBy(e => e.ExamDay)
                                        .ToList()
                                })
                                .OrderBy(d => d.Day)
                                .ToList()
                        })
                        .OrderBy(m => m.Month)
                        .ToList()
                })
                .OrderBy(c => c.Year)
                .ToList();

            return ApiResponse<List<CalendarExamResponse>>.Success(calendar);
        }

        public ApiResponse<ExamScheduleDetailResponse> GetScheduleWithClasses(long id)
        {
            var entity = _repository.GetDetailWithClasses(id);
            if (entity == null)
                return ApiResponse<ExamScheduleDetailResponse>.NotFound("ExamSchedule không tồn tại");

            // Mapping thủ công
            var response = new ExamScheduleDetailResponse
            {
                Id = entity.Id,
                Name = entity.Name,
                ExamDay = entity.ExamDay,

                // Dùng đúng property DTO
                DurationInMinutes = entity.duration_in_minutes,

                Type = entity.Type,
                Form = entity.Form,
                Status = (int)entity.Status,
                StatusName = entity.Status.ToString(),

                AcademicYearId = entity.AcademicYearId ?? 0,
                Subject = entity.Subject ?? 0,
                SemesterId = entity.SemesterId ?? 0,
                GradeLevelsId = entity.GradeLevelsId ?? 0,

                AcademicYear = (entity.AcademicYear != null
                    && entity.AcademicYear.StartTime.HasValue
                    && entity.AcademicYear.EndTime.HasValue)
                    ? $"{entity.AcademicYear.StartTime.Value:yyyy} - {entity.AcademicYear.EndTime.Value:yyyy}"
                    : null,

                SubjectName = entity.SubjectNavigation?.Name,

                // Chuyển sang dùng 2 trường mới
                SemesterName = entity.Semester?.Name,
                GradeLevelName = entity.GradeLevels?.Name,

                TeacherNames = (entity.Exam?.ExamGraders != null
                    ? entity.Exam.ExamGraders.Select(eg => eg.User.FullName).ToList()
                    : new List<string>())
            };

            // Map ParticipatingClasses với đúng DTO
            response.ParticipatingClasses = entity.ExamScheduleClasses
      .Select(esc => new ExamScheduleClassDetailDto
      {
          ClassId = esc.Class!.Id,
          ClassCode = esc.Class.Code,
          ClassName = esc.Class.Name,
          SupervisoryTeacherName = esc.Class.User?.FullName,      // homeroom teacher
          StudentQuantity = esc.Class.StudentQuantity ?? 0,
          JoinedExamStudentQuantity = esc.joined_student_quantity,
          ExamGraders = esc.ExamGraders
              .Where(eg => eg.User != null)
              .Select(eg => eg.User.FullName!)
              .ToList()
      })
      .ToList();

            return ApiResponse<ExamScheduleDetailResponse>.Success(response);
        }
        public ApiResponse<ExamScheduleResponse> GetById(long id)
        {
            var entity = _repository.GetById(id);
            if (entity == null) return ApiResponse<ExamScheduleResponse>.NotFound("ExamSchedule không tồn tại");

            var response = _mapper.Map<ExamScheduleResponse>(entity);
            return ApiResponse<ExamScheduleResponse>.Success(response);
        }

        public ApiResponse<List<ExamScheduleResponse>> Create(ExamScheduleRequest request)
        {
            var responses = new List<ExamScheduleResponse>();

            try
            {
                foreach (var semesterId in request.SemesterIds)
                {
                    var entity = _mapper.Map<ExamSchedule>(request);
                    entity.SemesterId = semesterId;

                    // Gán danh sách lớp thi
                    if (request.ClassIds != null && request.ClassIds.Any())
                    {
                        foreach (var classId in request.ClassIds)
                        {
                            var examScheduleClass = new ExamScheduleClass
                            {
                                ClassId = classId,
                                Active = true
                            };
                            entity.ExamScheduleClasses.Add(examScheduleClass);
                        }
                    }

                    _repository.Create(entity);

                    // Gán danh sách giám thị cho từng lớp
                                foreach (var gf in request.GradersForClasses)
                                    {
                        var esc = entity.ExamScheduleClasses
                                                        .FirstOrDefault(x => x.ClassId == gf.ClassId);
                                        if (esc != null && gf.GraderIds.Any())
                                            {
                            _repository.AddGraders(entity.Id, esc.Id, gf.GraderIds);
                                            }
                                   }

                    var response = _mapper.Map<ExamScheduleResponse>(entity);
                    responses.Add(response);
                }

                return ApiResponse<List<ExamScheduleResponse>>.Success(responses);
            }
            catch (Exception ex)
            {
                return ApiResponse<List<ExamScheduleResponse>>.Error(new Dictionary<string, string[]>
        {
            { "Exception", new[] { ex.Message } }
        });
            }
        }

        public ApiResponse<ExamScheduleResponse> Update(long id, ExamScheduleRequest request)
        {
            var entity = _repository.GetById(id);
            if (entity == null)
                return ApiResponse<ExamScheduleResponse>.NotFound("ExamSchedule không tồn tại");

            try
            {
                // Xóa các lớp và giám thị cũ
                _repository.RemoveAllClassesAndGraders(entity.Id);

                // Map lại thông tin cơ bản
                _mapper.Map(request, entity);

                // Tạo lại danh sách lớp thi
                entity.ExamScheduleClasses = new List<ExamScheduleClass>();
                if (request.ClassIds != null && request.ClassIds.Any())
                {
                    foreach (var classId in request.ClassIds)
                    {
                        entity.ExamScheduleClasses.Add(new ExamScheduleClass
                        {
                            ClassId = classId,
                            Active = true
                        });
                    }
                }

                // Cập nhật thông tin ExamSchedule và danh sách lớp
                _repository.Update(entity);

                // Gán lại giám thị (sau khi có ExamScheduleClasses mới)
                foreach (var item in request.GradersForClasses)
                {
                    var examClass = entity.ExamScheduleClasses
                        .FirstOrDefault(x => x.ClassId == item.ClassId);

                    if (examClass != null && item.GraderIds != null)
                    {
                        _repository.AddGraders(entity.Id, examClass.Id, item.GraderIds);
                    }
                }

                var response = _mapper.Map<ExamScheduleResponse>(entity);
                return ApiResponse<ExamScheduleResponse>.Success(response);
            }
            catch (Exception ex)
            {
                return ApiResponse<ExamScheduleResponse>.Error(new Dictionary<string, string[]>
        {
            { "Exception", new[] { ex.Message } }
        });
            }
        }


        public ApiResponse<object> Delete(long id)
        {
            var entity = _repository.GetById((int)id);
            if (entity == null)
            {
                return ApiResponse<object>.NotFound("ExamSchedule không tồn tại");
            }

            try
            {
                var result = _repository.Delete((int)id);
                return result
                    ? ApiResponse<object>.Success()
                    : ApiResponse<object>.Conflict("Không thể xóa ExamSchedule");
            }
            catch (Exception ex)
            {
                return ApiResponse<object>.Error(new Dictionary<string, string[]>
        {
            { "Exception", new[] { ex.Message } }
        });
            }
        }


    }

}

