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
    public class ExamScheduleService : IExamScheduleService
    {
        private readonly ExamScheduleRepo _repository;
        private readonly IMapper _mapper;

        public ExamScheduleService(ExamScheduleRepo repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public ApiResponse<ICollection<ExamScheduleResponse>> GetExamSchedules(int page, int pageSize, string search, string sortColumn, string sortOrder)
        {
            var examSchedules = _repository.GetExamSchedules();

            if (!string.IsNullOrEmpty(search))
            {
                examSchedules = examSchedules
                    .Where(es => es.Name != null && es.Name.Contains(search))
                    .ToList();
            }

            examSchedules = sortColumn?.ToLower() switch
            {
                "id" => sortOrder.ToLower() == "desc"
                    ? examSchedules.OrderByDescending(es => es.Id).ToList()
                    : examSchedules.OrderBy(es => es.Id).ToList(),
                "name" => sortOrder.ToLower() == "desc"
                    ? examSchedules.OrderByDescending(es => es.Name).ToList()
                    : examSchedules.OrderBy(es => es.Name).ToList(),
                "examday" => sortOrder.ToLower() == "desc"
                    ? examSchedules.OrderByDescending(es => es.ExamDay).ToList()
                    : examSchedules.OrderBy(es => es.ExamDay).ToList(),
                _ => examSchedules.OrderBy(es => es.Id).ToList()
            };

            var paginatedResult = examSchedules
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            var mappedResult = _mapper.Map<ICollection<ExamScheduleResponse>>(paginatedResult);
            return ApiResponse<ICollection<ExamScheduleResponse>>.Success(mappedResult);
        }

        public ApiResponse<ExamScheduleResponse> GetExamScheduleById(int id)
        {
            var examSchedule = _repository.GetExamScheduleById(id);
            if (examSchedule == null)
            {
                return ApiResponse<ExamScheduleResponse>.NotFound();
            }
            return ApiResponse<ExamScheduleResponse>.Success(_mapper.Map<ExamScheduleResponse>(examSchedule));
        }

        public ApiResponse<ICollection<ExamScheduleResponse>> GetExamScheduleByName(string name)
        {
            var examSchedules = _repository.GetExamSchedulesByName(name);
            if (examSchedules == null || !examSchedules.Any())
            {
                return ApiResponse<ICollection<ExamScheduleResponse>>.NotFound();
            }
            return ApiResponse<ICollection<ExamScheduleResponse>>.Success(_mapper.Map<ICollection<ExamScheduleResponse>>(examSchedules));
        }

        public ApiResponse<ExamScheduleResponse> CreateExamSchedule(ExamScheduleRequest request)
        {
            var examSchedule = _mapper.Map<ExamSchedule>(request);
            // Nếu cần thiết, thiết lập ExamDay theo thời gian hiện tại
            examSchedule.ExamDay = System.DateTime.SpecifyKind(System.DateTime.UtcNow, System.DateTimeKind.Unspecified);
            var createdExamSchedule = _repository.CreateExamSchedule(examSchedule);
            if (createdExamSchedule != null)
            {
                var response = _mapper.Map<ExamScheduleResponse>(createdExamSchedule);
                return ApiResponse<ExamScheduleResponse>.Success(response);
            }
            return ApiResponse<ExamScheduleResponse>.Error(new Dictionary<string, string[]>
            {
                { "Error", new[] { "Failed to create exam schedule" } }
            });
        }

        public ApiResponse<ExamScheduleResponse> UpdateExamSchedule(int id, ExamScheduleRequest request)
        {
            var existingExamSchedule = _repository.GetExamScheduleById(id);
            if (existingExamSchedule == null)
            {
                return ApiResponse<ExamScheduleResponse>.Error(new Dictionary<string, string[]>
                {
                    { "Error", new[] { "Exam schedule not found" } }
                });
            }

            // Cập nhật các thuộc tính từ request
            _mapper.Map(request, existingExamSchedule);

            var updatedExamSchedule = _repository.UpdateExamSchedule(existingExamSchedule);
            if (updatedExamSchedule != null)
            {
                var response = _mapper.Map<ExamScheduleResponse>(updatedExamSchedule);
                return ApiResponse<ExamScheduleResponse>.Success(response);
            }
            return ApiResponse<ExamScheduleResponse>.Error(new Dictionary<string, string[]>
            {
                { "Error", new[] { "Failed to update exam schedule" } }
            });
        }

        public ApiResponse<ExamScheduleResponse> DeleteExamSchedule(int id)
        {
            var existingExamSchedule = _repository.GetExamScheduleById(id);
            if (existingExamSchedule == null)
            {
                return ApiResponse<ExamScheduleResponse>.NotFound("Exam schedule not found.");
            }

            try
            {
                var success = _repository.DeleteExamSchedule(id);
                if (success)
                {
                    return ApiResponse<ExamScheduleResponse>.Success();
                }
                return ApiResponse<ExamScheduleResponse>.Error(new Dictionary<string, string[]>
                {
                    { "Error", new[] { "No changes were made." } }
                });
            }
            catch (System.Exception ex)
            {
                System.Console.WriteLine($"[ERROR] Deleting exam schedule failed: {ex.Message}");
                return ApiResponse<ExamScheduleResponse>.Error(new Dictionary<string, string[]>
                {
                    { "Error", new[] { "An error occurred during deletion." } }
                });
            }
        }
    }
}
