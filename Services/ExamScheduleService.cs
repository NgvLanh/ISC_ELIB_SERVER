using AutoMapper;
using ISC_ELIB_SERVER.DTOs.Responses;
using ISC_ELIB_SERVER.DTOs.Requests;
using ISC_ELIB_SERVER.Models;
using ISC_ELIB_SERVER.Repositories;
using System.Collections.Generic;

namespace ISC_ELIB_SERVER.Services
{

    public interface IExamScheduleService
    {
        ApiResponse<PagedResult<ExamScheduleResponse>> GetAll(
        int page,
        int pageSize,
        string? search,
        string? sortBy,
        bool isDescending,
        int? academicYearId,
        int? semesterId,
            int? gradeLevelsId,  
        int? classId
    );

        ApiResponse<ExamScheduleResponse> GetById(long id);
        ApiResponse<ExamScheduleResponse> Create(ExamScheduleRequest request);
        ApiResponse<ExamScheduleResponse> Update(long id, ExamScheduleRequest request);
        ApiResponse<object> Delete(long id);

    }
    public class ExamScheduleService: IExamScheduleService
    {
        private readonly ExamScheduleRepo _repository;
        private readonly IMapper _mapper;

        public ExamScheduleService(ExamScheduleRepo repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public ApiResponse<PagedResult<ExamScheduleResponse>> GetAll(int page, int pageSize, string? search, string? sortBy, bool isDescending, int? academicYearId, int? semesterId, int? gradeLevelsId,  
    int? classId)
        {
            var entities = _repository.GetAll(page, pageSize, search, sortBy, isDescending, academicYearId, semesterId,   gradeLevelsId, 
    classId );
            var responses = _mapper.Map<ICollection<ExamScheduleResponse>>(entities.Items);

            var result = new PagedResult<ExamScheduleResponse>(responses, entities.TotalItems, page, pageSize);
            return ApiResponse<PagedResult<ExamScheduleResponse>>.Success(result);
        }

        public ApiResponse<ExamScheduleResponse> GetById(long id)
        {
            var entity = _repository.GetById(id);
            if (entity == null) return ApiResponse<ExamScheduleResponse>.NotFound("ExamSchedule không tồn tại");

            var response = _mapper.Map<ExamScheduleResponse>(entity);
            return ApiResponse<ExamScheduleResponse>.Success(response);
        }

        public ApiResponse<ExamScheduleResponse> Create(ExamScheduleRequest request)
        {
            try
            {
                var entity = _mapper.Map<ExamSchedule>(request);
                _repository.Create(entity);
                var response = _mapper.Map<ExamScheduleResponse>(entity);
                return ApiResponse<ExamScheduleResponse>.Success(response);
            }
            catch (Exception ex)
            {
                return ApiResponse<ExamScheduleResponse>.Error(new Dictionary<string, string[]>
                {
                    { "Exception", new[] { ex.Message } }
                });
            }
        }

        public ApiResponse<ExamScheduleResponse> Update(long id, ExamScheduleRequest request)
        {
            var entity = _repository.GetById(id);
            if (entity == null) return ApiResponse<ExamScheduleResponse>.NotFound("ExamSchedule không tồn tại");

            try
            {
                _mapper.Map(request, entity);
                _repository.Update(entity);

                var response = _mapper.Map<ExamScheduleResponse>(entity);
                return ApiResponse<ExamScheduleResponse>.Success(response);
            }
            catch (Exception ex)
            {
                return ApiResponse<ExamScheduleResponse>.Error(new Dictionary<string, string[]>
                {
                    { "Exception", new[] { ex.Message } }
                });
            }
        }

        public ApiResponse<object> Delete(long id)
        {
            var entity = _repository.GetById((int)id);
            if (entity == null)
            {
                return ApiResponse<object>.NotFound("ExamSchedule không tồn tại");
            }

            try
            {
                var result = _repository.Delete((int)id);
                return result
                    ? ApiResponse<object>.Success()
                    : ApiResponse<object>.Conflict("Không thể xóa ExamSchedule");
            }
            catch (Exception ex)
            {
                return ApiResponse<object>.Error(new Dictionary<string, string[]>
        {
            { "Exception", new[] { ex.Message } }
        });
            }
        }


    }

}

