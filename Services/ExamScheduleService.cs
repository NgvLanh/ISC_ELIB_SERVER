using AutoMapper;
using ISC_ELIB_SERVER.DTOs.Responses;
using ISC_ELIB_SERVER.DTOs.Requests;
using ISC_ELIB_SERVER.Models;
using ISC_ELIB_SERVER.Repositories;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

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
            foreach (var response in responses)
            {
                var entity = entities.Items.FirstOrDefault(e => e.Id == response.Id);
                if (entity != null)
                {
                    response.ClassNames = entity.ExamScheduleClasses?
                        .Select(esc => esc.Class?.Name)
                        .Where(name => name != null)
                        .ToList();
                }
            }
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
            // 1. Lấy entity từ repository
            var entity = _repository.GetDetailWithClasses(id);
            if (entity == null)
            {
                // 2. Nếu không tìm thấy, return NotFound
                return ApiResponse<ExamScheduleDetailResponse>.NotFound("ExamSchedule không tồn tại");
            }

            // 3. Tạo đối tượng response
            var response = new ExamScheduleDetailResponse
            {
                Id = entity.Id,
                Name = entity.Name,
                ExamDay = entity.ExamDay,

                // Chú ý dùng thuộc tính DurationInMinutes (DTO) = duration_in_minutes (entity)
                DurationInMinutes = entity.duration_in_minutes,

                Type = entity.Type,
                Form = entity.Form,
                Status = (int)entity.Status,
                StatusName = entity.Status.ToString(),
                AcademicYearId = entity.AcademicYearId ?? 0,
                Subject = entity.Subject ?? 0,

                // Lưu ý: Nếu entity.SemesterId là nullable thì ta kiểm tra:
                SemesterId = entity.SemesterId.HasValue ? entity.SemesterId.Value : 0,

                GradeLevelsId = entity.GradeLevelsId ?? 0,


                // Các trường quan hệ khác
                AcademicYear = entity.AcademicYear != null
                    ? $"{entity.AcademicYear.StartTime:yyyy} - {entity.AcademicYear.EndTime:yyyy}"
                    : null,

                SemesterName = entity.Semester?.Name,
                GradeLevelName = entity.GradeLevels?.Name,
                SubjectName = entity.SubjectNavigation?.Name,

                // Lấy danh sách giám thị (TeacherNames)
                TeacherNames = entity.ExamScheduleClasses
                    .SelectMany(esc => esc.ExamGraders)
                    .Where(eg => eg.User != null)
                    .Select(eg => eg.User.FullName)
                    .Distinct()
                    .ToList()
            };

            // 4. Gán ParticipatingClasses
            response.ParticipatingClasses = entity.ExamScheduleClasses.Select(esc => new ExamScheduleClassDetailDto
            {
                ClassId = esc.Class!.Id,
                ClassCode = esc.Class.Code,
                ClassName = esc.Class.Name,
                SupervisoryTeacherName = esc.Class.User?.FullName,
                StudentQuantity = esc.Class.StudentQuantity ?? 0,
                JoinedExamStudentQuantity = esc.joined_student_quantity,
                ExamGraders = esc.ExamGraders
                    .Where(eg => eg.User != null)
                    .Select(eg => eg.User.FullName)
                    .ToList()
            }).ToList();

            // 5. Return success
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
                    // 1. Map request → entity
                    var entity = _mapper.Map<ExamSchedule>(request);

                    // 2. Gán thêm semester và examId
                    entity.SemesterId = semesterId;
                    entity.ExamId = request.ExamId;    // ← quan trọng

                    // 3. Tạo các ExamScheduleClass
                    foreach (var classId in request.ClassIds)
                    {
                        entity.ExamScheduleClasses.Add(new ExamScheduleClass
                        {
                            ClassId = classId,
                            Active = true
                        });
                    }

                    // 4. Lưu ExamSchedule (để entity.Id và entity.ExamId tồn tại)
                    _repository.Create(entity);

                    // 5. Gán giám thị (ExamGrader) sử dụng đúng ExamId
                    foreach (var gf in request.GradersForClasses)
                    {
                        var esc = entity.ExamScheduleClasses
                                        .FirstOrDefault(x => x.ClassId == gf.ClassId);
                        if (esc != null && gf.GraderIds.Any())
                        {
                            // ← truyền entity.ExamId, không phải entity.Id
                            _repository.AddGraders(entity.ExamId.Value, esc.Id, gf.GraderIds);
                        }
                    }

                    // 6. Map lại response
                    var resp = _mapper.Map<ExamScheduleResponse>(entity);
                    responses.Add(resp);
                }

                return ApiResponse<List<ExamScheduleResponse>>.Success(responses);
            }
            catch (DbUpdateException dbEx)
            {
                // Bắt lỗi DB để xem chi tiết
                var msg = dbEx.InnerException?.Message ?? dbEx.Message;
                return ApiResponse<List<ExamScheduleResponse>>.Error(new Dictionary<string, string[]>
        {
            { "DbUpdateException", new[] { msg } }
        });
            }
        }

        public ApiResponse<ExamScheduleResponse> Update(long id, ExamScheduleRequest request)
        {
            // Sử dụng phương thức lấy entity ở trạng thái tracking
            var entity = _repository.GetByIdForUpdate(id);
            if (entity == null)
                return ApiResponse<ExamScheduleResponse>.NotFound("ExamSchedule không tồn tại");
            try
            {
                // 1. Xóa các lớp và graders cũ
                _repository.RemoveAllClassesAndGraders(entity.Id);

                // 2. Cập nhật các thuộc tính cơ bản từ request
                entity.Name = request.Name;
                entity.ExamDay = request.ExamDay;
                entity.Type = request.Type;
                entity.Form = request.Form;
                if (request.Status.HasValue)
                    entity.Status = (Enums.ExamStatus)request.Status.Value;
                entity.AcademicYearId = request.AcademicYearId;
                entity.Subject = request.Subject;

                // Lưu ý: Kiểm tra trường hợp SemesterIds rỗng
                entity.SemesterId = request.SemesterIds.Any()
                                    ? request.SemesterIds.First()
                                    : entity.SemesterId;

                entity.GradeLevelsId = request.GradeLevelsId;
                entity.duration_in_minutes = request.duration_in_minutes;

                // 3. Tái tạo danh sách ExamScheduleClasses mới
                // Khi xóa, các collection cũ đã bị loại bỏ nên khởi tạo lại
                entity.ExamScheduleClasses = request.ClassIds
                    .Select(cid => new ExamScheduleClass { ClassId = cid, Active = true })
                    .ToList();

                // 4. Cập nhật entity (với tracking EF sẽ tự nhận biết thay đổi)
                _repository.Update(entity);

                // 5. Thêm lại danh sách graders cho từng lớp vừa tạo
                foreach (var gf in request.GradersForClasses)
                {
                    var esc = entity.ExamScheduleClasses.FirstOrDefault(x => x.ClassId == gf.ClassId);
                    if (esc != null && gf.GraderIds.Any())
                    {
                        // Chú ý: Sử dụng lại ExamId của entity (đảm bảo đã có giá trị)
                        _repository.AddGraders(entity.ExamId!.Value, esc.Id, gf.GraderIds);
                    }
                }

                // 6. Map lại response DTO
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

