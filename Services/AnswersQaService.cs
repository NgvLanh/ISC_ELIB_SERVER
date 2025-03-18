﻿using ISC_ELIB_SERVER.Models;
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

            var result = query.ToList().Select(a => new AnswersQaResponse
            {
                Id = a.Id,
                Content = a.Content ?? "",
                CreateAt = a.CreateAt ?? DateTime.Now,
                UserId = a.UserId ?? 0,
                QuestionId = a.QuestionId ?? 0,

                UserAvatar = a.User?.AvatarUrl ?? "https://via.placeholder.com/40",
                UserName = a.User?.FullName ?? "Unknown",
                UserRole = a.User?.Role?.Name ?? "Người dùng",

                // Lấy danh sách hình ảnh
                ImageUrls = a.AnswerImagesQas?.Select(img => img.ImageUrl).ToList() ?? new List<string>()
            }).ToList();

            return result.Any()
                ? ApiResponse<ICollection<AnswersQaResponse>>.Success(result)
                : ApiResponse<ICollection<AnswersQaResponse>>.NotFound("Không có dữ liệu câu trả lời.");
        }

        public ApiResponse<AnswersQaResponse> GetAnswerById(long id)
        {
            var answer = _repository.GetAnswerById(id);
            if (answer == null)
            {
                return ApiResponse<AnswersQaResponse>.NotFound($"Không tìm thấy câu trả lời #{id}");
            }

            var response = new AnswersQaResponse
            {
                Id = answer.Id,
                Content = answer.Content ?? "",
                CreateAt = answer.CreateAt ?? DateTime.Now,
                UserId = answer.UserId ?? 0,
                QuestionId = answer.QuestionId ?? 0,

                UserAvatar = answer.User?.AvatarUrl ?? "https://via.placeholder.com/40",
                UserName = answer.User?.FullName ?? "Unknown",
                UserRole = answer.User?.Role?.Name ?? "Người dùng",

                // Lấy danh sách hình ảnh
                ImageUrls = answer.AnswerImagesQas?.Select(img => img.ImageUrl).ToList() ?? new List<string>()
            };

            return ApiResponse<AnswersQaResponse>.Success(response);
        }

      public async Task<ApiResponse<AnswersQaResponse>> CreateAnswer(AnswersQaRequest answerRequest)
        {
            List<string> imageBase64List = new List<string>();

            // 🔥 Kiểm tra xem có ảnh không
            if (answerRequest.ImageBase64s != null && answerRequest.ImageBase64s.Count > 0)
            {
                foreach (var base64 in answerRequest.ImageBase64s)
                {
                    if (!string.IsNullOrEmpty(base64))
                    {
                        imageBase64List.Add(base64); //  Lưu trực tiếp Base64
                    }
                }
            }

            var newAnswer = new AnswersQa
            {
                Content = answerRequest.Content,
                UserId = answerRequest.UserId,
                QuestionId = answerRequest.QuestionId,
                CreateAt = DateTime.Now,
                Active = true
            };

            var createdAnswer = _repository.CreateAnswer(newAnswer, imageBase64List); // Lưu ảnh Base64

            var response = new AnswersQaResponse
            {
                Id = createdAnswer.Id,
                Content = createdAnswer.Content,
                CreateAt = createdAnswer.CreateAt ?? DateTime.Now,
                UserId = createdAnswer.UserId ?? 0,
                QuestionId = createdAnswer.QuestionId ?? 0,
                UserAvatar = createdAnswer.User?.AvatarUrl ?? "https://via.placeholder.com/40",
                UserName = createdAnswer.User?.FullName ?? "Unknown",
                UserRole = createdAnswer.User?.Role?.Name ?? "Người dùng",
                ImageUrls = imageBase64List //  Trả về danh sách Base64
            };

            return ApiResponse<AnswersQaResponse>.Success(response);
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
