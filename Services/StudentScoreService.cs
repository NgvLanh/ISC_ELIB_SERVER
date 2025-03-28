using ISC_ELIB_SERVER.Models;
using ISC_ELIB_SERVER.Repositories;
using ISC_ELIB_SERVER.DTOs.Responses;
using ISC_ELIB_SERVER.DTOs.Requests;
using AutoMapper;
using System.Collections.Generic;
using System.Linq;

namespace ISC_ELIB_SERVER.Services
{
    public interface IStudentScoreService
    {
        ApiResponse<ICollection<StudentScoreResponse>> GetStudentScores(int page, int pageSize, string search, string sortColumn, string sortOrder);
        ApiResponse<StudentScoreResponse> GetStudentScoreById(long id);
        ApiResponse<StudentScoreResponse> CreateStudentScore(StudentScoreRequest studentScoreRequest);
        ApiResponse<StudentScoreResponse> UpdateStudentScore(long id, StudentScoreRequest studentScoreRequest);
        ApiResponse<StudentScore> DeleteStudentScore(long id);
    }

    public class StudentScoreService : IStudentScoreService
    {
        private readonly IStudentScoreRepo _repository;
        private readonly IMapper _mapper;

        public StudentScoreService(IStudentScoreRepo repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public ApiResponse<ICollection<StudentScoreResponse>> GetStudentScores(int page, int pageSize, string search, string sortColumn, string sortOrder)
        {
            var query = _repository.GetStudentScores().AsQueryable();

            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(ss => ss.UserId.ToString().Contains(search));
            }

            query = sortColumn switch
            {
                "ScoreValue" => sortOrder.ToLower() == "desc" ? query.OrderByDescending(ss => ss.SubjectId) : query.OrderBy(ss => ss.SubjectId),
                "Id" => sortOrder.ToLower() == "desc" ? query.OrderByDescending(ss => ss.Id) : query.OrderBy(ss => ss.Id),
                _ => query.OrderBy(ss => ss.Id)
            };

            var result = query.Skip((page - 1) * pageSize).Take(pageSize).ToList();
            var response = _mapper.Map<ICollection<StudentScoreResponse>>(result);

            return result.Any()
                ? ApiResponse<ICollection<StudentScoreResponse>>.Success(response)
                : ApiResponse<ICollection<StudentScoreResponse>>.NotFound("Không có dữ liệu");
        }

        public ApiResponse<StudentScoreResponse> GetStudentScoreById(long id)
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

        public ApiResponse<StudentScoreResponse> UpdateStudentScore(long id, StudentScoreRequest studentScoreRequest)
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

        public ApiResponse<StudentScore> DeleteStudentScore(long id)
        {
            var success = _repository.DeleteStudentScore(id);
            return success
                ? ApiResponse<StudentScore>.Success()
                : ApiResponse<StudentScore>.NotFound("Không tìm thấy để xóa");
        }
    }
}
