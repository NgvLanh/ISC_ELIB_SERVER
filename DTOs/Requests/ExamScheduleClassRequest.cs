namespace ISC_ELIB_SERVER.DTOs.Requests
{
    public class ExamScheduleClassRequest
    {
        public long ClassId { get; set; }
        public long ExamScheduleId { get; set; }
        public long SupervisoryTeacherId { get; set; }
    }
}
