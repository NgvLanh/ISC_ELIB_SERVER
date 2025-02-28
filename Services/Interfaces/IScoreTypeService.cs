using ISC_ELIB_SERVER.DTOs.Responses;
using ISC_ELIB_SERVER.DTOs.Requests;
using System.Collections.Generic;
using ISC_ELIB_SERVER.DTOs.Responses.ISC_ELIB_SERVER.DTOs.Responses;
using ISC_ELIB_SERVER.Models;

namespace ISC_ELIB_SERVER.Services.Interfaces
{
    public interface IScoreTypeService
    {
        ApiResponse<ICollection<ScoreTypeResponse>> GetScoreTypes(int page, int pageSize, string search, string sortColumn, string sortOrder);
        ApiResponse<ScoreTypeResponse> GetScoreTypeById(long id);
        ApiResponse<ScoreTypeResponse> GetScoreTypeByName(string name);
        ApiResponse<ScoreTypeResponse> CreateScoreType(ScoreTypeRequest scoreTypeRequest);
        ApiResponse<ScoreTypeResponse> UpdateScoreType(long id, ScoreTypeRequest scoreTypeRequest);
        ApiResponse<ScoreType> DeleteScoreType(long id);
    }
}
