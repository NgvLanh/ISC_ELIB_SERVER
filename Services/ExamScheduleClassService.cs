using AutoMapper;
using ISC_ELIB_SERVER.DTOs.Requests;
using ISC_ELIB_SERVER.DTOs.Responses;
using ISC_ELIB_SERVER.Models;
using ISC_ELIB_SERVER.Repositories;

namespace ISC_ELIB_SERVER.Services
{
    public class ExamScheduleClassService
    {
        private readonly ExamScheduleClassRepo _repository;
        private readonly IMapper _mapper;

        public ExamScheduleClassService(ExamScheduleClassRepo repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public ApiResponse<ICollection<ExamScheduleClassResponse>> GetAll()
        {
            var entities = _repository.GetAll();
            var responses = _mapper.Map<ICollection<ExamScheduleClassResponse>>(entities);
            return ApiResponse<ICollection<ExamScheduleClassResponse>>.Success(responses);
        }

        public ApiResponse<ExamScheduleClassResponse> GetById(long id)
        {
            var entity = _repository.GetById(id);
            if (entity == null) return ApiResponse<ExamScheduleClassResponse>.NotFound("ExamScheduleClass không tồn tại");

            var response = _mapper.Map<ExamScheduleClassResponse>(entity);
            return ApiResponse<ExamScheduleClassResponse>.Success(response);
        }

        public ApiResponse<ExamScheduleClassResponse> Create(ExamScheduleClassRequest request)
        {
            var entity = _mapper.Map<ExamScheduleClass>(request);
            _repository.Create(entity);

            var response = _mapper.Map<ExamScheduleClassResponse>(entity);
            return ApiResponse<ExamScheduleClassResponse>.Success(response);
        }

        public ApiResponse<ExamScheduleClassResponse> Update(long id, ExamScheduleClassRequest request)
        {
            var entity = _repository.GetById(id);
            if (entity == null) return ApiResponse<ExamScheduleClassResponse>.NotFound("ExamScheduleClass không tồn tại");

            _mapper.Map(request, entity);
            _repository.Update(entity);

            var response = _mapper.Map<ExamScheduleClassResponse>(entity);
            return ApiResponse<ExamScheduleClassResponse>.Success(response);
        }

        public ApiResponse<object> Delete(long id)
        {
            var result = _repository.Delete(id);
            return result
                ? ApiResponse<object>.Success("Xóa thành công")
                : ApiResponse<object>.NotFound("ExamScheduleClass không tồn tại");
        }
    }
}
