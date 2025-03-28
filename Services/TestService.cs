using AutoMapper;
using ISC_ELIB_SERVER.DTOs.Requests;
using ISC_ELIB_SERVER.DTOs.Responses;
using ISC_ELIB_SERVER.Models;
using ISC_ELIB_SERVER.Repositories;

namespace ISC_ELIB_SERVER.Services
{
        public interface ITestService
        {
            ApiResponse<ICollection<TestResponse>> GetTestes(int page, int pageSize, string search, string sortColumn, string sortOrder);
            ApiResponse<TestResponse> GetTestById(long id);
            ApiResponse<TestResponse> GetTestByName(string name);
            ApiResponse<TestResponse> CreateTest(TestRequest TestRequest);
            ApiResponse<Test> UpdateTest(long id, TestRequest Test);
            ApiResponse<Test> DeleteTest(long id);
        }


        public class TestService : ITestService
        {
            private readonly TestRepo _repository;
            private readonly IMapper _mapper;

            public TestService(TestRepo repository, IMapper mapper)
            {
                _repository = repository;
                _mapper = mapper;
            }

            public ApiResponse<ICollection<TestResponse>> GetTestes(int page, int pageSize, string search, string sortColumn, string sortOrder)
            {
                var query = _repository.GetTests().AsQueryable();

                if (!string.IsNullOrEmpty(search))
                {
                    query = query.Where(ts => ts.Name.ToLower().Contains(search.ToLower()));
                }

                query = sortColumn switch
                {
                    "Name" => sortOrder.ToLower() == "desc" ? query.OrderByDescending(ts => ts.Name) : query.OrderBy(ts => ts.Name),
                    "Id" => sortOrder.ToLower() == "desc" ? query.OrderByDescending(ts => ts.Id) : query.OrderBy(ts => ts.Id),
                    _ => query.OrderBy(us => us.Id)
                };

                var result = query.Skip((page - 1) * pageSize).Take(pageSize).ToList();

            var response = _mapper.Map<ICollection<TestResponse>>(result);

            return result.Any()
                    ? ApiResponse<ICollection<TestResponse>>.Success(response)
                    : ApiResponse<ICollection<TestResponse>>.NotFound("Không có dữ liệu");
            }

            public ApiResponse<TestResponse> GetTestById(long id)
            {
                var Test = _repository.GetTestById(id);
                return Test != null
                    ? ApiResponse<TestResponse>.Success(_mapper.Map<TestResponse>(Test))
                    : ApiResponse<TestResponse>.NotFound($"Không tìm thấy bài kiểm tra #{id}");
            }

            public ApiResponse<TestResponse> GetTestByName(string name)
            {
                var Test = _repository.GetTests().FirstOrDefault(ts => ts.Name?.ToLower() == name.ToLower());
                return Test != null
                    ? ApiResponse<TestResponse>.Success(_mapper.Map<TestResponse>(Test))
                    : ApiResponse<TestResponse>.NotFound($"Không tìm thấy bài kiểm tra có tên: {name}");
            }

            public ApiResponse<TestResponse> CreateTest(TestRequest TestRequest)
            {
            //var existing = _repository.GetTests().FirstOrDefault(ts => ts.Name?.ToLower() == TestRequest.Name.ToLower());
            //if (existing != null)
            //{
            //    return ApiResponse<Test>.Conflict("Tên bài kiểm tra đã tồn tại");
            //}

            var testEntity = _mapper.Map<Test>(TestRequest);

            // Tạo mới bài kiểm tra
            var created = _repository.CreateTest(testEntity);

            // Trả về kết quả với kiểu TestResponse
            return ApiResponse<TestResponse>.Success(_mapper.Map<TestResponse>(created));
        }

        public ApiResponse<Test> UpdateTest(long id,TestRequest testRequest)
        {
            
                // Tìm bản ghi cần cập nhật trong database
                var existingTest = _repository.GetTestById(id);
                if (existingTest == null)
                {
                    return ApiResponse<Test>.NotFound($"Không tìm thấy bài kiểm tra với Id = {id}");
                }

                // Ánh xạ dữ liệu từ request sang entity, chỉ cập nhật các trường cần thiết
                _mapper.Map(testRequest, existingTest);

                // Thực hiện cập nhật bản ghi
                var updated = _repository.UpdateTest(existingTest);
                return ApiResponse<Test>.Success(updated);
        }

        public ApiResponse<Test> DeleteTest(long id)
            {
                var success = _repository.DeleteTest(id);
                return success
                    ? ApiResponse<Test>.Success()
                    : ApiResponse<Test>.NotFound("Không tìm thấy bài kiểm tra dùng để xóa");
            }
        } 
    }

