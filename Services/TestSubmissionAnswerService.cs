using ISC_ELIB_SERVER.Models;
using ISC_ELIB_SERVER.Repositories;
using ISC_ELIB_SERVER.DTOs.Responses;
using ISC_ELIB_SERVER.DTOs.Requests;
using AutoMapper;

namespace ISC_ELIB_SERVER.Services
{
    public interface ITestSubmissionAnswerService
    {
        ApiResponse<ICollection<TestSubmissionAnswerResponse>> GetTestSubmissionAnswers(int page, int pageSize, string search, string sortColumn, string sortOrder);
        ApiResponse<TestSubmissionAnswerResponse> GetTestSubmissionAnswerById(int id);
        ApiResponse<TestSubmissionAnswerResponse> CreateTestSubmissionAnswer(TestSubmissionAnswerRequest testSubmissionAnswerRequest);
        ApiResponse<TestSubmissionAnswerResponse> UpdateTestSubmissionAnswer(int id, TestSubmissionAnswerRequest testSubmissionAnswerRequest);
        ApiResponse<TestSubmissionAnswerResponse> DeleteTestSubmissionAnswer(int id);
    }

    public class TestSubmissionAnswerService : ITestSubmissionAnswerService
    {
        private readonly TestSubmissionAnswerRepo _repository;
        private readonly TestAnswerRepo _testAnswerRepo; // Repository kiểm tra đáp án đúng
        private readonly IMapper _mapper;

        public TestSubmissionAnswerService(TestSubmissionAnswerRepo repository, TestAnswerRepo testAnswerRepo, IMapper mapper)
        {
            _repository = repository;
            _testAnswerRepo = testAnswerRepo;
            _mapper = mapper;
        }

        public ApiResponse<ICollection<TestSubmissionAnswerResponse>> GetTestSubmissionAnswers(int page, int pageSize, string search, string sortColumn, string sortOrder)
        {
            var testSubmissionAnswers = _repository.GetAllTestSubmissionAnswers().ToList(); // Lấy toàn bộ danh sách

            // Lọc dữ liệu
            if (!string.IsNullOrEmpty(search))
            {
                testSubmissionAnswers = testSubmissionAnswers
                    .Where(t => t.AnswerText != null && t.AnswerText.Contains(search))
                    .ToList();
            }

            // Sắp xếp dữ liệu
            testSubmissionAnswers = sortColumn?.ToLower() switch
            {
                "id" => sortOrder.ToLower() == "desc"
                    ? testSubmissionAnswers.OrderByDescending(t => t.Id).ToList()
                    : testSubmissionAnswers.OrderBy(t => t.Id).ToList(),

                "answertext" => sortOrder.ToLower() == "desc"
                    ? testSubmissionAnswers.OrderByDescending(t => t.AnswerText).ToList()
                    : testSubmissionAnswers.OrderBy(t => t.AnswerText).ToList(),

                _ => testSubmissionAnswers.OrderBy(t => t.Id).ToList() // Mặc định sắp xếp theo ID
            };

            // Phân trang
            var paginatedResult = testSubmissionAnswers
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            var mappedData = _mapper.Map<ICollection<TestSubmissionAnswerResponse>>(paginatedResult);

            return ApiResponse<ICollection<TestSubmissionAnswerResponse>>.Success(mappedData);
        }

        public ApiResponse<TestSubmissionAnswerResponse> GetTestSubmissionAnswerById(int id)
        {
            var testSubmissionAnswer = _repository.GetTestSubmissionAnswerById(id);
            return testSubmissionAnswer != null
                ? ApiResponse<TestSubmissionAnswerResponse>.Success(_mapper.Map<TestSubmissionAnswerResponse>(testSubmissionAnswer))
                : ApiResponse<TestSubmissionAnswerResponse>.NotFound("Không tìm thấy câu trả lời.");
        }

        public ApiResponse<TestSubmissionAnswerResponse> CreateTestSubmissionAnswer(TestSubmissionAnswerRequest testSubmissionAnswerRequest)
        {
            if (testSubmissionAnswerRequest == null)
            {
                return new ApiResponse<TestSubmissionAnswerResponse>(1, "Dữ liệu đầu vào không hợp lệ.", null, null);
            }

            try
            {
                // Kiểm tra xem selected_answer_id có phải là đáp án đúng không
                bool isCorrect = _testAnswerRepo.IsCorrectAnswer(testSubmissionAnswerRequest.SelectedAnswerId);

                var testSubmissionAnswer = _mapper.Map<TestSubmissionsAnswer>(testSubmissionAnswerRequest);
                testSubmissionAnswer.IsCorrect = isCorrect; // Cập nhật giá trị is_correct

                var result = _repository.CreateTestSubmissionAnswer(testSubmissionAnswer);

                return result != null
                    ? ApiResponse<TestSubmissionAnswerResponse>.Success(_mapper.Map<TestSubmissionAnswerResponse>(result))
                    : ApiResponse<TestSubmissionAnswerResponse>.Conflict("Không thể tạo bản ghi.");
            }
            catch (Exception ex)
            {
                return new ApiResponse<TestSubmissionAnswerResponse>(1, "Lỗi hệ thống", null, new Dictionary<string, string[]>
                {
                    { "Exception", new[] { ex.Message } }
                });
            }
        }

        public ApiResponse<TestSubmissionAnswerResponse> UpdateTestSubmissionAnswer(int id, TestSubmissionAnswerRequest testSubmissionAnswerRequest)
        {
            if (testSubmissionAnswerRequest == null)
            {
                return new ApiResponse<TestSubmissionAnswerResponse>(1, "Dữ liệu đầu vào không hợp lệ.", null, null);
            }

            try
            {
                var existingAnswer = _repository.GetTestSubmissionAnswerById(id);
                if (existingAnswer == null)
                {
                    return ApiResponse<TestSubmissionAnswerResponse>.NotFound("Không tìm thấy câu trả lời.");
                }

                // Kiểm tra xem selected_answer_id có phải là đáp án đúng không
                bool isCorrect = _testAnswerRepo.IsCorrectAnswer(testSubmissionAnswerRequest.SelectedAnswerId);

                // Cập nhật dữ liệu
                _mapper.Map(testSubmissionAnswerRequest, existingAnswer);
                existingAnswer.IsCorrect = isCorrect; // Cập nhật giá trị is_correct

                var updatedResult = _repository.UpdateTestSubmissionAnswer(existingAnswer);

                return updatedResult != null
                    ? ApiResponse<TestSubmissionAnswerResponse>.Success(_mapper.Map<TestSubmissionAnswerResponse>(updatedResult))
                    : ApiResponse<TestSubmissionAnswerResponse>.Conflict("Không thể cập nhật bản ghi.");
            }
            catch (Exception ex)
            {
                return new ApiResponse<TestSubmissionAnswerResponse>(1, "Lỗi hệ thống", null, new Dictionary<string, string[]>
                {
                    { "Exception", new[] { ex.Message } }
                });
            }
        }

        public ApiResponse<TestSubmissionAnswerResponse> DeleteTestSubmissionAnswer(int id)
        {
            var result = _repository.DeleteTestSubmissionAnswer(id);
            return result
                ? ApiResponse<TestSubmissionAnswerResponse>.Success()
                : ApiResponse<TestSubmissionAnswerResponse>.NotFound("Không tìm thấy câu trả lời.");
        }
    }
}
