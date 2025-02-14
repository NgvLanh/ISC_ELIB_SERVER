using AutoMapper;
using ISC_ELIB_SERVER.DTOs.Requests;
using ISC_ELIB_SERVER.DTOs.Responses;
using ISC_ELIB_SERVER.Models;
using ISC_ELIB_SERVER.Repositories;

namespace ISC_ELIB_SERVER.Services
{
    public class ExamGraderService
    {
        private readonly ExamGraderRepo _repository;
        private readonly IMapper _mapper;

        public ExamGraderService(ExamGraderRepo repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public ApiResponse<ICollection<ExamGraderResponse>> GetAll()
        {
            var entities = _repository.GetAll();
            var responses = _mapper.Map<ICollection<ExamGraderResponse>>(entities);
            return ApiResponse<ICollection<ExamGraderResponse>>.Success(responses);
        }

        public ApiResponse<ExamGraderResponse> GetById(long id)
        {
            var entity = _repository.GetById(id);
            if (entity == null) return ApiResponse<ExamGraderResponse>.NotFound("ExamGrader không tồn tại");

            var response = _mapper.Map<ExamGraderResponse>(entity);
            return ApiResponse<ExamGraderResponse>.Success(response);
        }

        public ApiResponse<ExamGraderResponse> Create(ExamGraderRequest request)
        {
            var entity = _mapper.Map<ExamGrader>(request);
            _repository.Create(entity);

            var response = _mapper.Map<ExamGraderResponse>(entity);
            return ApiResponse<ExamGraderResponse>.Success(response);
        }

        public ApiResponse<ExamGraderResponse> Update(long id, ExamGraderRequest request)
        {
            var entity = _repository.GetById(id);
            if (entity == null) return ApiResponse<ExamGraderResponse>.NotFound("ExamGrader không tồn tại");

            _mapper.Map(request, entity);
            _repository.Update(entity);

            var response = _mapper.Map<ExamGraderResponse>(entity);
            return ApiResponse<ExamGraderResponse>.Success(response);
        }

        public ApiResponse<object> Delete(long id)
        {
            var result = _repository.Delete(id);
            return result
                ? ApiResponse<object>.Success()
                : ApiResponse<object>.NotFound("ExamGrader không tồn tại");
        }
    }
}
