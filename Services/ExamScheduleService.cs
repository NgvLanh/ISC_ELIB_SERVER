using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using ISC_ELIB_SERVER.DTOs.Requests;
using ISC_ELIB_SERVER.DTOs.Responses;
using ISC_ELIB_SERVER.Models;
using ISC_ELIB_SERVER.Repositories;

namespace ISC_ELIB_SERVER.Services
{
    public class ExamScheduleService : IExamScheduleService
    {
        private readonly ExamScheduleRepo _examScheduleRepo;
        private readonly IMapper _mapper;

        public ExamScheduleService(ExamScheduleRepo examScheduleRepo, IMapper mapper)
        {
            _examScheduleRepo = examScheduleRepo;
            _mapper = mapper;
        }

        public IEnumerable<ExamScheduleResponse> GetExamSchedules()
        {
            var schedules = _examScheduleRepo.GetExamSchedules();
            return _mapper.Map<IEnumerable<ExamScheduleResponse>>(schedules);
        }

        public ExamScheduleResponse? GetExamScheduleById(int id)
        {
            var schedule = _examScheduleRepo.GetExamScheduleById(id);
            return schedule != null ? _mapper.Map<ExamScheduleResponse>(schedule) : null;
        }

        public void CreateExamSchedule(ExamScheduleRequest request)
        {
            var schedule = _mapper.Map<ExamSchedule>(request);
            schedule.Active = true;
            _examScheduleRepo.CreateExamSchedule(schedule);
        }

        public void UpdateExamSchedule(int id, ExamScheduleRequest request)
        {
            var existingSchedule = _examScheduleRepo.GetExamScheduleById(id);
            if (existingSchedule != null)
            {
                _mapper.Map(request, existingSchedule);
                _examScheduleRepo.UpdateExamSchedule(existingSchedule);
            }
        }

        public void DeleteExamSchedule(int id)
        {
            _examScheduleRepo.DeleteExamSchedule(id);
        }
    }
}
