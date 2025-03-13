using ISC_ELIB_SERVER.Models;
using ISC_ELIB_SERVER.Repositories;
using ISC_ELIB_SERVER.DTOs.Responses;
using ISC_ELIB_SERVER.DTOs.Requests;
using ISC_ELIB_SERVER.Services.Interfaces;
using AutoMapper;
using System.Collections.Generic;
using System.Linq;
using ISC_ELIB_SERVER.DTOs.Responses.ISC_ELIB_SERVER.DTOs.Responses;

namespace ISC_ELIB_SERVER.Services
{
    public class StudentScoreService : IStudentScoreService
    {
        private readonly IStudentScoreRepo _repository;
        private readonly IMapper _mapper;

        public StudentScoreService(IStudentScoreRepo repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public ApiResponse<StudentScoreResponse> GetStudentScoreById(int id)
        {
            var studentScore = _repository.GetStudentScoreById(id);
            return studentScore != null
                ? ApiResponse<StudentScoreResponse>.Success(_mapper.Map<StudentScoreResponse>(studentScore))
                : ApiResponse<StudentScoreResponse>.NotFound($"Không có dữ liệu");
        }

        public ApiResponse<StudentScoreResponse> CreateStudentScore(StudentScoreRequest studentScoreRequest)
        {
            var created = _repository.CreateStudentScore(new StudentScore()
            {
                Score = studentScoreRequest.Score,
                UserId = studentScoreRequest.UserId,
                ScoreTypeId = studentScoreRequest.ScoreTypeId,
                SubjectId = studentScoreRequest.SubjectId,
                SemesterId = studentScoreRequest.SemesterId
            });

            return ApiResponse<StudentScoreResponse>.Success(_mapper.Map<StudentScoreResponse>(created));
        }

        public ApiResponse<StudentScoreResponse> UpdateStudentScore(int id, StudentScoreRequest studentScoreRequest)
        {
            var existingStudentScore = _repository.GetStudentScoreById(id);

            if (existingStudentScore == null)
            {
                return ApiResponse<StudentScoreResponse>.NotFound($"Không tìm thấy StudentScore với ID {id}");
            }
            existingStudentScore.Score = studentScoreRequest.Score;
            existingStudentScore.UserId = studentScoreRequest.UserId;
            existingStudentScore.ScoreTypeId = studentScoreRequest.ScoreTypeId;
            existingStudentScore.SubjectId = studentScoreRequest.SubjectId;
            existingStudentScore.SemesterId = studentScoreRequest.SemesterId;

            var updated = _repository.UpdateStudentScore(existingStudentScore);

            return ApiResponse<StudentScoreResponse>.Success(_mapper.Map<StudentScoreResponse>(updated));
        }

        public ApiResponse<StudentScore> DeleteStudentScore(int id)
        {
            var success = _repository.DeleteStudentScore(id);
            return success
                ? ApiResponse<StudentScore>.Success()
                : ApiResponse<StudentScore>.NotFound("Không tìm thấy để xóa");
        }

        public ApiResponse<ICollection<StudentScoreResponse>> GetStudentScores(int? page, int? pageSize, string? sortColumn, string? sortOrder)
        {
            var query = _repository.GetStudentScores().AsQueryable();

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

            var response = _mapper.Map<ICollection<StudentScoreResponse>>(result);

            return result.Any() ? ApiResponse<ICollection<StudentScoreResponse>>
            .Success(response, page, pageSize, _repository.GetStudentScores().Count)
             : ApiResponse<ICollection<StudentScoreResponse>>.NotFound("Không có dữ liệu");
        }
    }
}
