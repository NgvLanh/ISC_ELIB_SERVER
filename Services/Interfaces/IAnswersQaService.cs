﻿using ISC_ELIB_SERVER.Models;
using ISC_ELIB_SERVER.Repositories;
using ISC_ELIB_SERVER.DTOs.Responses;
using ISC_ELIB_SERVER.DTOs.Requests;
using AutoMapper;

namespace ISC_ELIB_SERVER.Services
{
    public interface IAnswersQaService
    {
        ApiResponse<ICollection<AnswersQaResponse>> GetAnswers(long? questionId);
        ApiResponse<AnswersQaResponse> GetAnswerById(long id);
        ApiResponse<AnswersQaResponse> CreateAnswer(AnswersQaRequest answerRequest);
        ApiResponse<AnswersQaResponse> UpdateAnswer(long id, AnswersQaRequest answerRequest);
        ApiResponse<AnswersQaResponse> DeleteAnswer(long id);
    }

  
}
