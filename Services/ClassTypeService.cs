using AutoMapper;
using ISC_ELIB_SERVER.DTOs.Requests;
using ISC_ELIB_SERVER.DTOs.Responses;
using ISC_ELIB_SERVER.Models;
using ISC_ELIB_SERVER.Repositories;
using System.Linq;
using System.Linq.Dynamic.Core;

namespace ISC_ELIB_SERVER.Services
{
    public class ClassTypeService : IClassTypeService
    {
        private readonly IClassTypeRepo _repository;
        private readonly ClassRepo _classRepository;
        private readonly AcademicYearRepo _academicYearRepository;
        private readonly IMapper _mapper;

        public ClassTypeService(IClassTypeRepo repository, ClassRepo classRepository, AcademicYearRepo academicYearRepository, IMapper mapper)
        {
            _repository = repository;
            _classRepository = classRepository;
            _academicYearRepository = academicYearRepository;
            _mapper = mapper;
        }

        public ApiResponse<ICollection<ClassTypeResponse>> GetClassTypes(int? page, int? pageSize, string? sortColumn, string? sortOrder)
        {
            var query = _repository.GetClassTypes().AsQueryable();

            query = sortColumn switch
            {
                "Id" => sortOrder.ToLower() == "desc" ? query.OrderByDescending(us => us.Id) : query.OrderBy(us => us.Id),
                _ => query.OrderBy(ay => ay.Id)
            };

            if (page.HasValue && pageSize.HasValue)
            {
                query = query.Skip((page.Value - 1) * pageSize.Value).Take(pageSize.Value);
            }

            var result = query.ToList();
            var response = _mapper.Map<ICollection<ClassTypeResponse>>(result);

            return result.Any()
                ? ApiResponse<ICollection<ClassTypeResponse>>.Success(response, page, pageSize, _repository.GetClassTypes().Count)
                : ApiResponse<ICollection<ClassTypeResponse>>.NotFound("Không có dữ liệu");
        }

        public ApiResponse<ClassTypeResponse> GetClassTypeById(int id)
        {
            var classType = _repository.GetClassTypeById(id);
            return classType != null
                ? ApiResponse<ClassTypeResponse>.Success(_mapper.Map<ClassTypeResponse>(classType))
                : ApiResponse<ClassTypeResponse>.NotFound("Không tìm thấy loại lớp");
        }

        public ApiResponse<ICollection<ClassTypeResponse>> GetClassTypeByName(NameRequest request)
        {
            if (request == null || string.IsNullOrWhiteSpace(request.Name))
            {
                return ApiResponse<ICollection<ClassTypeResponse>>.BadRequest("Tên không được để trống");
            }

            var classTypes = _repository.GetClassTypes()
                .Where(ct => ct.Name.ToLower().Contains(request.Name.ToLower()))
                .ToList();

            var response = _mapper.Map<ICollection<ClassTypeResponse>>(classTypes);

            return response.Any()
                ? ApiResponse<ICollection<ClassTypeResponse>>.Success(response)
                : ApiResponse<ICollection<ClassTypeResponse>>.NotFound("Không tìm thấy loại lớp phù hợp");
        }




        public ApiResponse<ClassTypeResponse> CreateClassType(ClassTypeRequest classTypeRequest)
        {
            var existingClassType = _repository.GetClassTypes().FirstOrDefault(ct => ct.Name.ToLower() == classTypeRequest.Name.ToLower());
            if (existingClassType != null)
            {
                return ApiResponse<ClassTypeResponse>.Conflict("Tên loại lớp đã tồn tại");
            }

            var classType = _mapper.Map<ClassType>(classTypeRequest);
            _repository.CreateClassType(classType);
            return ApiResponse<ClassTypeResponse>.Success(_mapper.Map<ClassTypeResponse>(classType));
        }

        public ApiResponse<ClassTypeResponse> UpdateClassType(int id, ClassTypeRequest classTypeRequest)
        {
            var existingClassType = _repository.GetClassTypeById(id);
            if (existingClassType == null)
            {
                return ApiResponse<ClassTypeResponse>.NotFound("Không tìm thấy loại lớp");
            }

            var duplicate = _repository.GetClassTypes().FirstOrDefault(ct => ct.Name.ToLower() == classTypeRequest.Name.ToLower() && ct.Id != id);
            if (duplicate != null)
            {
                return ApiResponse<ClassTypeResponse>.Conflict("Tên loại lớp đã tồn tại");
            }

            existingClassType.Name = classTypeRequest.Name;
            existingClassType.Description = classTypeRequest.Description;

            var updatedClassType = _repository.UpdateClassType(existingClassType);
            return ApiResponse<ClassTypeResponse>.Success(_mapper.Map<ClassTypeResponse>(updatedClassType));
        }

        public ApiResponse<bool> DeleteClassType(int id)
        {
            var deleted = _repository.DeleteClassType(id);
            return deleted
                ? ApiResponse<bool>.Success(true)
                : ApiResponse<bool>.NotFound("Không tìm thấy loại lớp để xóa");
        }

        public ApiResponse<ICollection<ClassTypeResponse>> GetClassTypesBySchoolYear(YearRequest year)
        {
            // Kiểm tra đầu vào
            if (year == null || string.IsNullOrWhiteSpace(year.Year))
            {
                return ApiResponse<ICollection<ClassTypeResponse>>.BadRequest("Niên khóa không được để trống");
            }

            // Tìm niên khóa
            var academicYear = _academicYearRepository.GetAcademicYears()
                .FirstOrDefault(ay => $"{ay.StartTime?.Year ?? 0}-{ay.EndTime?.Year ?? 0}" == year.Year);

            if (academicYear == null)
            {
                return ApiResponse<ICollection<ClassTypeResponse>>.NotFound("Niên khóa không tồn tại");
            }

            // Lấy danh sách ID loại lớp theo niên khóa
            var classTypeIds = _classRepository.GetClass()
                .Where(c => c.AcademicYearId == academicYear.Id)
                .Select(c => c.ClassTypeId)
                .Distinct()
                .ToList();

            // Truy vấn danh sách loại lớp
            var result = _repository.GetClassTypes()
                .Where(ct => classTypeIds.Contains(ct.Id))
                .ToList();

            // Mapping kết quả
            var response = _mapper.Map<ICollection<ClassTypeResponse>>(result);

            return response.Any()
                ? ApiResponse<ICollection<ClassTypeResponse>>.Success(response)
                : ApiResponse<ICollection<ClassTypeResponse>>.NotFound("Không có dữ liệu");
        }

    }
}
