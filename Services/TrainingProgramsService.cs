using AutoMapper;
using ISC_ELIB_SERVER.DTOs.Requests;
using ISC_ELIB_SERVER.DTOs.Responses;
using ISC_ELIB_SERVER.Models;
using ISC_ELIB_SERVER.Repositories;
using System.Xml.Linq;
using System;
using ISC_ELIB_SERVER.Services.Interfaces;

namespace ISC_ELIB_SERVER.Services
{
    public class TrainingProgramsService : ITrainingProgramService
    {
        private readonly TrainingProgramsRepo _repository;
        private readonly MajorRepo _Majorrepository;
        private readonly IMapper _mapper;

        public TrainingProgramsService(TrainingProgramsRepo repository, MajorRepo majorrepository, IMapper mapper)
        {
            _repository = repository;
            _Majorrepository = majorrepository;
            _mapper = mapper;
        }

        public ApiResponse<ICollection<TrainingProgramsResponse>> GetTrainingPrograms(int page, int pageSize, string search, string sortColumn, string sortOrder)
        {
            var query = _repository.GetTrainingProgram().AsQueryable();

            query = query.Where(us => us.Active == null || us.Active == false);

            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(us => us.Name.ToLower().Contains(search.ToLower()));
            }

            query = sortColumn switch
            {
                "Name" => sortOrder.ToLower() == "desc" ? query.OrderByDescending(us => us.Name) : query.OrderBy(us => us.Name),
                "Id" => sortOrder.ToLower() == "desc" ? query.OrderByDescending(us => us.Id) : query.OrderBy(us => us.Id),
                _ => query.OrderBy(us => us.Id)
            };

            var result = query.Skip((page - 1) * pageSize).Take(pageSize).ToList();

            var response = _mapper.Map<ICollection<TrainingProgramsResponse>>(result);

            return result.Any()
                ? ApiResponse<ICollection<TrainingProgramsResponse>>.Success(response)
                : ApiResponse<ICollection<TrainingProgramsResponse>>.NotFound("Không có dữ liệu");
        }

        public ApiResponse<TrainingProgramsResponse> GetTrainingProgramsById(long id)
        {
            var trainingProgram = _repository.GetTrainingProgramById(id);
            return (trainingProgram != null && (trainingProgram.Active == false))
                ? ApiResponse<TrainingProgramsResponse>.Success(_mapper.Map<TrainingProgramsResponse>(trainingProgram))
                : ApiResponse<TrainingProgramsResponse>.NotFound($"Không tìm thấy chương trình đào tạo #{id}");
        }

        public ApiResponse<TrainingProgramsResponse> CreateTrainingPrograms(TrainingProgramsRequest trainingProgramsRequest)
        {
            var existing = _repository.GetTrainingProgram().FirstOrDefault(us => us.Name?.ToLower() == trainingProgramsRequest.Name.ToLower());
            if (existing != null)
            {
                return ApiResponse<TrainingProgramsResponse>.Conflict("Tên chương trình đào tạo đã tồn tại");
            }

            var majorExists = _Majorrepository.GetMajor()
            .Any(m => m.Id == trainingProgramsRequest.MajorId);
            if (!majorExists)
            {
                return ApiResponse<TrainingProgramsResponse>.Conflict("Chủ đề không tồn tại");
            }

            var created = _repository.CreateTrainingProgram(new TrainingProgram()
            {
                Name = trainingProgramsRequest.Name,
                MajorId = trainingProgramsRequest.MajorId,
                SchoolFacilitiesId = trainingProgramsRequest.SchoolFacilitiesId,
                StartDate = trainingProgramsRequest.StartDate,
                EndDate = trainingProgramsRequest.EndDate,
                TrainingForm = trainingProgramsRequest.TrainingForm,
                Active = false,
                FileName = trainingProgramsRequest.FileName,
                FilePath = trainingProgramsRequest.FilePath,
                Degree = trainingProgramsRequest.Degree
            });
            return ApiResponse<TrainingProgramsResponse>.Success(_mapper.Map<TrainingProgramsResponse>(created));
        }

        public ApiResponse<TrainingProgramsResponse> UpdateTrainingPrograms(long id, TrainingProgramsRequest trainingProgramsRequest)
        {
            var existingTrainingPrograms = _repository.GetTrainingProgramById(id);
            if (existingTrainingPrograms == null || existingTrainingPrograms.Active == true)
            {
                return ApiResponse<TrainingProgramsResponse>.NotFound("Không tìm thấy chương trình đào tạo.");
            }

            var existing = _repository.GetTrainingProgram()
                .FirstOrDefault(us => us.Id != id && us.Name?.ToLower() == trainingProgramsRequest.Name.ToLower());
            if (existing != null)
            {
                return ApiResponse<TrainingProgramsResponse>.Conflict("Tên chương trình đào tạo đã tồn tại");
            }

            existingTrainingPrograms.Name = trainingProgramsRequest.Name;
            existingTrainingPrograms.MajorId = trainingProgramsRequest.MajorId;
            existingTrainingPrograms.SchoolFacilitiesId = trainingProgramsRequest.SchoolFacilitiesId;
            existingTrainingPrograms.Degree = trainingProgramsRequest.Degree;
            existingTrainingPrograms.StartDate = trainingProgramsRequest.StartDate;
            existingTrainingPrograms.EndDate = trainingProgramsRequest.EndDate;
            existingTrainingPrograms.TrainingForm = trainingProgramsRequest.TrainingForm;
            existingTrainingPrograms.Active = false;
            existingTrainingPrograms.FileName = trainingProgramsRequest.FileName;
            existingTrainingPrograms.FilePath = trainingProgramsRequest.FilePath;
            _repository.UpdateTrainingProgram(existingTrainingPrograms);
            return ApiResponse<TrainingProgramsResponse>.Success(_mapper.Map<TrainingProgramsResponse>(existingTrainingPrograms));
        }

        public ApiResponse<TrainingProgram> DeleteTrainingPrograms(long id)
        {
            var existingTrainingProgram = _repository.GetTrainingProgramById(id);
            if (existingTrainingProgram == null)
            {
                return ApiResponse<TrainingProgram>.NotFound("Không tìm thấy Chương trình đào tạo.");
            }

            if (existingTrainingProgram.Active == true)
            {
                return ApiResponse<TrainingProgram>.Conflict("Chương trình đào tạo không tồn tại.");
            }

            existingTrainingProgram.Active = true;
            _repository.DeleteTrainingProgram(existingTrainingProgram);

            return ApiResponse<TrainingProgram>.Success();
        }
    }
}
