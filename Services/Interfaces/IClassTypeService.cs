using ISC_ELIB_SERVER.DTOs.Requests;
using ISC_ELIB_SERVER.DTOs.Responses;
using System.Collections.Generic;

namespace ISC_ELIB_SERVER.Services
{
    public interface IClassTypeService
    {
        ApiResponse<ICollection<ClassTypeResponse>> GetClassTypes(int? page, int? pageSize, string? sortColumn, string? sortOrder);
        ApiResponse<ClassTypeResponse> GetClassTypeById(int id);
        ApiResponse<ClassTypeResponse> GetClassTypeByName(string name);
        ApiResponse<ClassTypeResponse> CreateClassType(ClassTypeRequest classTypeRequest);
        ApiResponse<ClassTypeResponse> UpdateClassType(int id, ClassTypeRequest classTypeRequest);
        ApiResponse<bool> DeleteClassType(int id);
    }
}
