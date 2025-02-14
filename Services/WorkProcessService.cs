﻿using AutoMapper;
using ISC_ELIB_SERVER.DTOs.Requests;
using ISC_ELIB_SERVER.DTOs.Responses;
using ISC_ELIB_SERVER.Models;
using ISC_ELIB_SERVER.Repositories;

namespace ISC_ELIB_SERVER.Services
{
    public interface IWorkProcessService
    {
        ApiResponse<ICollection<WorkProcessResponse>> GetWorkProcess(int page, int pageSize, string search, string sortColumn, string sortOrder);

        ApiResponse<ICollection<WorkProcessResponse>> GetWorkProcessNoPaging();
        ApiResponse<WorkProcessResponse> GetWorkProcessById(long id);
        ApiResponse<WorkProcessResponse> CreateWorkProcess(WorkProcessRequest workProcess_AddRequest);
        ApiResponse<WorkProcess> UpdateWorkProcess(long id, WorkProcessRequest workProcess_UpdateRequest);
        ApiResponse<WorkProcess> DeleteWorkProcess(long id);
    }

    public class WorkProcessService : IWorkProcessService
    {
        private readonly WorkProcessRepo _repository;
        private readonly IMapper _mapper;
        private readonly isc_elibContext _context;

        public WorkProcessService(WorkProcessRepo repository, IMapper mapper, isc_elibContext context)
        {
            _repository = repository;
            _mapper = mapper;
            _context = context;
        }

        public ApiResponse<WorkProcessResponse> CreateWorkProcess(WorkProcessRequest workProcess_AddRequest)
        {

            var workProcess = _mapper.Map<WorkProcess>(workProcess_AddRequest);
            var created = _repository.CreateWorkProcess(workProcess);
            return ApiResponse<WorkProcessResponse>.Success(_mapper.Map<WorkProcessResponse>(created));
        }

        public ApiResponse<WorkProcess> DeleteWorkProcess(long id)
        {
            var success = _repository.DeleteWorkProcess(id);
            return success
                ? ApiResponse<WorkProcess>.Success()
                : ApiResponse<WorkProcess>.NotFound("Không tìm thấy trạng thái người dùng để xóa");
        }

        public ApiResponse<ICollection<WorkProcessResponse>> GetWorkProcess(int page, int pageSize, string search, string sortColumn, string sortOrder)
        {
            var query = _repository.GetWorkProcess().AsQueryable();


            query = sortColumn switch
            {
                "Id" => sortOrder.ToLower() == "desc" ? query.OrderByDescending(us => us.Id) : query.OrderBy(us => us.Id),
                _ => query.OrderBy(us => us.Id)
            };

            var result = query.Skip((page - 1) * pageSize).Take(pageSize).ToList();

            var response = _mapper.Map<ICollection<WorkProcessResponse>>(result);

            return result.Any()
                ? ApiResponse<ICollection<WorkProcessResponse>>.Success(response)
                : ApiResponse<ICollection<WorkProcessResponse>>.NotFound("Không có dữ liệu");
        }

        public ApiResponse<ICollection<WorkProcessResponse>> GetWorkProcessNoPaging()
        {
            var exsting = _repository.GetWorkProcess();
            var response = _mapper.Map<ICollection<WorkProcessResponse>>(exsting);
            return exsting.Any()
                ? ApiResponse<ICollection<WorkProcessResponse>>.Success(response)
                : ApiResponse<ICollection<WorkProcessResponse>>.NotFound("Không có dữ liệu");
        }


        public ApiResponse<WorkProcessResponse> GetWorkProcessById(long id)
        {
            var workProcess = _repository.GetWorkProcessById(id);
            return workProcess != null
                ? ApiResponse<WorkProcessResponse>.Success(_mapper.Map<WorkProcessResponse>(workProcess))
                : ApiResponse<WorkProcessResponse>.NotFound($"Không tìm thấy trạng quá trình công tác #{id}");
        }

        public ApiResponse<WorkProcess> UpdateWorkProcess(long id, WorkProcessRequest workProcessRequest)
        {
            var workProcess = _mapper.Map<WorkProcess>(workProcessRequest);
            var updated = _repository.UpdateWorkProcess(id, workProcessRequest);
            return updated != null
                ? ApiResponse<WorkProcess>.Success(updated)
                : ApiResponse<WorkProcess>.NotFound("Không tìm thấy trạng thái quá trình công tác để cập nhật");
        }

        
    }
    
}
