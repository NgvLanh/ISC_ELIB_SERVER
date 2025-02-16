using ISC_ELIB_SERVER.DTOs.Requests;
using ISC_ELIB_SERVER.DTOs.Responses;
using System.Collections.Generic;

namespace ISC_ELIB_SERVER.Services
{
    public interface IClassTypeService
    {
        ApiResponse<ICollection<ClassTypeResponse>> GetClassTypes(int page, int pageSize, string search, string sortColumn, string sortOrder);
        ApiResponse<ClassTypeResponse> GetClassTypeById(long id);
        ApiResponse<ClassTypeResponse> GetClassTypeByName(string name);
        ApiResponse<ClassTypeResponse> CreateClassType(ClassTypeRequest classTypeRequest);
        ApiResponse<ClassTypeResponse> UpdateClassType(long id, ClassTypeRequest classTypeRequest);
        ApiResponse<bool> DeleteClassType(long id);
    }
}
