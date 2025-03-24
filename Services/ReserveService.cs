using ISC_ELIB_SERVER.Models;
using ISC_ELIB_SERVER.Repositories;
using ISC_ELIB_SERVER.DTOs.Responses;
using ISC_ELIB_SERVER.DTOs.Requests;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ISC_ELIB_SERVER.Services
{
    public interface IReserveService
    {
        ApiResponse<ICollection<ReserveResponse>> GetReserves(int page, int pageSize, string search, string sortColumn, string sortOrder);
        ApiResponse<ReserveResponse> GetReserveById(long id);
        ApiResponse<ReserveResponse> CreateReserve(ReserveRequest reserveRequest);
        ApiResponse<Reserve> UpdateReserve(Reserve reserve);
        ApiResponse<Reserve> DeleteReserve(long id);
        ApiResponse<ICollection<ReserveListResponse>> GetActiveReserves(int page, int pageSize, string search, string sortColumn, string sortOrder);
    }

    public class ReserveService : IReserveService
    {
        private readonly ReserveRepo _repository;
        private readonly IMapper _mapper;

        public ReserveService(ReserveRepo repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public ApiResponse<ICollection<ReserveResponse>> GetReserves(int page, int pageSize, string search, string sortColumn, string sortOrder)
        {
            var query = _repository.GetReserves().AsQueryable();
            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(r => r.ReserveDate.HasValue
                    && r.ReserveDate.Value.ToString("yyyy-MM-dd").Contains(search));
            }

            query = sortColumn switch
            {
                "ReserveDate" => sortOrder.Equals("desc", StringComparison.OrdinalIgnoreCase)
                                  ? query.OrderByDescending(r => r.ReserveDate)
                                  : query.OrderBy(r => r.ReserveDate),
                "Id" => sortOrder.Equals("desc", StringComparison.OrdinalIgnoreCase)
                        ? query.OrderByDescending(r => r.Id)
                        : query.OrderBy(r => r.Id),
                _ => query.OrderBy(r => r.Id)
            };

            var result = query.Skip((page - 1) * pageSize).Take(pageSize).ToList();
            var response = _mapper.Map<ICollection<ReserveResponse>>(result);
            return result.Any() ?
                ApiResponse<ICollection<ReserveResponse>>.Success(response) :
                ApiResponse<ICollection<ReserveResponse>>.NotFound("Không có dữ liệu");
        }

        public ApiResponse<ReserveResponse> GetReserveById(long id)
        {
            var reserve = _repository.GetReserveById(id);
            return reserve != null ?
                ApiResponse<ReserveResponse>.Success(_mapper.Map<ReserveResponse>(reserve)) :
                ApiResponse<ReserveResponse>.NotFound($"Không tìm thấy đặt chỗ #{id}");
        }

        public ApiResponse<ReserveResponse> CreateReserve(ReserveRequest reserveRequest)
        {
            var reserve = _mapper.Map<Reserve>(reserveRequest);
            var created = _repository.CreateReserve(reserve); // Sử dụng reserve đã map, không tạo mới empty Reserve()
            return ApiResponse<ReserveResponse>.Success(_mapper.Map<ReserveResponse>(created));
        }

        public ApiResponse<Reserve> UpdateReserve(Reserve reserve)
        {
            var updated = _repository.UpdateReserve(reserve);
            return updated != null ?
                ApiResponse<Reserve>.Success(updated) :
                ApiResponse<Reserve>.NotFound("Không tìm thấy đặt chỗ để cập nhật");
        }

        public ApiResponse<Reserve> DeleteReserve(long id)
        {
            var success = _repository.DeleteReserve(id);
            return success ?
                ApiResponse<Reserve>.Success() :
                ApiResponse<Reserve>.NotFound("Không tìm thấy đặt chỗ để xóa");
        }

        public ApiResponse<ICollection<ReserveListResponse>> GetActiveReserves(int page, int pageSize, string search, string sortColumn, string sortOrder)
        {
            var reserves = _repository.GetActiveReserves();
            var response = _mapper.Map<ICollection<ReserveListResponse>>(reserves);

            return reserves.Any()
                ? ApiResponse<ICollection<ReserveListResponse>>.Success(response)
                : ApiResponse<ICollection<ReserveListResponse>>.NotFound("Không có dữ liệu bảo lưu.");
        }
    }
}
