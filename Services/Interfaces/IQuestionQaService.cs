using ISC_ELIB_SERVER.Models;
using ISC_ELIB_SERVER.Repositories;
using ISC_ELIB_SERVER.DTOs.Responses;
using ISC_ELIB_SERVER.DTOs.Requests;
using AutoMapper;

namespace ISC_ELIB_SERVER.Services
{
    public interface IQuestionQaService
    {
        ApiResponse<ICollection<QuestionQaResponse>> GetQuestions(int userId,int page, int pageSize, string search, string sortColumn, string sortOrder);
        ApiResponse<QuestionQaResponse> GetQuestionById(long id);
        ApiResponse<QuestionQaResponse> GetQuestionByIdForUser(int id, int userId);
        Task<ApiResponse<QuestionQaResponse>> CreateQuestion(QuestionQaRequest questionRequest, List<IFormFile> files);
        ApiResponse<QuestionQaResponse> UpdateQuestion(long id, QuestionQaRequest QuestionQaRequest);
        ApiResponse<QuestionQaResponse> DeleteQuestion(long id);
        ApiResponse<ICollection<QuestionQaResponse>> GetAnsweredQuestions(int userId, int page, int pageSize);
        ApiResponse<ICollection<QuestionQaResponse>> SearchQuestionsByUserName(string userName, bool onlyAnswered = false);
        ApiResponse<ICollection<QuestionQaResponse>> GetRecentQuestions(int userId, int page, int pageSize);

    }

  
}
 