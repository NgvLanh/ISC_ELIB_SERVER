using ISC_ELIB_SERVER.Models;
using ISC_ELIB_SERVER.Repositories;
using ISC_ELIB_SERVER.DTOs.Requests;
using ISC_ELIB_SERVER.DTOs.Responses;
using AutoMapper;

namespace ISC_ELIB_SERVER.Services
{
    public class AnswersQaService : IAnswersQaService
    {
        private readonly AnswersQaRepo _repository;
        private readonly IMapper _mapper;

        public AnswersQaService(AnswersQaRepo repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public ApiResponse<ICollection<AnswersQaResponse>> GetAnswers(long? questionId)
        {
            var query = _repository.GetAnswers().AsQueryable();

            if (questionId.HasValue)
            {
                query = query.Where(a => a.QuestionId == questionId.Value);
            }

            var result = query.ToList();
            var response = _mapper.Map<ICollection<AnswersQaResponse>>(result);

            return result.Any()
                ? ApiResponse<ICollection<AnswersQaResponse>>.Success(response)
                : ApiResponse<ICollection<AnswersQaResponse>>.NotFound("Không có dữ liệu câu trả lời.");
        }

        public ApiResponse<AnswersQaResponse> GetAnswerById(long id)
        {
            var answer = _repository.GetAnswerById(id);
            return answer != null
                ? ApiResponse<AnswersQaResponse>.Success(_mapper.Map<AnswersQaResponse>(answer))
                : ApiResponse<AnswersQaResponse>.NotFound($"Không tìm thấy câu trả lời #{id}");
        }

        public ApiResponse<AnswersQaResponse> CreateAnswer(AnswersQaRequest answerRequest)
        {
            var created = _repository.CreateAnswer(new AnswersQa()
            {
                Content = answerRequest.Content,
                UserId = answerRequest.UserId,
                QuestionId = answerRequest.QuestionId,
                CreateAt = DateTime.Now
            });

            return ApiResponse<AnswersQaResponse>.Success(_mapper.Map<AnswersQaResponse>(created));
        }

        public ApiResponse<AnswersQaResponse> UpdateAnswer(long id, AnswersQaRequest answerRequest)
        {
            var existing = _repository.GetAnswerById(id);
            if (existing == null)
            {
                return ApiResponse<AnswersQaResponse>.NotFound("Câu trả lời không tồn tại");
            }

            existing.Content = answerRequest.Content;
            var updated = _repository.UpdateAnswer(existing);

            return ApiResponse<AnswersQaResponse>.Success(_mapper.Map<AnswersQaResponse>(updated));
        }

        public ApiResponse<AnswersQaResponse> DeleteAnswer(long id)
        {
            var success = _repository.DeleteAnswer(id);
            return success
                ? ApiResponse<AnswersQaResponse>.Success()
                : ApiResponse<AnswersQaResponse>.NotFound("Không tìm thấy câu trả lời để xóa");
        }
    }
}
