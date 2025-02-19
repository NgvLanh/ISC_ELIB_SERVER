using ISC_ELIB_SERVER.DTOs.Requests;
using ISC_ELIB_SERVER.DTOs.Responses;
using ISC_ELIB_SERVER.Models;

namespace ISC_ELIB_SERVER.Mappers
{
    public static class TeacherFamilyMapper
    {
        public static TeacherFamilyResponse ToResponse(this TeacherFamily entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));

            return new TeacherFamilyResponse
            {
                Id = entity.Id,
                TeacherId = entity.TeacherId,
                GuardianName = entity.GuardianName ?? string.Empty,
                GuardianPhone = entity.GuardianPhone ?? string.Empty,
                GuardianAddressDetail = entity.GuardianAddressDetail ?? string.Empty,
                GuardianAddressFull = entity.GuardianAddressFull ?? string.Empty,
                ProvinceCode = entity.ProvinceCode,
                DistrictCode = entity.DistrictCode,
                WardCode = entity.WardCode
            };
        }
    }

}
