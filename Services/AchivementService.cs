using AutoMapper;
using ISC_ELIB_SERVER.DTOs.Requests;
using ISC_ELIB_SERVER.DTOs.Responses;
using ISC_ELIB_SERVER.Enums;
using ISC_ELIB_SERVER.Models;
using ISC_ELIB_SERVER.Repositories;
using ISC_ELIB_SERVER.Services.Interfaces;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace ISC_ELIB_SERVER.Services
{
    public class AchivementService : IAchivementService
    {
        private readonly AchivementRepo _repository;
        private readonly IMapper _mapper;

        public AchivementService(AchivementRepo repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public ApiResponse<ICollection<AchivementResponse>> GetAchivements(int page, int pageSize, string search, string sortColumn, string sortOrder)
        {
            var query = _repository.GetAchievements().AsQueryable();

            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(us => us.Content.ToLower().Contains(search.ToLower()));
            }

            query = sortColumn switch
            {
                "Content" => sortOrder.ToLower() == "desc" ? query.OrderByDescending(us => us.Content) : query.OrderBy(us => us.Content),
                "Id" => sortOrder.ToLower() == "desc" ? query.OrderByDescending(us => us.Id) : query.OrderBy(us => us.Id),
                _ => query.OrderBy(us => us.Id)
            };

            var result = query.Skip((page - 1) * pageSize).Take(pageSize).ToList();

            var response = result.Select(achievement => new AchivementResponse
            {
                Id = achievement.Id,
                Content = achievement.Content,
                DateAwarded = achievement.DateAwarded,
                UserId = achievement.UserId,
                TypeId = achievement.TypeId,
                File = achievement.File,
                TypeValue = achievement.TypeId.HasValue && Enum.IsDefined(typeof(EType), achievement.TypeId.Value)
                             ? ((EType)achievement.TypeId.Value).ToString()
                             : "Không xác định"
            }).ToList();

            return response.Any()
                ? ApiResponse<ICollection<AchivementResponse>>.Success(response)
                : ApiResponse<ICollection<AchivementResponse>>.NotFound("Không có dữ liệu");
        }

        public ApiResponse<AchivementResponse> GetAchivementById(int id)
        {
            var achievement = _repository.GetAchivementById(id);

            if (achievement == null)
            {
                return ApiResponse<AchivementResponse>.NotFound("Không tìm thấy");
            }

            var response = new AchivementResponse
            {
                Id = achievement.Id,
                Content = achievement.Content,
                DateAwarded = achievement.DateAwarded,
                UserId = achievement.UserId,
                TypeId = achievement.TypeId,
                File = achievement.File
            };

            if (response.TypeId.HasValue && Enum.IsDefined(typeof(EType), response.TypeId.Value))
            {
                response.TypeValue = ((EType)response.TypeId.Value).ToString();
            }
            else
            {
                response.TypeValue = "Không xác định";
            }

            return ApiResponse<AchivementResponse>.Success(response);
        }

        public ApiResponse<AchivementResponse> CreateAchivement(AchivementRequest achivementRequest)
        {

            try
            {

                if (achivementRequest is null)
                {
                    return ApiResponse<AchivementResponse>.BadRequest("Dữ liệu đầu vào không hợp lệ");
                }

                var achivement = new Achievement
                {
                    UserId = achivementRequest.UserId,
                    DateAwarded = DateTime.SpecifyKind(achivementRequest.DateAwarded, DateTimeKind.Unspecified),
                    Content = achivementRequest.Content,
                    TypeId = achivementRequest?.TypeId,
                    File = achivementRequest?.File,
                };

                var created = _repository.CreateAchivement(achivement);

                var response = new AchivementResponse
                {
                    Id = created.Id,
                    Content = created.Content,
                    DateAwarded = created.DateAwarded,
                    UserId = created.UserId,
                    TypeId = created.TypeId,
                    File = created.File
                };

                if (response.TypeId.HasValue && Enum.IsDefined(typeof(EType), response.TypeId.Value))
                {
                    response.TypeValue = ((EType)response.TypeId.Value).ToString();
                }
                else
                {
                    response.TypeValue = "Không xác định";
                }

                return ApiResponse<AchivementResponse>.Success(_mapper.Map<AchivementResponse>(response));
            }
            catch (Exception)
            {
                return ApiResponse<AchivementResponse>.BadRequest("Dữ liệu đầu vào không hợp lệ");
            }
        }

        public ApiResponse<AchivementResponse> UpdateAchivement(int id, AchivementRequest achivementRequest)
        {
            try
            {
                

                var achievement = _repository.GetAchivementById(id);
                if (achievement == null)
                {
                    return ApiResponse<AchivementResponse>.NotFound($"Không tìm thấy loại đầu vào #{id}");
                }

                achievement.UserId = achivementRequest.UserId;
                achievement.TypeId = achivementRequest.TypeId;
                achievement.Content = achivementRequest.Content;
                achievement.File = achivementRequest.File;
                achievement.DateAwarded = DateTime.SpecifyKind(achivementRequest.DateAwarded, DateTimeKind.Unspecified);

                

                var updated = _repository.UpdateAchivement(achievement);

                if (updated == null)
                {
                    return ApiResponse<AchivementResponse>.BadRequest("Cập nhật thất bại");
                }

                var response = new AchivementResponse
                {
                    Id = updated.Id,
                    Content = updated.Content,
                    DateAwarded = updated.DateAwarded,
                    UserId = updated.UserId,
                    TypeId = updated.TypeId,
                    File = updated.File
                };

                if (response.TypeId.HasValue && Enum.IsDefined(typeof(EType), response.TypeId.Value))
                {
                    response.TypeValue = ((EType)response.TypeId.Value).ToString();
                }
                else
                {
                    response.TypeValue = "Không xác định";
                }

                return updated != null
                    ? ApiResponse<AchivementResponse>.Success(response)
                    : ApiResponse<AchivementResponse>.NotFound("Không tìm thấy vai trò để sửa");
            }
            catch
            {
                return ApiResponse<AchivementResponse>.BadRequest("Cập nhật thất bại");
            }
        }

        public ApiResponse<bool> DeleteAchivement(int id)
        {
            var success = _repository.DeleteAchivement(id);
            return success
                ? ApiResponse<bool>.Success(true)
                : ApiResponse<bool>.NotFound("Không tìm thấy vai trò để xóa");
        }
    }
}
