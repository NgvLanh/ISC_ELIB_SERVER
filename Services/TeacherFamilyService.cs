using ISC_ELIB_SERVER.DTOs.Requests;
using ISC_ELIB_SERVER.DTOs.Responses;
using ISC_ELIB_SERVER.Mappers;
using ISC_ELIB_SERVER.Models;
using ISC_ELIB_SERVER.Repositories;

namespace ISC_ELIB_SERVER.Services
{
    public class TeacherFamilyService : ITeacherFamilyService
    {
        private readonly ITeacherFamilyRepository _repository;

        public TeacherFamilyService(ITeacherFamilyRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<TeacherFamilyResponse>> GetAllAsync()
        {
            var entities = await _repository.GetAllAsync();
            return entities.Select(e => e.ToResponse());
        }

        public async Task<TeacherFamilyResponse?> GetByIdAsync(long id)
        {
            var entity = await _repository.GetByIdAsync(id);
            return entity?.ToResponse();
        }

        public async Task AddAsync(TeacherFamilyRequest request)
        {
            var entity = new TeacherFamily
            {
                TeacherId = request.TeacherId,
                GuardianName = request.GuardianName,
                GuardianPhone = request.GuardianPhone,
                GuardianAddressDetail = request.GuardianAddressDetail,
                GuardianAddressFull = request.GuardianAddressFull,
                ProvinceCode = request.ProvinceCode,
                DistrictCode = request.DistrictCode,
                WardCode = request.WardCode
            };

            await _repository.AddAsync(entity);
        }

        public async Task UpdateAsync(long id, TeacherFamilyRequest request)
        {
            var entity = await _repository.GetByIdAsync(id);
            if (entity == null) throw new KeyNotFoundException("TeacherFamily not found");

            entity.TeacherId = request.TeacherId;
            entity.GuardianName = request.GuardianName;
            entity.GuardianPhone = request.GuardianPhone;
            entity.GuardianAddressDetail = request.GuardianAddressDetail;
            entity.GuardianAddressFull = request.GuardianAddressFull;
            entity.ProvinceCode = request.ProvinceCode;
            entity.DistrictCode = request.DistrictCode;
            entity.WardCode = request.WardCode;

            await _repository.UpdateAsync(entity);
        }

        public async Task DeleteAsync(long id)
        {
            await _repository.DeleteAsync(id);
        }
    }

}
