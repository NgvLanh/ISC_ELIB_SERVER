using ISC_ELIB_SERVER.Models;
using ISC_ELIB_SERVER.Repositories;
using ISC_ELIB_SERVER.DTOs.Requests;
using ISC_ELIB_SERVER.DTOs.Responses;
using AutoMapper;
using System.Collections.Generic;
using System.Linq;

namespace ISC_ELIB_SERVER.Services
{
    public interface ISystemSettingsService
    {
        ApiResponse<ICollection<SystemSettingResponse>> GetSystemSettings(int page, int pageSize);
        ApiResponse<SystemSettingResponse> GetSystemSettingByUserId(int userId);
        ApiResponse<SystemSettingResponse> CreateOrUpdateSystemSetting(SystemSettingRequest systemSettingRequest, int userId);
        ApiResponse<object> DeleteSystemSetting(int id);
    }

    public class SystemSettingsService : ISystemSettingsService
    {
        private readonly ISystemSettingsRepo _repository;
        private readonly IMapper _mapper;

        public SystemSettingsService(ISystemSettingsRepo repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public ApiResponse<ICollection<SystemSettingResponse>> GetSystemSettings(int page, int pageSize)
        {
            var query = _repository.GetAll().AsQueryable();
            var result = query.Skip((page - 1) * pageSize).Take(pageSize).ToList();
            var response = _mapper.Map<ICollection<SystemSettingResponse>>(result);

            return result.Any()
                ? ApiResponse<ICollection<SystemSettingResponse>>.Success(response)
                : ApiResponse<ICollection<SystemSettingResponse>>.NotFound("Không có dữ liệu");
        }

        public ApiResponse<SystemSettingResponse> GetSystemSettingByUserId(int userId)
        {
            var setting = _repository.GetAll().FirstOrDefault(ss => ss.UserId == userId);
            return setting != null
                ? ApiResponse<SystemSettingResponse>.Success(_mapper.Map<SystemSettingResponse>(setting))
                : ApiResponse<SystemSettingResponse>.NotFound("Không tìm thấy cài đặt hệ thống của người dùng");
        }

        public ApiResponse<SystemSettingResponse> CreateOrUpdateSystemSetting(SystemSettingRequest systemSettingRequest, int userId)
        {
            var existingSetting = _repository.GetAll().FirstOrDefault(ss => ss.UserId == userId);

            if (existingSetting != null)
            {
                existingSetting.ThemeId = systemSettingRequest.ThemeId;
                existingSetting.Captcha = systemSettingRequest.Captcha;
                var updated = _repository.Update(existingSetting);
                return ApiResponse<SystemSettingResponse>.Success(_mapper.Map<SystemSettingResponse>(updated));
            }
            else
            {
                var newSetting = new SystemSetting
                {
                    UserId = userId,
                    ThemeId = systemSettingRequest.ThemeId,
                    Captcha = systemSettingRequest.Captcha,
                    Active = true
                };

                var created = _repository.Create(newSetting);
                return ApiResponse<SystemSettingResponse>.Success(_mapper.Map<SystemSettingResponse>(created));
            }
        }

        public ApiResponse<object> DeleteSystemSetting(int id)
        {
            var success = _repository.Delete(id);
            return success ? ApiResponse<object>.Success() : ApiResponse<object>.NotFound("Không tìm thấy cài đặt hệ thống để xóa");
        }
    }
}
