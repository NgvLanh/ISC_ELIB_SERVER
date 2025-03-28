using ISC_ELIB_SERVER.Models;
using ISC_ELIB_SERVER.Repositories;
using ISC_ELIB_SERVER.DTOs.Responses;
using ISC_ELIB_SERVER.DTOs.Requests;
using AutoMapper;
using ISC_ELIB_SERVER.Repositories.ISC_ELIB_SERVER.Repositories;

namespace ISC_ELIB_SERVER.Services
{
    public interface IEntryTypeService
    {
        ApiResponse<ICollection<EntryTypeResponse>> GetEntryTypes(int page, int pageSize, string search, string sortColumn, string sortOrder);
        ApiResponse<EntryTypeResponse> GetEntryTypeById(long id);
        ApiResponse<EntryTypeResponse> GetEntryTypeByName(string name);
        ApiResponse<EntryTypeResponse> CreateEntryType(EntryTypeRequest entryTypeRequest);
        ApiResponse<EntryType> UpdateEntryType(EntryType entryType);
        ApiResponse<EntryType> DeleteEntryType(long id);
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

            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(et => et.Name.ToLower().Contains(search.ToLower()));
            }

            query = sortColumn switch
            {
                "Name" => sortOrder.ToLower() == "desc" ? query.OrderByDescending(et => et.Name) : query.OrderBy(et => et.Name),
                "Id" => sortOrder.ToLower() == "desc" ? query.OrderByDescending(et => et.Id) : query.OrderBy(et => et.Id),
                _ => query.OrderBy(et => et.Id)
            };

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

        public ApiResponse<EntryTypeResponse> GetEntryTypeByName(string name)
        {
            var entryType = _repository.GetEntryTypes().FirstOrDefault(et => et.Name?.ToLower() == name.ToLower());
            return entryType != null
                ? ApiResponse<EntryTypeResponse>.Success(_mapper.Map<EntryTypeResponse>(entryType))
                : ApiResponse<EntryTypeResponse>.NotFound($"Không tìm thấy loại đầu vào có tên: {name}");
        }

        public ApiResponse<EntryTypeResponse> CreateEntryType(EntryTypeRequest entryTypeRequest)
        {
            var existing = _repository.GetEntryTypes().FirstOrDefault(et => et.Name?.ToLower() == entryTypeRequest.Name.ToLower());
            if (existing != null)
            {
                return ApiResponse<EntryTypeResponse>.Conflict("Tên loại đầu vào đã tồn tại");
            }

            var created = _repository.CreateEntryType(new EntryType() { Name = entryTypeRequest.Name });
            return ApiResponse<EntryTypeResponse>.Success(_mapper.Map<EntryTypeResponse>(created));
        }

        public ApiResponse<EntryType> UpdateEntryType(EntryType entryType)
        {
            var updated = _repository.UpdateEntryType(entryType);
            return updated != null
                ? ApiResponse<EntryType>.Success(updated)
                : ApiResponse<EntryType>.NotFound("Không tìm thấy loại đầu vào để cập nhật");
        }

        public ApiResponse<EntryType> DeleteEntryType(long id)
        {
            var success = _repository.DeleteEntryType(id);
            return success
                ? ApiResponse<EntryType>.Success()
                : ApiResponse<EntryType>.NotFound("Không tìm thấy loại đầu vào để xóa");
        }
    }
}
