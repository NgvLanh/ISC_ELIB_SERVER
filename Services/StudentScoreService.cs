using ISC_ELIB_SERVER.Models;
using ISC_ELIB_SERVER.Repositories;
using ISC_ELIB_SERVER.DTOs.Responses;
using ISC_ELIB_SERVER.DTOs.Requests;
using ISC_ELIB_SERVER.Services.Interfaces;
using AutoMapper;
using Newtonsoft.Json;

namespace ISC_ELIB_SERVER.Services
{
    public class StudentScoreService : IStudentScoreService
    {
        private readonly IStudentScoreRepo _repository;
        private readonly TestRepo _testRepo;
        private readonly IClassesRepo _classesRepo;
        private readonly UserRepo _userRepo;
        private readonly SemesterRepo _semesterRepo;
        private readonly IMapper _mapper;

        public StudentScoreService(IStudentScoreRepo repository, TestRepo testRepo, IClassesRepo classesRepo,
        UserRepo userRepo,
        SemesterRepo semesterRepo,
        IMapper mapper)
        {
            _repository = repository;
            _testRepo = testRepo;
            _classesRepo = classesRepo;
            _userRepo = userRepo;
            _semesterRepo = semesterRepo;
            _mapper = mapper;
        }

        public ApiResponse<StudentScoreResponse> GetStudentScoreById(int id)
        {
            var studentScore = _repository.GetStudentScoreById(id);
            return studentScore != null
                ? ApiResponse<StudentScoreResponse>.Success(_mapper.Map<StudentScoreResponse>(studentScore))
                : ApiResponse<StudentScoreResponse>.NotFound($"Không có dữ liệu");
        }

        public ApiResponse<StudentScoreResponse> CreateStudentScore(StudentScoreRequest studentScoreRequest)
        {
            var created = _repository.CreateStudentScore(new StudentScore()
            {
                Score = studentScoreRequest.Score,
                UserId = studentScoreRequest.UserId,
                ScoreTypeId = studentScoreRequest.ScoreTypeId,
                SubjectId = studentScoreRequest.SubjectId,
                SemesterId = studentScoreRequest.SemesterId
            });

            return ApiResponse<StudentScoreResponse>.Success(_mapper.Map<StudentScoreResponse>(created));
        }

        public ApiResponse<StudentScoreResponse> UpdateStudentScore(int id, StudentScoreRequest studentScoreRequest)
        {
            var existingStudentScore = _repository.GetStudentScoreById(id);

            if (existingStudentScore == null)
            {
                return ApiResponse<StudentScoreResponse>.NotFound($"Không tìm thấy StudentScore với ID {id}");
            }
            existingStudentScore.Score = studentScoreRequest.Score;
            existingStudentScore.UserId = studentScoreRequest.UserId;
            existingStudentScore.ScoreTypeId = studentScoreRequest.ScoreTypeId;
            existingStudentScore.SubjectId = studentScoreRequest.SubjectId;
            existingStudentScore.SemesterId = studentScoreRequest.SemesterId;

            var updated = _repository.UpdateStudentScore(existingStudentScore);

            return ApiResponse<StudentScoreResponse>.Success(_mapper.Map<StudentScoreResponse>(updated));
        }

        public ApiResponse<StudentScore> DeleteStudentScore(int id)
        {
            var success = _repository.DeleteStudentScore(id);
            return success
                ? ApiResponse<StudentScore>.Success()
                : ApiResponse<StudentScore>.NotFound("Không tìm thấy để xóa");
        }

        public ApiResponse<ICollection<StudentScoreResponse>> GetStudentScores(int? page, int? pageSize, string? sortColumn, string? sortOrder)
        {
            var query = _repository.GetStudentScores().AsQueryable();

            query = sortColumn switch
            {
                "Id" => sortOrder.ToLower() == "desc" ? query.OrderByDescending(us => us.Id) : query.OrderBy(us => us.Id),
                _ => query.OrderBy(ay => ay.Id)
            };


            if (page.HasValue && pageSize.HasValue)
            {
                query = query.Skip((page.Value - 1) * pageSize.Value).Take(pageSize.Value);
            }

            var result = query.ToList();

            var response = _mapper.Map<ICollection<StudentScoreResponse>>(result);

            return result.Any() ? ApiResponse<ICollection<StudentScoreResponse>>
            .Success(response, page, pageSize, _repository.GetStudentScores().Count)
             : ApiResponse<ICollection<StudentScoreResponse>>.NotFound("Không có dữ liệu");
        }

        public ApiResponse<StudentScoreDashboardResponse> ViewStudentDashboardScores(int? academicYearId, int? classId, int? gradeLevelId, int? subjectId)
        {
            if (academicYearId == null)
                return ApiResponse<StudentScoreDashboardResponse>.Fail("Thiếu năm học");

            if (classId == null)
                return ApiResponse<StudentScoreDashboardResponse>.Fail("Thiếu lớp");

            if (gradeLevelId == null)
                return ApiResponse<StudentScoreDashboardResponse>.Fail("Thiếu khối");

            if (subjectId == null)
                return ApiResponse<StudentScoreDashboardResponse>.Fail("Thiếu môn học");

            var testOfSubject = _mapper.Map<StudentScoreByTestResponse>(_testRepo.GetTestsBySubjectId((int)subjectId));
            if (testOfSubject == null)
                return ApiResponse<StudentScoreDashboardResponse>.Fail("Không có bài kiểm tra cho môn học này");

            var classTest = _mapper.Map<ClassScoreResponse>(_classesRepo.GetClassById(classId ?? 0));
            if (classTest == null)
                return ApiResponse<StudentScoreDashboardResponse>.Fail("Không tìm thấy lớp học");

            var studentsOfClass = _mapper.Map<ICollection<StudentResponse>>(_userRepo.GetUsersByClassId((int)classId));
            if (studentsOfClass == null || !studentsOfClass.Any())
                return ApiResponse<StudentScoreDashboardResponse>.Fail("Không tìm thấy sinh viên trong lớp");

            var semesterOfAcademicYear = _mapper.Map<ICollection<SemesterScoreResponse>>(_semesterRepo.GetSemestersByAcademicYearId(academicYearId ?? 0));
            // Console.WriteLine(JsonConvert.SerializeObject(semesterOfAcademicYear, Formatting.Indented));
            foreach (var student in studentsOfClass)
            {
                student.Semesters = semesterOfAcademicYear.Select(semester => new SemesterScoreResponse
                {
                    Id = semester.Id,
                    Name = semester.Name,
                    Scores = _mapper.Map<ICollection<ScoreResponse>>(
                            _repository.GetStudentScoresByUserIdAndSubjectIdAndSemesterId(student.Id, (int)subjectId, semester.Id)),
                    AverageScore = _mapper.Map<ICollection<ScoreResponse>>(
                            _repository.GetStudentScoresByUserIdAndSubjectIdAndSemesterId(student.Id, (int)subjectId, semester.Id)).Any()
                            ? _mapper.Map<ICollection<ScoreResponse>>(
                            _repository.GetStudentScoresByUserIdAndSubjectIdAndSemesterId(student.Id, (int)subjectId, semester.Id)).Sum(s => s.Score * s.ScoreType.Weight) / _mapper.Map<ICollection<ScoreResponse>>(
                            _repository.GetStudentScoresByUserIdAndSubjectIdAndSemesterId(student.Id, (int)subjectId, semester.Id)).Sum(s => s.ScoreType.Weight)
                            : 0
                }).ToList();

                student.AverageScore = student.Semesters.Any()
                    ? student.Semesters.Average(s => s.AverageScore)
                    : 0;

                student.Passed = student.AverageScore >= 5.0;
                student.LastUpdate = DateTime.Now;
            }


            testOfSubject.Class = classTest;

            var dashboardResponse = new StudentScoreDashboardResponse
            {
                Test = testOfSubject,
                Students = studentsOfClass
            };

            return ApiResponse<StudentScoreDashboardResponse>.Success(dashboardResponse);
        }

    }
}
