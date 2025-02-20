using ISC_ELIB_SERVER.Dto.Request;
using ISC_ELIB_SERVER.Dto.Response;
using ISC_ELIB_SERVER.Mapper;
using ISC_ELIB_SERVER.Mappers;
using ISC_ELIB_SERVER.Models;
using ISC_ELIB_SERVER.Repository;
using ISC_ELIB_SERVER.Requests;
using ISC_ELIB_SERVER.Responses;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ISC_ELIB_SERVER.Service
{
    public class ExamScheduleClassService : IExamScheduleClassService
    {
        private readonly ExamScheduleClassRepository _repository;

        public ExamScheduleClassService(ExamScheduleClassRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<ExamScheduleClassResponse>> GetAllAsync()
        {
            var entities = await _repository.GetAllAsync();
            return entities.Select(ExamScheduleClassMapper.ToResponse);
        }

        public async Task<ExamScheduleClassResponse?> GetByIdAsync(int id)
        {
            var entity = await _repository.GetByIdAsync(id);
            return entity == null ? null : ExamScheduleClassMapper.ToResponse(entity);
        }

        public async Task<ExamScheduleClassResponse> AddAsync(ExamScheduleClassRequest request)
        {
            var entity = ExamScheduleClassMapper.ToEntity(request);
            var result = await _repository.AddAsync(entity);
            return ExamScheduleClassMapper.ToResponse(result);
        }

        public async Task<ExamScheduleClassResponse?> UpdateAsync(int id, ExamScheduleClassRequest request)
        {
            var entity = await _repository.GetByIdAsync(id);
            if (entity == null) return null;

            entity.ClassId = request.ClassId;
            entity.ExamScheduleId = request.ExamScheduleId;
            entity.SupervisoryTeacherId = request.SupervisoryTeacherId;
            entity.Active = request.Active;

            var result = await _repository.UpdateAsync(entity);
            return ExamScheduleClassMapper.ToResponse(result);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            return await _repository.DeleteAsync(id);
        }
    }
}
