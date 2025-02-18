using ISC_ELIB_SERVER.DTOs.Requests;
using ISC_ELIB_SERVER.DTOs.Responses;
using ISC_ELIB_SERVER.Models;
using ISC_ELIB_SERVER.Repositories;
using AutoMapper;

namespace ISC_ELIB_SERVER.Services
{
    public interface IEntryTypeService
    {
        ApiResponse<ICollection<EntryTypeResponse>> GetEntryTypes(int page, int pageSize, string search, string sortColumn, string sortOrder);
        ApiResponse<EntryTypeResponse> GetEntryTypeById(long id);
        ApiResponse<EntryTypeResponse> CreateEntryType(EntryTypeRequest entryTypeRequest);
        ApiResponse<EntryTypeResponse> UpdateEntryType(long id, EntryTypeRequest entryTypeRequest);
        ApiResponse<object> DeleteEntryType(long id);
    }

    public class EntryTypeService : IEntryTypeService
    {
        private readonly EntryTypeRepo _repository;
        private readonly IMapper _mapper;

        public EntryTypeService(EntryTypeRepo repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public ApiResponse<ICollection<EntryTypeResponse>> GetEntryTypes(int page, int pageSize, string search, string sortColumn, string sortOrder)
        {
            var query = _repository.GetEntryTypes().AsQueryable();

            // Tìm kiếm theo tên
            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(et => et.Name.ToLower().Contains(search.ToLower()));
            }

            // Sắp xếp dữ liệu
            query = sortColumn switch
            {
                "Name" => sortOrder.ToLower() == "desc" ? query.OrderByDescending(et => et.Name) : query.OrderBy(et => et.Name),
                "Id" => sortOrder.ToLower() == "desc" ? query.OrderByDescending(et => et.Id) : query.OrderBy(et => et.Id),
                _ => query.OrderBy(et => et.Id)
            };

            // Phân trang
            var result = query.Skip((page - 1) * pageSize).Take(pageSize).ToList();
            var response = _mapper.Map<ICollection<EntryTypeResponse>>(result);

            return result.Any()
                ? ApiResponse<ICollection<EntryTypeResponse>>.Success(response)
                : ApiResponse<ICollection<EntryTypeResponse>>.NotFound("Không có dữ liệu");
        }

        public ApiResponse<EntryTypeResponse> GetEntryTypeById(long id)
        {
            var entryType = _repository.GetEntryTypeById(id);
            return entryType != null
                ? ApiResponse<EntryTypeResponse>.Success(_mapper.Map<EntryTypeResponse>(entryType))
                : ApiResponse<EntryTypeResponse>.NotFound($"Không tìm thấy loại đầu vào #{id}");
        }

        public ApiResponse<EntryTypeResponse> CreateEntryType(EntryTypeRequest entryTypeRequest)
        {
            // Kiểm tra tên đã tồn tại chưa
            var existing = _repository.GetEntryTypes().FirstOrDefault(et => et.Name?.ToLower() == entryTypeRequest.Name.ToLower());
            if (existing != null)
            {
                return ApiResponse<EntryTypeResponse>.Conflict("Tên loại đầu vào đã tồn tại");
            }

            // Tạo mới
            var newEntryType = new EntryType { Name = entryTypeRequest.Name };
            var created = _repository.CreateEntryType(newEntryType);
            return ApiResponse<EntryTypeResponse>.Success(_mapper.Map<EntryTypeResponse>(created));
        }

        public ApiResponse<EntryTypeResponse> UpdateEntryType(long id, EntryTypeRequest entryTypeRequest)
        {
            var existingEntryType = _repository.GetEntryTypeById(id);
            if (existingEntryType == null)
            {
                return ApiResponse<EntryTypeResponse>.NotFound($"Không tìm thấy loại đầu vào #{id} để cập nhật");
            }

            existingEntryType.Name = entryTypeRequest.Name;
            var updated = _repository.UpdateEntryType(existingEntryType);
            return ApiResponse<EntryTypeResponse>.Success(_mapper.Map<EntryTypeResponse>(updated));
        }

        public ApiResponse<object> DeleteEntryType(long id)
        {
            var existingEntryType = _repository.GetEntryTypeById(id);
            if (existingEntryType == null)
            {
                return ApiResponse<object>.NotFound($"Không tìm thấy loại đầu vào #{id} để xóa");
            }

            var success = _repository.DeleteEntryType(id);
            return success
                ? ApiResponse<object>.Success("Xóa thành công")
                : ApiResponse<object>.Error("Xóa thất bại");
        }
    }
}
