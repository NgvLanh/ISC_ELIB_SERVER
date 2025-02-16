using ISC_ELIB_SERVER.Models;
using ISC_ELIB_SERVER.Repositories;
using ISC_ELIB_SERVER.DTOs.Responses;
using ISC_ELIB_SERVER.DTOs.Requests;
using AutoMapper;

namespace ISC_ELIB_SERVER.Services
{
    public interface IQuestionQaService
    {
        ApiResponse<ICollection<QuestionQaResponse>> GetQuestions(int page, int pageSize, string search, string sortColumn, string sortOrder);
        ApiResponse<QuestionQaResponse> GetQuestionById(long id);
        ApiResponse<QuestionQaResponse> CreateQuestion(QuestionQaRequest questionRequest);
        ApiResponse<QuestionQaResponse> UpdateQuestion(long id, QuestionQaRequest QuestionQaRequest);
        ApiResponse<QuestionQaResponse> DeleteQuestion(long id);
    }

    public class QuestionQaService : IQuestionQaService
    {
        private readonly QuestionQaRepo _repository;
        private readonly IMapper _mapper;

        public QuestionQaService(QuestionQaRepo repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public ApiResponse<ICollection<QuestionQaResponse>> GetQuestions(int page, int pageSize, string search, string sortColumn, string sortOrder)
        {
            var query = _repository.GetQuestions().AsQueryable();

            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(q => q.Content.ToLower().Contains(search.ToLower()));
            }

            query = sortColumn switch
            {
                "Content" => sortOrder.ToLower() == "desc" ? query.OrderByDescending(q => q.Content) : query.OrderBy(q => q.Content),
                "Id" => sortOrder.ToLower() == "desc" ? query.OrderByDescending(q => q.Id) : query.OrderBy(q => q.Id),
                _ => query.OrderBy(q => q.Id)
            };

            var result = query.Skip((page - 1) * pageSize).Take(pageSize).ToList();

            var response = _mapper.Map<ICollection<QuestionQaResponse>>(result);

            return result.Any()
                ? ApiResponse<ICollection<QuestionQaResponse>>.Success(response)
                : ApiResponse<ICollection<QuestionQaResponse>>.NotFound("Không có dữ liệu");
        }

        public ApiResponse<QuestionQaResponse> GetQuestionById(long id)
        {
            var question = _repository.GetQuestionById(id);
            return question != null
                ? ApiResponse<QuestionQaResponse>.Success(_mapper.Map<QuestionQaResponse>(question))
                : ApiResponse<QuestionQaResponse>.NotFound($"Không tìm thấy câu hỏi #{id}");
        }

        public ApiResponse<QuestionQaResponse> CreateQuestion(QuestionQaRequest questionRequest)
        {
            var created = _repository.CreateQuestion(new QuestionQa()
            {
                Content = questionRequest.Content,
                UserId = questionRequest.UserId,
                SubjectId = questionRequest.SubjectId,
                // Dùng DateTime.Now để lấy giờ hệ thống
                CreateAt = DateTime.Now
            });

            return ApiResponse<QuestionQaResponse>.Success(_mapper.Map<QuestionQaResponse>(created));
        }


        public ApiResponse<QuestionQaResponse> UpdateQuestion(long id, QuestionQaRequest request)
        {
            var existing = _repository.GetQuestionById(id);
            if (existing == null)
            {
                return ApiResponse<QuestionQaResponse>.NotFound("Câu hỏi không tồn tại");
            }

            // Chỉ cần cập nhật các trường được yêu cầu từ request
            existing.Content = request.Content;
            existing.SubjectId = request.SubjectId; // Sử dụng SubjectId
            existing.UserId = request.UserId;

            // Nếu cần thay đổi thời gian cập nhật (giờ hệ thống)
            existing.CreateAt = DateTime.Now;  // Hoặc sử dụng DateTime.SpecifyKind nếu muốn giữ UTC

            // Cập nhật dữ liệu trong cơ sở dữ liệu
            var updated = _repository.UpdateQuestion(existing);

            // Trả về kết quả đã cập nhật
            return ApiResponse<QuestionQaResponse>.Success(_mapper.Map<QuestionQaResponse>(updated));
        }


        public ApiResponse<QuestionQaResponse> DeleteQuestion(long id)
        {
            var success = _repository.DeleteQuestion(id);
            return success
                ? ApiResponse<QuestionQaResponse>.Success()
                : ApiResponse<QuestionQaResponse>.NotFound("Không tìm thấy câu hỏi để xóa");
        }
    }
}
 