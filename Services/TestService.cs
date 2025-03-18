using AutoMapper;
using ISC_ELIB_SERVER.DTOs.Requests;
using ISC_ELIB_SERVER.DTOs.Responses;
using ISC_ELIB_SERVER.Models;
using ISC_ELIB_SERVER.Repositories;
using ISC_ELIB_SERVER.Services.Interfaces;
using Sprache;
using System.Xml.Linq;
using System.Security.Claims;

namespace ISC_ELIB_SERVER.Services
{
       
    public class TestService : ITestService
    {
            private readonly TestRepo _testRepo;
            private readonly SemesterRepo _semesterRepo;
        private readonly SubjectRepo _subjectRepo;
            private readonly UserRepo _userRepo;
            private readonly IMapper _mapper;

            public TestService(TestRepo testRepo, IMapper mapper, SemesterRepo semesterRepo, UserRepo userRepo, SubjectRepo subjectRepo)
            {
                _testRepo = testRepo;
                _userRepo = userRepo;
            _semesterRepo = semesterRepo;
            _subjectRepo = subjectRepo;
                _mapper = mapper;
            }

            public ApiResponse<ICollection<TestResponse>> GetTestes(int? page, int? pageSize, string? search, string? sortColumn, string? sortOrder)
            {
                var query = _testRepo.GetTests().AsQueryable();

                query = query.Where(qr => qr.Active.HasValue && qr.Active.Value);

                if (!string.IsNullOrEmpty(search))
                {
                    query = query.Where(ts => ts.Name.ToLower().Contains(search.ToLower()));
                }

                query = sortColumn?.ToLower() switch
                {
                    "name" => sortOrder?.ToLower() == "desc" ? query.OrderByDescending(ts => ts.Name) : query.OrderBy(ts => ts.Name),
                    "id" => sortOrder?.ToLower() == "desc" ? query.OrderByDescending(ts => ts.Id) : query.OrderBy(ts => ts.Id),
                    _ => query.OrderBy(us => us.Id)
                };
                query = query.Where(qr => qr.Active == true);

                var total = query.Count();

            
                if (page.HasValue && pageSize.HasValue)
                {
                    query = query.Skip((page.Value - 1) * pageSize.Value).Take(pageSize.Value);
                }
                var result = query.ToList();

                var response = _mapper.Map<ICollection<TestResponse>>(result);

                return result.Any()
                        ? ApiResponse<ICollection<TestResponse>>.Success(
                                data: response,
                                totalItems: total,
                                pageSize: pageSize,
                                page: page
                            )
                        : ApiResponse<ICollection<TestResponse>>.NotFound("Không có dữ liệu");
            }

            public ApiResponse<TestResponse> GetTestById(long id)
            {
                var Test = _testRepo.GetTestById(id);
                return Test != null
                    ? ApiResponse<TestResponse>.Success(_mapper.Map<TestResponse>(Test))
                    : ApiResponse<TestResponse>.NotFound($"Không tìm thấy bài kiểm tra #{id}");
            }

            public ApiResponse<TestResponse> GetTestByName(string name)
            {
                

                var Test = _testRepo.GetTests().FirstOrDefault(ts => ts.Name?.ToLower() == name.ToLower());
                return Test != null
                    ? ApiResponse<TestResponse>.Success(_mapper.Map<TestResponse>(Test))
                    : ApiResponse<TestResponse>.NotFound($"Không tìm thấy bài kiểm tra có tên: {name}");
            }

        public ApiResponse<TestResponse> CreateTest(TestRequest testRequest, string? idUser)
        {
            if (string.IsNullOrEmpty(idUser))
            {
                return ApiResponse<TestResponse>.Fail("Không tìm thấy ID trong token");
            }

            if (!int.TryParse(idUser, out int userId))
            {
                return ApiResponse<TestResponse>.Fail("ID trong token không hợp lệ");
            }

            var semester = _semesterRepo.GetSemesterById(testRequest.SemesterId);
            if( semester == null)
            {
                return ApiResponse<TestResponse>.NotFound($"Học kỳ có id {testRequest.SemesterId} không tồn tạ!!!");
            }
            var subject = _subjectRepo.GetSubjectById(testRequest.SubjectId);
            if (subject == null)
            {
                return ApiResponse<TestResponse>.NotFound($"Môn học có id {testRequest.SubjectId} không tồn tạ!!!");
            }

            var user = _userRepo.GetUserById(userId);
            if (user == null)
            {
                return ApiResponse<TestResponse>.NotFound($"Người dùng có id {userId} không tồn tạ!!!");
            }

            var testEntity = _mapper.Map<Test>(testRequest);
          
            testEntity.StartTime = testEntity.StartTime.HasValue?DateTime.SpecifyKind(testEntity.StartTime.Value, DateTimeKind.Unspecified):null;
            testEntity.EndTime = testEntity.EndTime.HasValue ? DateTime.SpecifyKind(testEntity.EndTime.Value, DateTimeKind.Unspecified) : null;
            // Tạo mới bài kiểm tra
            var created = _testRepo.CreateTest(testEntity);

            // Trả về kết quả với kiểu TestResponse
            return ApiResponse<TestResponse>.Success(_mapper.Map<TestResponse>(created));
         }

    public ApiResponse<TestResponse> UpdateTest(long id,TestRequest testRequest, string? idUser)
    {
            if (string.IsNullOrEmpty(idUser))
            {
                return ApiResponse<TestResponse>.Fail("Không tìm thấy ID trong token");
            }

            if (!int.TryParse(idUser, out int userId))
            {
                return ApiResponse<TestResponse>.Fail("ID trong token không hợp lệ");
            }


            // Tìm bản ghi cần cập nhật trong database
            var existingTest = _testRepo.GetTestById(id);
                if (existingTest == null)
                {
                    return ApiResponse<TestResponse>.NotFound($"Bài kiểm tra có id {id} không tồn tạ!!!");
                }

            var semester = _semesterRepo.GetSemesterById(testRequest.SemesterId);
            if (semester == null)
            {
                return ApiResponse<TestResponse>.NotFound($"Học kỳ có id {testRequest.SemesterId} không tồn tạ!!!");
            }
            var subject = _subjectRepo.GetSubjectById(testRequest.SubjectId);
            if (subject == null)
            {
                return ApiResponse<TestResponse>.NotFound($"Môn học có id {testRequest.SubjectId} không tồn tạ!!!");
            }

            var user = _userRepo.GetUserById(userId);
            if (user == null)
            {
                return ApiResponse<TestResponse>.NotFound($"Người dùng có id {userId} không tồn tạ!!!");
            }

            // Ánh xạ dữ liệu từ request sang entity, chỉ cập nhật các trường cần thiết
            _mapper.Map(testRequest, existingTest);
            existingTest.StartTime = existingTest.StartTime.HasValue ? DateTime.SpecifyKind(existingTest.StartTime.Value, DateTimeKind.Unspecified) : null;
            existingTest.EndTime = existingTest.EndTime.HasValue ? DateTime.SpecifyKind(existingTest.EndTime.Value, DateTimeKind.Unspecified) : null;

            // Thực hiện cập nhật bản ghi
            var updated = _testRepo.UpdateTest(existingTest);
                return ApiResponse<TestResponse>.Success(_mapper.Map<TestResponse>(updated));
     }

    public ApiResponse<Test> DeleteTest(long id)
         {
                var success = _testRepo.DeleteTest(id);
                return success
                    ? ApiResponse<Test>.Success()
                    : ApiResponse<Test>.NotFound("Không tìm thấy bài kiểm tra để xóa");
         }
    } 
}

