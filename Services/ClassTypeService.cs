using AutoMapper;
using CloudinaryDotNet;
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


        public ApiResponse<ClassTypeResponse> GetClassTypeById(int id)
        {
            var classType = _repository.GetClassTypeById(id);
            return classType != null
                ? ApiResponse<ClassTypeResponse>.Success(_mapper.Map<ClassTypeResponse>(classType))
                : ApiResponse<ClassTypeResponse>.NotFound("Không tìm thấy loại lớp");
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

        public ApiResponse<ICollection<ClassTypeResponse>> GetClassTypes(
    int? page, int? pageSize, int? searchYear, string? searchName, string? sortColumn, string? sortOrder)
        {
            if (searchYear == null)
            {
                return ApiResponse<ICollection<ClassTypeResponse>>.BadRequest("Niên khóa không được để trống");
            }

            var academicYear = _academicYearRepository.GetAcademicYears()
                .FirstOrDefault(ay => ay.Id == searchYear);

            if (academicYear == null)
            {
                return ApiResponse<ICollection<ClassTypeResponse>>.NotFound("Niên khóa không tồn tại");
            }

            var classTypeIds = _classRepository.GetClass()
                .Where(c => c.AcademicYearId == academicYear.Id)
                .Select(c => c.ClassTypeId)
                .Distinct()
                .ToList();

            var query = _repository.GetClassTypes()
                .Where(ct => classTypeIds.Contains(ct.Id))
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(searchName))
            {
                query = query.Where(st => st.Name.ToLower().Contains(searchName.ToLower()));
            }

            query = sortColumn switch
            {
                "Id" => (sortOrder?.ToLower() == "desc") ? query.OrderByDescending(ct => ct.Id) : query.OrderBy(ct => ct.Id),
                _ => query.OrderBy(ct => ct.Id)
            };

            if (page.HasValue && pageSize.HasValue)
            {
                query = query.Skip((page.Value - 1) * pageSize.Value).Take(pageSize.Value);
            }

            var result = query.ToList();
            var response = _mapper.Map<ICollection<ClassTypeResponse>>(result);

            return response.Any()
                ? ApiResponse<ICollection<ClassTypeResponse>>.Success(response, page, pageSize, classTypeIds.Count)
                : ApiResponse<ICollection<ClassTypeResponse>>.NotFound("Không có dữ liệu");
        }


    }
}
