using AutoMapper;
using ISC_ELIB_SERVER.DTOs.Responses;
using ISC_ELIB_SERVER.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

public class ExamGraderService : IExamGraderService
{
    private readonly ExamGraderRepository _repository;
    private readonly IMapper _mapper;

    public ExamGraderService(ExamGraderRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<List<ExamGraderResponse>> GetAllExamGradersAsync()
    {
        var examGraders = await _repository.GetAllExamGradersAsync();
        return _mapper.Map<List<ExamGraderResponse>>(examGraders);
    }

    public async Task<ExamGraderResponse?> GetExamGraderByIdAsync(int id)
    {
        var examGrader = await _repository.GetExamGraderByIdAsync(id);
        return examGrader != null ? _mapper.Map<ExamGraderResponse>(examGrader) : null;
    }

    public async Task<ExamGraderResponse> CreateExamGraderAsync(ExamGraderRequest request)
    {
        var examGrader = _mapper.Map<ExamGrader>(request);
        await _repository.CreateExamGraderAsync(examGrader);
        return _mapper.Map<ExamGraderResponse>(examGrader);
    }

    public async Task<ExamGraderResponse?> UpdateExamGraderAsync(int id, ExamGraderRequest request)
    {
        var existingExamGrader = await _repository.GetExamGraderByIdAsync(id);
        if (existingExamGrader == null) return null;

        _mapper.Map(request, existingExamGrader);
        await _repository.UpdateExamGraderAsync(existingExamGrader);

        return _mapper.Map<ExamGraderResponse>(existingExamGrader);
    }

    public async Task<bool> DeleteExamGraderAsync(int id)
    {
        return await _repository.DeleteExamGraderAsync(id);
    }
}
