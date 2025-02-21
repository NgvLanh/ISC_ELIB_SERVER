using ISC_ELIB_SERVER.Models;
using ISC_ELIB_SERVER.Repositories;
using ISC_ELIB_SERVER.DTOs.Responses;
using ISC_ELIB_SERVER.DTOs.Requests;
using AutoMapper;
using ISC_ELIB_SERVER.Services.Interfaces;

namespace ISC_ELIB_SERVER.Services
{



    public class TransferSchoolService : ITransferSchoolService
    {
        private readonly TransferSchoolRepo _repository;
        private readonly IMapper _mapper;

        public TransferSchoolService(TransferSchoolRepo repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public ApiResponse<ICollection<TransferSchoolResponse>> GetTransferSchools(int page, int pageSize, string search, string sortColumn, string sortOrder)
        {
            var query = _repository.GetTransferSchools().AsQueryable();

            query = sortColumn switch
            {
                "Id" => sortOrder.ToLower() == "desc" ? query.OrderByDescending(us => us.Id) : query.OrderBy(us => us.Id),
                _ => query.OrderBy(us => us.Id)
            };

            var result = query.Skip((page - 1) * pageSize).Take(pageSize).ToList();

            var response = _mapper.Map<ICollection<TransferSchoolResponse>>(result);

            return result.Any()
                ? ApiResponse<ICollection<TransferSchoolResponse>>.Success(response)
                : ApiResponse<ICollection<TransferSchoolResponse>>.NotFound("Không có dữ liệu");
        }

        public ApiResponse<ICollection<TransferSchoolResponse>> GetTransferSchoolsNormal()
        {
            var result = _repository.GetTransferSchools();

            var response = _mapper.Map<ICollection<TransferSchoolResponse>>(result);

            return result.Any()
                ? ApiResponse<ICollection<TransferSchoolResponse>>.Success(response)
                : ApiResponse<ICollection<TransferSchoolResponse>>.NotFound("Không có dữ liệu");
        }


        public ApiResponse<TransferSchoolResponse> GetTransferSchoolById(long id)
        {
            var TransferSchool = _repository.GetTransferSchoolById(id);
            return TransferSchool != null
                ? ApiResponse<TransferSchoolResponse>.Success(_mapper.Map<TransferSchoolResponse>(TransferSchool))
                : ApiResponse<TransferSchoolResponse>.NotFound($"Không tìm thấy thông tin tạm nghỉ của #{id}");
        }


        public ApiResponse<TransferSchoolResponse> CreateTransferSchool(TransferSchool_AddRequest request)
        {
            if (request.TransferSchoolDate.HasValue)
            {
                // Chuyển sang DateTime có Kind Unspecified
                request.TransferSchoolDate = DateTime.SpecifyKind(request.TransferSchoolDate.Value, DateTimeKind.Unspecified);
            }
            var TransferSchool = _mapper.Map<TransferSchool>(request);
            var created = _repository.CreateTransferSchool(TransferSchool);
            return ApiResponse<TransferSchoolResponse>.Success(_mapper.Map<TransferSchoolResponse>(created));
        }

        public ApiResponse<TransferSchool> UpdateTransferSchool(long id , TransferSchool_UpdateRequest request)
        {
            if (request.TransferSchoolDate.HasValue)
            {
                // Chuyển sang DateTime có Kind Unspecified
                request.TransferSchoolDate = DateTime.SpecifyKind(request.TransferSchoolDate.Value, DateTimeKind.Unspecified);
            }

            var TransferSchool = _mapper.Map<TransferSchool>(request);
            var updated = _repository.UpdateTransferSchool(id ,TransferSchool);
            return updated != null
                ? ApiResponse<TransferSchool>.Success(updated)
                : ApiResponse<TransferSchool>.NotFound("Không tìm thấy trạng thái người dùng để cập nhật");
        }

        public ApiResponse<TransferSchool> DeleteTransferSchool(long id)
        {
            var success = _repository.DeleteTransferSchool2(id);
            return success
                ? ApiResponse<TransferSchool>.Success()
                : ApiResponse<TransferSchool>.NotFound("Không tìm thấy trạng thái người dùng để xóa");
        }
    }

}
