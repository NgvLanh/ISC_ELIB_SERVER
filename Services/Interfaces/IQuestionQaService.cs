using ISC_ELIB_SERVER.Models;
using ISC_ELIB_SERVER.Repositories;
using ISC_ELIB_SERVER.DTOs.Responses;
using ISC_ELIB_SERVER.DTOs.Requests;
using AutoMapper;

namespace ISC_ELIB_SERVER.Services
{
    public interface IQuestionQaService
    {
        ApiResponse<ICollection<QuestionQaResponse>> GetQuestions(int page, int pageSize, string search, string sortColumn, string sortOrder);
        ApiResponse<QuestionQaResponse> GetQuestionById(long id);
        ApiResponse<QuestionQaResponse> CreateQuestion(QuestionQaRequest questionRequest);
        ApiResponse<QuestionQaResponse> UpdateQuestion(long id, QuestionQaRequest QuestionQaRequest);
        ApiResponse<QuestionQaResponse> DeleteQuestion(long id);
    }

  
}
 