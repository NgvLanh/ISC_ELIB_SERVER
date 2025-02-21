using AutoMapper;
using ISC_ELIB_SERVER.DTOs.Requests;
using ISC_ELIB_SERVER.DTOs.Responses;
using ISC_ELIB_SERVER.Models;
using ISC_ELIB_SERVER.Repositories;
using ISC_ELIB_SERVER.Services.Interfaces;

namespace ISC_ELIB_SERVER.Services
{
    public class TestsSubmissionService : ITestsSubmissionService
    {
        private readonly TestsSubmissionRepo _repository;
        private readonly IMapper _mapper;

        public TestsSubmissionService(TestsSubmissionRepo repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public ApiResponse<ICollection<TestsSubmissionResponse>> GetTestsSubmissiones(int page, int pageSize, string search, string sortColumn, string sortOrder)
        {
            var query = _repository.GetTestsSubmissions().AsQueryable();

            query = sortColumn switch
            {
                "Id" => sortOrder.ToLower() == "desc" ? query.OrderByDescending(ts => ts.Id) : query.OrderBy(ts => ts.Id),
                _ => query.OrderBy(us => us.Id)
            };

            var result = query.Skip((page - 1) * pageSize).Take(pageSize).ToList();

            var response = _mapper.Map<ICollection<TestsSubmissionResponse>>(result);

            return result.Any()
                    ? ApiResponse<ICollection<TestsSubmissionResponse>>.Success(response)
                    : ApiResponse<ICollection<TestsSubmissionResponse>>.NotFound("Không có dữ liệu");
        }

        public ApiResponse<TestsSubmissionResponse> GetTestsSubmissionById(long id)
        {
            var TestsSubmission = _repository.GetTestsSubmissionById(id);
            return TestsSubmission != null
                ? ApiResponse<TestsSubmissionResponse>.Success(_mapper.Map<TestsSubmissionResponse>(TestsSubmission))
                : ApiResponse<TestsSubmissionResponse>.NotFound($"Không tìm thấy thông tin #{id}");
        }


        public ApiResponse<TestsSubmissionResponse> CreateTestsSubmission(TestsSubmissionRequest TestsSubmissionRequest)
        {

            var TestsSubmissionEntity = _mapper.Map<TestsSubmission>(TestsSubmissionRequest);

            var created = _repository.CreateTestsSubmission(TestsSubmissionEntity);

            return ApiResponse<TestsSubmissionResponse>.Success(_mapper.Map<TestsSubmissionResponse>(created));
        }

        public ApiResponse<TestsSubmissionResponse> UpdateTestsSubmission(long id, TestsSubmissionRequest TestsSubmissionRequest)
        {

            // Tìm bản ghi cần cập nhật trong database
            var existingTestsSubmission = _repository.GetTestsSubmissionById(id);
            if (existingTestsSubmission == null)
            {
                return ApiResponse<TestsSubmissionResponse>.NotFound($"Không tìm thấy thông tin với Id = {id}");
            }

            // Ánh xạ dữ liệu từ request sang entity, chỉ cập nhật các trường cần thiết
            _mapper.Map(TestsSubmissionRequest, existingTestsSubmission);

            // Thực hiện cập nhật bản ghi
            var updated = _repository.UpdateTestsSubmission(existingTestsSubmission);
            return ApiResponse<TestsSubmissionResponse>.Success(_mapper.Map<TestsSubmissionResponse>(updated));
        }

        public ApiResponse<TestsSubmission> DeleteTestsSubmission(long id)
        {
            var success = _repository.DeleteTestsSubmission(id);
            return success
                ? ApiResponse<TestsSubmission>.Success()
                : ApiResponse<TestsSubmission>.NotFound("Không tìm thấy");
        }
    }
}
