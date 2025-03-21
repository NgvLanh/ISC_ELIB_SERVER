using ISC_ELIB_SERVER.DTOs.Requests;
using ISC_ELIB_SERVER.DTOs.Responses;
using ISC_ELIB_SERVER.Models;
using ISC_ELIB_SERVER.Repositories;
using System.Collections.Generic;
using System.Linq;

namespace ISC_ELIB_SERVER.Services
{
    public class TestAnswerService
    {
        private readonly TestAnswerRepo _repository;

        public TestAnswerService(TestAnswerRepo repository)
        {
            _repository = repository;
        }

        //  Lấy danh sách câu trả lời theo QuestionId
        public ApiResponse<List<TestAnswerResponse>> GetAnswersByQuestion(int questionId)
        {
            var answers = _repository.GetAnswersByQuestion(questionId);

            var response = answers.Select(a => new TestAnswerResponse
            {
                Id = a.Id,
                QuestionId = a.QuestionId ?? 0,
                AnswerText = a.AnswerText ?? "",
                IsCorrect = a.IsCorrect ?? false
             
            }).ToList();

            return response.Any()
                ? ApiResponse<List<TestAnswerResponse>>.Success(response)
                : ApiResponse<List<TestAnswerResponse>>.NotFound("Không có câu trả lời nào cho câu hỏi này.");
        }

        //  Tạo câu trả lời
      public ApiResponse<TestAnswer> CreateTestAnswer(TestAnswerRequest request)
        {
            var newAnswer = new TestAnswer
            {
                QuestionId = request.QuestionId,
                AnswerText = request.AnswerText,
                IsCorrect = request.IsCorrect
            };

            var createdAnswer = _repository.CreateTestAnswer(newAnswer);
            return ApiResponse<TestAnswer>.Success(createdAnswer);
        }

        //  Cập nhật câu trả lời
        public ApiResponse<TestAnswerResponse> UpdateTestAnswer(int id, TestAnswerRequest request)
        {
            var existingAnswer = _repository.GetAnswerById(id);
            if (existingAnswer == null)
                return ApiResponse<TestAnswerResponse>.NotFound("Không tìm thấy câu trả lời.");

            // Cập nhật thông tin câu trả lời
            existingAnswer.AnswerText = request.AnswerText;
            existingAnswer.IsCorrect = request.IsCorrect;

            var updatedAnswer = _repository.UpdateAnswer(existingAnswer);

            var response = new TestAnswerResponse
            {
                Id = updatedAnswer.Id,
                QuestionId = updatedAnswer.QuestionId ?? 0,
                AnswerText = updatedAnswer.AnswerText ?? "",
                IsCorrect = updatedAnswer.IsCorrect ?? false
            };

            return ApiResponse<TestAnswerResponse>.Success(response);
        }

        //  Xóa câu trả lời
        public ApiResponse<bool> DeleteTestAnswer(int id)
        {
            var existingAnswer = _repository.GetAnswerById(id);
            if (existingAnswer == null)
                return ApiResponse<bool>.NotFound("Không tìm thấy câu trả lời.");

            var deleted = _repository.DeleteAnswer(id);
            return deleted
                ? ApiResponse<bool>.Success(true)
                : ApiResponse<bool>.Error(new Dictionary<string, string[]> { { "message", new[] { "Xóa câu trả lời thất bại." } } });
        }
    }
}
