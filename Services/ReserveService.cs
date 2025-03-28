using ISC_ELIB_SERVER.Models;
using ISC_ELIB_SERVER.Repositories;
using ISC_ELIB_SERVER.DTOs.Responses;
using ISC_ELIB_SERVER.DTOs.Requests;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace ISC_ELIB_SERVER.Services
{
    public interface IReserveService
    {
        ApiResponse<ReserveListResponse> GetReserveById(long id);
        ApiResponse<ReserveListResponse> GetReserveByStudentId(int studentId);
        ApiResponse<ReserveResponse> CreateReserve(ReserveRequest reserveRequest);
        ApiResponse<ReserveResponse> UpdateReserve(long id,ReserveRequest reserveRequest);
        ApiResponse<ReserveResponse> DeleteReserve(long id);
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

        // Lấy danh sách bảo lưu đang hoạt động
        public ApiResponse<ICollection<ReserveListResponse>> GetActiveReserves(int page, int pageSize, string search, string sortColumn, string sortOrder)
        {
            var reserves = _repository.GetActiveReserves(page, pageSize, search, sortColumn, sortOrder);
            var response = _mapper.Map<ICollection<ReserveListResponse>>(reserves);

            return reserves.Any()
                ? ApiResponse<ICollection<ReserveListResponse>>.Success(response)
                : ApiResponse<ICollection<ReserveListResponse>>.NotFound("Không có dữ liệu bảo lưu.");
        }

        // Lấy thông tin bảo lưu theo StudentId
        public ApiResponse<ReserveListResponse> GetReserveByStudentId(int studentId)
        {
            var reserve = _repository.GetReserveByStudentId(studentId);
            if (reserve == null)
            {
                return ApiResponse<ReserveListResponse>.NotFound("Không tìm thấy thông tin bảo lưu.");
            }

            var response = _mapper.Map<ReserveListResponse>(reserve);
            return ApiResponse<ReserveListResponse>.Success(response);
        }

        // Lấy thông tin bảo lưu theo Id Reserve
        public ApiResponse<ReserveListResponse> GetReserveById(long id)
        {
            var reserve = _repository.GetReserveById(id);
            return reserve != null ?
                ApiResponse<ReserveListResponse>.Success(_mapper.Map<ReserveListResponse>(reserve)) :
                ApiResponse<ReserveListResponse>.NotFound($"Không tìm thấy đặt chỗ #{id}");
        }

        public ApiResponse<ReserveResponse> CreateReserve(ReserveRequest reserveRequest)
        {
            var reserve = _mapper.Map<Reserve>(reserveRequest);

            var created = _repository.CreateReserve(reserve);
            return ApiResponse<ReserveResponse>.Success(_mapper.Map<ReserveResponse>(created));
        }


        public ApiResponse<ReserveResponse> UpdateReserve(long id, ReserveRequest reserveRequest)
        {
            var existingReserve = _repository.GetReserveById(id);
            if (existingReserve == null)
            {
                return ApiResponse<ReserveResponse>.NotFound($"Không tìm thấy đặt chỗ có ID {id}");
            }

            _mapper.Map(reserveRequest, existingReserve);
            _repository.UpdateReserve(existingReserve);

            var response = _mapper.Map<ReserveResponse>(existingReserve);
            return ApiResponse<ReserveResponse>.Success(response);
        }

        public ApiResponse<ReserveResponse> DeleteReserve(long id)
        {
            var success = _repository.DeleteReserve(id);
            return success ?
                ApiResponse<ReserveResponse>.Success() :
                ApiResponse<ReserveResponse>.NotFound("Không tìm thấy đặt chỗ để xóa");
        }       
    }
}
