using ISC_ELIB_SERVER.Models;
using ISC_ELIB_SERVER.Repositories;
using ISC_ELIB_SERVER.DTOs.Responses;
using ISC_ELIB_SERVER.DTOs.Requests;
using AutoMapper;

namespace ISC_ELIB_SERVER.Services
{
    public interface ISupportService
    {
        ApiResponse<ICollection<SupportResponse>> GetSupports(int page, int pageSize, string search, string sortColumn, string sortOrder);
        ApiResponse<SupportResponse> GetSupportById(long id);
        ApiResponse<SupportResponse> CreateSupport(SupportRequest SupportRequest);
        ApiResponse<Support> UpdateSupport(Support Support);
        ApiResponse<Support> DeleteSupport(long id);
    }

    public class SupportService : ISupportService
    {
        private readonly SupportRepo _repository;
        private readonly IMapper _mapper;

        public SupportService(SupportRepo repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public ApiResponse<ICollection<SupportResponse>> GetSupports(int page, int pageSize, string search, string sortColumn, string sortOrder)
        {
            var query = _repository.GetSupports().AsQueryable();
            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(n => n.Title.Contains(search) || n.Content.Contains(search));
            }

            query = sortColumn switch
            {
                "CreateAt" => sortOrder.Equals("desc", StringComparison.OrdinalIgnoreCase) ? query.OrderByDescending(n => n.CreateAt) : query.OrderBy(n => n.CreateAt),
                "Id" => sortOrder.Equals("desc", StringComparison.OrdinalIgnoreCase) ? query.OrderByDescending(n => n.Id) : query.OrderBy(n => n.Id),
                _ => query.OrderBy(n => n.Id)
            };

            var result = query.Skip((page - 1) * pageSize).Take(pageSize).ToList();
            var response = _mapper.Map<ICollection<SupportResponse>>(result);
            return result.Any() ? ApiResponse<ICollection<SupportResponse>>.Success(response) : ApiResponse<ICollection<SupportResponse>>.NotFound("Không có dữ liệu");
        }

        public ApiResponse<SupportResponse> GetSupportById(long id)
        {
            var Support = _repository.GetSupportById(id);
            return Support != null ? ApiResponse<SupportResponse>.Success(_mapper.Map<SupportResponse>(Support)) : ApiResponse<SupportResponse>.NotFound($"Không tìm thấy thông báo #{id}");
        }

        public ApiResponse<SupportResponse> CreateSupport(SupportRequest SupportRequest)
        {
            var Support = _mapper.Map<Support>(SupportRequest);
            var created = _repository.CreateSupport(Support);
            return ApiResponse<SupportResponse>.Success(_mapper.Map<SupportResponse>(created));
        }

        public ApiResponse<Support> UpdateSupport(Support Support)
        {
            var updated = _repository.UpdateSupport(Support);
            return updated != null ? ApiResponse<Support>.Success(updated) : ApiResponse<Support>.NotFound("Không tìm thấy thông báo để cập nhật");
        }

        public ApiResponse<Support> DeleteSupport(long id)
        {
            var success = _repository.DeleteSupport(id);
            return success ? ApiResponse<Support>.Success() : ApiResponse<Support>.NotFound("Không tìm thấy thông báo để xóa");
        }
    }
}
