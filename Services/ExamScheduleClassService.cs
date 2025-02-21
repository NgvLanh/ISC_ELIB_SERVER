using ISC_ELIB_SERVER.DTOs.Requests;
using ISC_ELIB_SERVER.DTOs.Responses;
using ISC_ELIB_SERVER.Models;
using ISC_ELIB_SERVER.Repositories;
using ISC_ELIB_SERVER.Services.Interfaces;
using AutoMapper;
using System.Collections.Generic;
using System.Linq;
using ISC_ELIB_SERVER.Requests;
using ISC_ELIB_SERVER.Responses;

namespace ISC_ELIB_SERVER.Services
{
    public class ExamScheduleClassService : IExamScheduleClassService
    {
        private readonly ExamScheduleClassRepo _repository;
        private readonly IMapper _mapper;

        public ExamScheduleClassService(ExamScheduleClassRepo repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public ApiResponse<ICollection<ExamScheduleClassResponse>> GetExamScheduleClasses(
            int page,
            int pageSize,
            int? classId,
            int? exampleSchedule,
            int? supervisoryTeacherId,
            string sortColumn,
            string sortOrder)
        {
            // Lấy toàn bộ dữ liệu
            var examScheduleClasses = _repository.GetExamScheduleClasses();

            // Áp dụng lọc nếu có điều kiện
            if (classId.HasValue || exampleSchedule.HasValue || supervisoryTeacherId.HasValue)
            {
                examScheduleClasses = _repository.GetExamScheduleClassesByFilter(classId, exampleSchedule, supervisoryTeacherId);
            }

            // Sắp xếp
            examScheduleClasses = sortColumn?.ToLower() switch
            {
                "id" => sortOrder.ToLower() == "desc"
                            ? examScheduleClasses.OrderByDescending(esc => esc.Id).ToList()
                            : examScheduleClasses.OrderBy(esc => esc.Id).ToList(),
                "classid" => sortOrder.ToLower() == "desc"
                            ? examScheduleClasses.OrderByDescending(esc => esc.ClassId).ToList()
                            : examScheduleClasses.OrderBy(esc => esc.ClassId).ToList(),
                "exampleschedule" => sortOrder.ToLower() == "desc"
                            ? examScheduleClasses.OrderByDescending(esc => esc.ExampleSchedule).ToList()
                            : examScheduleClasses.OrderBy(esc => esc.ExampleSchedule).ToList(),
                "supervisoryteacherid" => sortOrder.ToLower() == "desc"
                            ? examScheduleClasses.OrderByDescending(esc => esc.SupervisoryTeacherId).ToList()
                            : examScheduleClasses.OrderBy(esc => esc.SupervisoryTeacherId).ToList(),
                _ => examScheduleClasses.OrderBy(esc => esc.Id).ToList()
            };

            // Phân trang
            var paginatedResult = examScheduleClasses
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            var mappedResult = _mapper.Map<ICollection<ExamScheduleClassResponse>>(paginatedResult);
            return ApiResponse<ICollection<ExamScheduleClassResponse>>.Success(mappedResult);
        }

        public ApiResponse<ExamScheduleClassResponse> GetExamScheduleClassById(int id)
        {
            var examScheduleClass = _repository.GetExamScheduleClassById(id);
            if (examScheduleClass == null)
                return ApiResponse<ExamScheduleClassResponse>.NotFound();
            var response = _mapper.Map<ExamScheduleClassResponse>(examScheduleClass);
            return ApiResponse<ExamScheduleClassResponse>.Success(response);
        }

        public ApiResponse<ICollection<ExamScheduleClassResponse>> GetExamScheduleClassByFilter(
            int? classId,
            int? exampleSchedule,
            int? supervisoryTeacherId)
        {
            var examScheduleClasses = _repository.GetExamScheduleClassesByFilter(classId, exampleSchedule, supervisoryTeacherId);
            if (examScheduleClasses == null || !examScheduleClasses.Any())
                return ApiResponse<ICollection<ExamScheduleClassResponse>>.NotFound();
            var response = _mapper.Map<ICollection<ExamScheduleClassResponse>>(examScheduleClasses);
            return ApiResponse<ICollection<ExamScheduleClassResponse>>.Success(response);
        }

        public ApiResponse<ExamScheduleClassResponse> CreateExamScheduleClass(ExamScheduleClassRequest request)
        {
            var examScheduleClass = _mapper.Map<ExamScheduleClass>(request);
            var created = _repository.CreateExamScheduleClass(examScheduleClass);
            if (created != null)
            {
                var response = _mapper.Map<ExamScheduleClassResponse>(created);
                return ApiResponse<ExamScheduleClassResponse>.Success(response);
            }
            return ApiResponse<ExamScheduleClassResponse>.Error(new Dictionary<string, string[]>
            {
                { "Error", new [] { "Failed to create exam schedule class" } }
            });
        }

        public ApiResponse<ExamScheduleClassResponse> UpdateExamScheduleClass(int id, ExamScheduleClassRequest request)
        {
            var existing = _repository.GetExamScheduleClassById(id);
            if (existing == null)
            {
                return ApiResponse<ExamScheduleClassResponse>.Error(new Dictionary<string, string[]>
                {
                    { "Error", new [] { "Exam schedule class not found" } }
                });
            }

            _mapper.Map(request, existing);
            var updated = _repository.UpdateExamScheduleClass(existing);
            if (updated != null)
            {
                var response = _mapper.Map<ExamScheduleClassResponse>(updated);
                return ApiResponse<ExamScheduleClassResponse>.Success(response);
            }
            return ApiResponse<ExamScheduleClassResponse>.Error(new Dictionary<string, string[]>
            {
                { "Error", new [] { "Failed to update exam schedule class" } }
            });
        }

        public ApiResponse<ExamScheduleClassResponse> DeleteExamScheduleClass(int id)
        {
            var existing = _repository.GetExamScheduleClassById(id);
            if (existing == null)
            {
                return ApiResponse<ExamScheduleClassResponse>.NotFound("Exam schedule class not found.");
            }

            var success = _repository.DeleteExamScheduleClass(id);
            if (success)
            {
                return ApiResponse<ExamScheduleClassResponse>.Success();
            }
            return ApiResponse<ExamScheduleClassResponse>.Error(new Dictionary<string, string[]>
            {
                { "Error", new [] { "No changes were made." } }
            });
        }
    }
}
