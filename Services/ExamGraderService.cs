using ISC_ELIB_SERVER.DTOs.Requests;
using ISC_ELIB_SERVER.DTOs.Responses;
using ISC_ELIB_SERVER.Models;
using ISC_ELIB_SERVER.Service;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
namespace ISC_ELIB_SERVER.Services
{
    public class ExamGraderService : IExamGraderService
    {
        private readonly ExamGraderRepository _repository;

        public ExamGraderService(ExamGraderRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<ExamGraderResponse>> GetAllAsync(int page, int pageSize, string? sortBy, bool isDescending, int? examId, int? userId)
        {
            var examGraders = await _repository.GetAllAsync(page, pageSize, sortBy, isDescending, examId, userId);
            return examGraders.Select(x => new ExamGraderResponse
            {
                Id = x.Id,
                ExamId = x.ExamId,
                UserId = x.UserId,
                ClassIds = x.ClassIds
            }).ToList();
        }

        public async Task<ExamGraderResponse?> GetByIdAsync(int id)
        {
            var examGrader = await _repository.GetByIdAsync(id);
            if (examGrader == null) return null;

            return new ExamGraderResponse
            {
                Id = examGrader.Id,
                ExamId = examGrader.ExamId,
                UserId = examGrader.UserId,
                ClassIds = examGrader.ClassIds
            };
        }

        public async Task AddAsync(ExamGraderRequest request)
        {
            var examGrader = new ExamGrader
            {
                ExamId = request.ExamId,
                UserId = request.UserId,
                ClassIds = request.ClassIds,
                Active = request.Active
            };
            await _repository.AddAsync(examGrader);
        }

        public async Task UpdateAsync(int id, ExamGraderRequest request)
        {
            var existingExamGrader = await _repository.GetByIdAsync(id);
            if (existingExamGrader == null) return;

            existingExamGrader.ExamId = request.ExamId;
            existingExamGrader.UserId = request.UserId;
            existingExamGrader.ClassIds = request.ClassIds;
            existingExamGrader.Active = request.Active;

            await _repository.UpdateAsync(existingExamGrader);
        }

        public async Task DeleteAsync(int id)
        {
            await _repository.DeleteAsync(id);
        }
    }
}
