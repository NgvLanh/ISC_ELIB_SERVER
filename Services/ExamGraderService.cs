using ISC_ELIB_SERVER.DTOs.Requests;
using ISC_ELIB_SERVER.DTOs.Responses;
using ISC_ELIB_SERVER.Models;
using ISC_ELIB_SERVER.Repositories;
using AutoMapper;
using ISC_ELIB_SERVER.Services.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace ISC_ELIB_SERVER.Services
{
    public class ExamGraderService : IExamGraderService
    {
        private readonly ExamGraderRepo _repository;
        private readonly IMapper _mapper;

        public ExamGraderService(ExamGraderRepo repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public ApiResponse<ICollection<ExamGraderResponse>> GetExamGraders(int page, int pageSize, string search, string sortColumn, string sortOrder)
        {
            var examGraders = _repository.GetExamGraders();

            // Tìm kiếm dựa trên thuộc tính ClassIds (có thể mở rộng nếu cần)
            if (!string.IsNullOrEmpty(search))
            {
                examGraders = examGraders.Where(eg => eg.ClassIds != null && eg.ClassIds.Contains(search)).ToList();
            }

            // Sắp xếp
            examGraders = sortColumn?.ToLower() switch
            {
                "id" => sortOrder.ToLower() == "desc" ? examGraders.OrderByDescending(eg => eg.Id).ToList() : examGraders.OrderBy(eg => eg.Id).ToList(),
                "examid" => sortOrder.ToLower() == "desc" ? examGraders.OrderByDescending(eg => eg.ExamId).ToList() : examGraders.OrderBy(eg => eg.ExamId).ToList(),
                "userid" => sortOrder.ToLower() == "desc" ? examGraders.OrderByDescending(eg => eg.UserId).ToList() : examGraders.OrderBy(eg => eg.UserId).ToList(),
                _ => examGraders.OrderBy(eg => eg.Id).ToList()
            };

            // Phân trang
            var paginatedResult = examGraders
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            var mappedResult = _mapper.Map<ICollection<ExamGraderResponse>>(paginatedResult);
            return ApiResponse<ICollection<ExamGraderResponse>>.Success(mappedResult);
        }

        public ApiResponse<ExamGraderResponse> GetExamGraderById(int id)
        {
            var examGrader = _repository.GetExamGraderById(id);
            if (examGrader == null)
            {
                return ApiResponse<ExamGraderResponse>.NotFound();
            }
            return ApiResponse<ExamGraderResponse>.Success(_mapper.Map<ExamGraderResponse>(examGrader));
        }

        public ApiResponse<ICollection<ExamGraderResponse>> GetExamGraderByExamId(int examId)
        {
            var examGraders = _repository.GetExamGradersByExamId(examId);
            if (examGraders == null || !examGraders.Any())
            {
                return ApiResponse<ICollection<ExamGraderResponse>>.NotFound();
            }
            return ApiResponse<ICollection<ExamGraderResponse>>.Success(_mapper.Map<ICollection<ExamGraderResponse>>(examGraders));
        }

        public ApiResponse<ExamGraderResponse> CreateExamGrader(ExamGraderRequest request)
        {
            var examGrader = _mapper.Map<ExamGrader>(request);
            var created = _repository.CreateExamGrader(examGrader);
            if (created != null)
            {
                var response = _mapper.Map<ExamGraderResponse>(created);
                return ApiResponse<ExamGraderResponse>.Success(response);
            }
            return ApiResponse<ExamGraderResponse>.Error(new Dictionary<string, string[]>
            {
                { "Error", new[] { "Failed to create exam grader" } }
            });
        }

        public ApiResponse<ExamGraderResponse> UpdateExamGrader(int id, ExamGraderRequest request)
        {
            var existing = _repository.GetExamGraderById(id);
            if (existing == null)
            {
                return ApiResponse<ExamGraderResponse>.Error(new Dictionary<string, string[]>
                {
                    { "Error", new[] { "Exam grader not found" } }
                });
            }

            _mapper.Map(request, existing);
            var updated = _repository.UpdateExamGrader(existing);
            if (updated != null)
            {
                var response = _mapper.Map<ExamGraderResponse>(updated);
                return ApiResponse<ExamGraderResponse>.Success(response);
            }
            return ApiResponse<ExamGraderResponse>.Error(new Dictionary<string, string[]>
            {
                { "Error", new[] { "Failed to update exam grader" } }
            });
        }

        public ApiResponse<ExamGraderResponse> DeleteExamGrader(int id)
        {
            var existing = _repository.GetExamGraderById(id);
            if (existing == null)
            {
                return ApiResponse<ExamGraderResponse>.NotFound("Exam grader not found.");
            }
            var success = _repository.DeleteExamGrader(id);
            if (success)
            {
                return ApiResponse<ExamGraderResponse>.Success();
            }
            return ApiResponse<ExamGraderResponse>.Error(new Dictionary<string, string[]>
            {
                { "Error", new[] { "No changes were made." } }
            });
        }
    }
}
